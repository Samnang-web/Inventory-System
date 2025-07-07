using InvenApi.DTOs;
using InvenApi.Models;
using InvenApi.Repository;

namespace InvenApi.Service
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _repo;
        private readonly IProductRepository _productrepo;
        public StockService(IStockRepository repo, IProductRepository productrepo)
        {
            _repo = repo;
            _productrepo = productrepo;
        }

        public async Task<IEnumerable<Stock>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(CreateStockDto dto)
        {
            var stock = new Stock
            {
                Product_Id = dto.Product_Id,
                Quantity = dto.Quantity,
                Types = dto.Types,
                Note = dto.Note,
                Created_by = dto.Created_by,
                CreatedAt = DateTime.Now
            };
            var id =await _repo.CreateAsync(stock);
            await RecalculateAndUpdateProductQuantity(dto.Product_Id);
            return id;
        }

        public async Task<bool> UpdateAsync(int id, UpdateStockDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Stock not found!");

            existing.Product_Id = dto.Product_Id;
            existing.Quantity = dto.Quantity;
            existing.Types = dto.Types;
            existing.Note = dto.Note;
            existing.Created_by = dto.Created_by;
            existing.CreatedAt = DateTime.Now;

            var result = await _repo.UpdateAsync(existing);
            if (result)
                await RecalculateAndUpdateProductQuantity(dto.Product_Id);
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            var result = await _repo.DeleteAsync(id);
            if (result)
                await RecalculateAndUpdateProductQuantity(existing.Product_Id);
            return result;
        }

        private async Task RecalculateAndUpdateProductQuantity(int productId)
        {
            var totalIn = await _repo.GetTotalQuantityByTypeAsync(productId, "IN");
            var totalOut = await _repo.GetTotalQuantityByTypeAsync(productId, "OUT");

            var finalQuantity = totalIn - totalOut;
            await _productrepo.UpdateProductQuantityAsync(productId, finalQuantity);
        }

    }
}
