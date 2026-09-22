using System.ComponentModel.DataAnnotations;

namespace MiApisBeer.DTO
{
    public class UserAuthDto
    {
        [Required(ErrorMessage = "El campo email es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo email no es válido")]
        public string email { get; set; } = null;
        [Required(ErrorMessage = "El campo password es obligatorio")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 20 caracteres.")]
        public string password { get; set; } = null;
    }
}
