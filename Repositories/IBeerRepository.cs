using MiApisBeer.Models;

namespace MiApisBeer.Repositories
{
    public interface IBeerRepository
    {
        Task<IEnumerable<Beeer>> GetAllAsync();
        Task<bool> NameExistsAsync(string name);
        Task<bool> BrandExistsAsync(int brandId);
        Task<Beeer?> BeerExistsAsync(int beerId);
        Task AddAsync(Beeer beer);
        Task UpdateAsync();
        Task DeleteAsync(Beeer beer);
    }
}
