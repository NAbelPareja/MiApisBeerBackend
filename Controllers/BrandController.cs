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
    [Authorize]
    public class BrandController : ControllerBase
    {

        private readonly IBrandRepository _brandRepository;
        public BrandController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetBrands()
        {
            var brands = await _brandRepository.GetAllAsyncc();
            return Ok(brands);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] BrandInsertDto brandDto)
        {
            var nameExist = await _brandRepository.NameExistsAsync(brandDto.Name);
            if (nameExist)
            {
                return BadRequest("Ya existe el nombre de la marca");
            }


            var brand = new Brand
            {
                Name = brandDto.Name
            };

            _brandRepository.AddAsync(brand);
            return CreatedAtAction(nameof(GetBrands), new { id = brand.BrandId }, brand);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutBrand(int id, [FromBody] BrandUpdateDto brandDto)
        {
            if(id != brandDto.Id)
            {
                return BadRequest("El id del url no conincide con el del json");
            }
            var nameExist = await _brandRepository.BrandExistsAsync(brandDto.Id);
            if (nameExist.Name == null)
            {
                return NotFound("La marca que intentas actualizar no existe.");
            }

            nameExist.Name = brandDto.Name;

            await _brandRepository.UpdateAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var brandExist = await _brandRepository.BrandExistsAsync(id);
            if (brandExist.BrandId == null)
            {
                return NotFound("El valor a eliminar no existe");
            }
            _brandRepository.DeleteAsync(brandExist);

            return NoContent();
        }
    }
}
