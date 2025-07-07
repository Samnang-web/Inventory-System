using InvenApi.DTOs;
using InvenApi.Helpers;
using InvenApi.Models;
using InvenApi.Repository;

namespace InvenApi.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IFileUploadService _fileUpload;
        public ProductService(IFileUploadService fileUpload, IProductRepository repo)
        {
            _repo = repo;
            _fileUpload = fileUpload;
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            return await _repo.GetAllProductsAsync();
        }

        public async Task<Products?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(CreateProductDto dto)
        {
            var imageUrl = await _fileUpload.UploadImageAsync(dto.Image_Url, "products");

            var product = new Products
            {
               
                Name = dto.Name,
                Sku = dto.Sku,
                Description = dto.Description,
                //Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                Image_Url = imageUrl,
                Category_Id = dto.Category_Id,
                CreatedAt = DateTime.Now
            };
            var id = await _repo.CreateAsync(product);
            return id;
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Product not found!");

            var imageUrl = await _fileUpload.UploadImageAsync(dto.Image_Url, "products");


            existing.Name = dto.Name;
            existing.Sku = dto.Sku;
            existing.Description = dto.Description;
            //existing.Quantity = dto.Quantity;
            existing.UnitPrice = dto.UnitPrice;
            existing.Image_Url = imageUrl ?? existing.Image_Url;
            existing.Category_Id = dto.Category_Id;



            return await _repo.UpdateAsync(existing);
           
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

    }
}
