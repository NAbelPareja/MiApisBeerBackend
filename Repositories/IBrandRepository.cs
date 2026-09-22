using MiApisBeer.Models;

namespace MiApisBeer.Repositories
{
    public interface IBrandRepository
    {
         Task<IEnumerable<Brand>> GetAllAsyncc();
         Task<bool> NameExistsAsync(string name);
         Task<Brand?> BrandExistsAsync(int brandId);
         Task AddAsync(Brand brand);
         Task UpdateAsync();
         Task DeleteAsync(Brand brand);
    }
}
