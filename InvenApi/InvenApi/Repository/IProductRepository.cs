using InvenApi.Models;

namespace InvenApi.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllProductsAsync();
        Task<Products?> GetByIdAsync(int id);
        Task<int> UpdateProductQuantityAsync(int productId, int quantity);
        Task<int> CreateAsync(Products product);
        Task<bool> UpdateAsync(Products product);
        Task<bool> DeleteAsync(int id);
    }
}
