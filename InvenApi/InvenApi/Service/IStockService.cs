using InvenApi.DTOs;
using InvenApi.Models;

namespace InvenApi.Service
{
    public interface IStockService
    {
        Task<IEnumerable<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateStockDto dto);
        Task<bool> UpdateAsync(int id, UpdateStockDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
