using InvenApi.DTOs;
using InvenApi.Models;
using InvenApi.Repository;

namespace InvenApi.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };
            await _repo.CreateAsync(category);
            return category.Id;
        }

        public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Category not found!");

            existing.Name = dto.Name;

            return await _repo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        
    }
}
