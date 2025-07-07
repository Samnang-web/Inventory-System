using InvenApi.DTOs;
using InvenApi.Models;

namespace InvenApi.Service
{
    public interface IProductService
    {
        Task<IEnumerable<Products>> GetAllAsync();
        Task<Products?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateProductDto dto);
        Task<bool> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsync(int id);

    }
}
