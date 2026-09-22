using MiApisBeer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MiApisBeer.Repositories
{
    public class BeerRepository : IBeerRepository
    {
        private readonly PubContext _context;

        public BeerRepository(PubContext context){ 
            _context = context; 
        }
        public async Task<IEnumerable<Beeer>> GetAllAsync()
        {
            return await _context.Beeers.Include(b => b.Brand).ToListAsync();
        }

        public async Task<bool> BrandExistsAsync(int brandId)
        {
            return await _context.Brands.AnyAsync(b => b.BrandId == brandId);
        }

        public async Task<Beeer?> BeerExistsAsync(int beerId)
        {
            return await _context.Beeers.FindAsync(beerId);
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _context.Beeers.AnyAsync(b => b.Name == name);
        }

        //para agregar 
        public async Task AddAsync(Beeer beer)
        {
            _context.Beeers.Add(beer);
            await _context.SaveChangesAsync();
        }

        //para guardar lo editado
        public async Task UpdateAsync()
        {

            await _context.SaveChangesAsync();
        }
        // para eliminar
        public async Task DeleteAsync(Beeer beer)
        {
            _context.Beeers.RemoveRange(beer);
            await _context.SaveChangesAsync();
        }

    }
}
