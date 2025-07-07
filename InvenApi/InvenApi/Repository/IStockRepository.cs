using InvenApi.Models;

namespace InvenApi.Repository
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(int id);
        Task<int> GetTotalQuantityByTypeAsync(int productId, string type);
        Task<int> CreateAsync(Stock stock);
        Task<bool> UpdateAsync(Stock stock);
        Task<bool> DeleteAsync(int id);
    }
}
