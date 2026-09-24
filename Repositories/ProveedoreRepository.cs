using MiApisBeer.Models;
using Microsoft.EntityFrameworkCore;

namespace MiApisBeer.Repositories
{
    public class ProveedoreRepository: IProveedoreRepository
    {
        private readonly PubContext _context;
        public ProveedoreRepository(PubContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Proveedore>> GetAllAsync()
        {
            return await _context.Proveedores.ToListAsync();
        }
        public async Task<bool> NameExistsAsync(string name)
        {
            return await _context.Proveedores.AnyAsync(b => b.Name == name);
        }
        public async Task<Proveedore?> ProveedoreExistsAsync(int proveedoreId)
        {
            return await _context.Proveedores.FindAsync(proveedoreId);

        }
        public async Task AddAsync(Proveedore proveedore)
        {
            _context.Proveedores.Add(proveedore);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Proveedore proveedore)
        {
            _context.Proveedores.Remove(proveedore);
            await _context.SaveChangesAsync();
        }
    }
}
