using MiApisBeer.Models;

namespace MiApisBeer.Repositories
{
    public interface IProveedoreRepository
    {
        Task<IEnumerable<Proveedore>> GetAllAsync();
        Task<bool> NameExistsAsync(string name);
        Task<Proveedore?> ProveedoreExistsAsync(int proveedoreId);
        Task AddAsync(Proveedore proveedore);
        Task UpdateAsync();
        Task DeleteAsync(Proveedore proveedore);
    }
}
