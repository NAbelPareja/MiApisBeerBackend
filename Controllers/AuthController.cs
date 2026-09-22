using MiApisBeer.DTO;
using MiApisBeer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiApisBeer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PubContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(PubContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> PostAuth(UserAuthDto user)
        {
            var userExist = await _context.Users.AnyAsync(u => u.Email == user.email);
            if (userExist)
            {
                return BadRequest("El user ya esta incrito");
            }

            // encriptacion
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.password);

            var newUser = new User
            {
                Email = user.email,
                PasswordHash = passwordHash
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Creacion exitosa" });
        }

        [HttpPost("login")]
        public async Task<ActionResult> login([FromBody] UserAuthDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
            {
                return Unauthorized("El correo electrónico o la contraseña son incorrectos.");
            }

            // Generar token JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UsedId.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            // leemos la clave secreta que gaurdamos en el json
            var keyString = _configuration.GetSection("Jwt:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString!));
            // Configuramos los algoritmos de firmado para que nadie pueda falsificarlo
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // Armamos el token final con una duración (por ejemplo, expira en 1 día)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 4. Le devolvemos la llave digital (Token) al Frontend en un JSON
            return Ok(new
            {
                mensaje = "Login exitoso",
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
