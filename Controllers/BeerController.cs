using MiApisBeer.DTO;
using MiApisBeer.Models;
using MiApisBeer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace MiApisBeer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BeerController : ControllerBase
    {

        private readonly IBeerRepository _beerRepository;
        public BeerController(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerDto>>> GetBeeers()
        {
            var beers = await _beerRepository.GetAllAsync();

            var beersDto = beers.Select(b => new BeerDto
            {
                BeerId = b.BeerId,
                Name = b.Name,
                BrandId = (int)b.BrandId,
                BrandName = b.Brand != null ? b.Brand.Name : "Sin Marca"
            });

            return Ok(beersDto);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] BeerInsertDto beerDto)
        {
            var brandExist = await _beerRepository.BrandExistsAsync(beerDto.BrandId);

            if (!brandExist)
            {
                return BadRequest("No existe la marca del producto"); 
            }

            var nameExist = await _beerRepository.NameExistsAsync(beerDto.Name);
            if (nameExist)
            {
                return BadRequest("Ya existe el nombre de la cerveza");
            }


            var beer = new Beeer
                {
                    Name = beerDto.Name,
                    BrandId = beerDto.BrandId
                };
            await _beerRepository.AddAsync(beer);
            return CreatedAtAction(nameof(GetBeeers), new { id = beer.BeerId }, beer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutBeer(int id, [FromBody] BeerUpdateDto beerDto)
        {
            if (id != beerDto.Id)
            {
                return BadRequest("El id del url o coincide con el del json");
            }

            var beerExist = await _beerRepository.BeerExistsAsync(id);

            if (beerExist == null)
            {
                return NotFound("No existe la cerveza");
            }

            var brandExist = await _beerRepository.BrandExistsAsync(beerDto.BrandId);

            if (!brandExist)
            {
                return BadRequest("No existe la marca del producto");
            }

            beerExist.Name = beerDto.Name;
            beerExist.BrandId = beerDto.BrandId;

            await _beerRepository.UpdateAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBeer(int id)
        {
            var beerExist = await _beerRepository.BeerExistsAsync(id);

            if (beerExist == null)
            {
                return NotFound("No existe la cerveza");
            }

            await _beerRepository.DeleteAsync(beerExist);
            return NoContent();
        }
    }
}
