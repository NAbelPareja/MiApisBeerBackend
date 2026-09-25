using MiApisBeer.DTO;
using MiApisBeer.Models;
using MiApisBeer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MiApisBeer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {

        private readonly IBrandRepository _brandRepository;
        public BrandController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
        {
            var brands = await _brandRepository.GetAllAsyncc();
              
            var brandsDto = brands.Select(b => new BrandDto
            {
                BrandId = b.BrandId,
                Name = b.Name,
                proveedorId = b.ProveedoresId ?? 0,
                ProveedorName = b.Proveedores != null ? b.Proveedores.Name : "Sin Proveedor"
            });
            return Ok(brandsDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult> Add([FromBody] BrandInsertDto brandDto)
        {
            var nameExist = await _brandRepository.NameExistsAsync(brandDto.Name);
            if (nameExist)
            {
                return BadRequest("Ya existe el nombre de la marca");
            }


            var brand = new Brand
            {
                Name = brandDto.Name,
                ProveedoresId = brandDto.ProveedorId
            };

            await _brandRepository.AddAsync(brand);
            return CreatedAtAction(nameof(GetBrands), new { id = brand.BrandId }, brand);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutBrand(int id, [FromBody] BrandUpdateDto brandDto)
        {

            if(id != brandDto.Id)
            {
                return BadRequest("El id del url no conincide con el del json");
            }

            var brandExist = await _brandRepository.BrandExistsAsync(id);
            if (brandExist.Name == null)
            {
                return NotFound("La marca que intentas actualizar no existe.");
            }

            var proveedorExist = await _brandRepository.ProveedoreExistsAsync(brandDto.ProveedoresId);
            if (!proveedorExist)
            {
                return BadRequest("No existe la proveedora");
            }

            brandExist.Name = brandDto.Name;
            brandExist.ProveedoresId = brandDto.ProveedoresId;

            await _brandRepository.UpdateAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var brandExist = await _brandRepository.BrandExistsAsync(id);
            if (brandExist == null)
            {
                return NotFound("El valor a eliminar no existe");
            }
            await _brandRepository.DeleteAsync(brandExist);
           
            return NoContent();
        }
    }
}
