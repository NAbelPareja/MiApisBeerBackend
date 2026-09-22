using MiApisBeer.Models;
using Microsoft.EntityFrameworkCore;

namespace MiApisBeer.Repositories
{
    public class BrandRepository: IBrandRepository
    {
        private readonly PubContext _context;

        public BrandRepository(PubContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Brand>> GetAllAsyncc()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _context.Brands.AnyAsync(b => b.Name == name);
        }
        public async Task<Brand?> BrandExistsAsync(int brandId)
        {
            return await _context.Brands.FindAsync(brandId);
        }


        public async Task AddAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Brand brand)
        {
            _context.Brands.RemoveRange(brand);
            await _context.SaveChangesAsync();
        }

    }
}
