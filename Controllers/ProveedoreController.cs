using MiApisBeer.DTO;
using MiApisBeer.Models;
using MiApisBeer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiApisBeer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoreController : ControllerBase
    {
        private readonly IProveedoreRepository _proveedoreRepository;
        public ProveedoreController(IProveedoreRepository proveedoreRepository)
        {
            _proveedoreRepository = proveedoreRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedore>>> GetProveedores()
        {
            var proveedores = await _proveedoreRepository.GetAllAsync();
            return Ok(proveedores);
        }

        [HttpPost]
        [Authorize(Roles ="Admin, Employee")]
        public async Task<ActionResult> Add([FromBody] ProveedoreInsertDto proveedoredto)
        {
            var nameExist = await _proveedoreRepository.NameExistsAsync(proveedoredto.Name);
            if (nameExist == true)
            {
                return BadRequest("Ya existe el nombre del proveedor");
            }

            var proveedore = new Proveedore
            {
                Name = proveedoredto.Name,
            };

            _proveedoreRepository.AddAsync(proveedore);
            return CreatedAtAction(nameof(GetProveedores), new { id = proveedore.ProveedoresId}, proveedoredto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] ProveedoreUpdateDto proveedore)
        {
            if(id != proveedore.Id)
            {
                return BadRequest("El id del url no conincide con el del json");
            }
            var proveedorExist = await _proveedoreRepository.ProveedoreExistsAsync(proveedore.Id);
            if (proveedorExist == null)
            {
                return NotFound("El proveedor que intentas actualizar no existe.");
            }
            proveedorExist.Name = proveedore.Name;
            await _proveedoreRepository.UpdateAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var proveedorExist = await _proveedoreRepository.ProveedoreExistsAsync(id);
            if (proveedorExist == null)
            {
                return NotFound("El proveedor que intentas eliminar no existe.");
            }
            await _proveedoreRepository.DeleteAsync(proveedorExist);
            return NoContent();
        }

    }
}
