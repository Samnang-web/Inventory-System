using Dapper;
using InvenApi.Models;
using System.Data;

namespace InvenApi.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnection _db;
        public CategoryRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var sql = @"SELECT * FROM Categories";
            return await _db.QueryAsync<Category>(sql);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var sql = @"SELECT * FROM Categories WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Category>(sql, new {Id=id});
        }
        public async Task<int> CreateAsync(Category category)
        {
            var sql = @"INSERT INTO Categories (Name) VALUES (@Name) RETURNING Id";
            return await _db.ExecuteScalarAsync<int>(sql, category);
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            var sql = @"UPDATE Categories SET Name = @Name";
            var rows = await _db.ExecuteAsync(sql, category);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"DELETE FROM Category WHERE Id = @Id";
            var rows = await _db.ExecuteAsync(sql, new {Id=id});
            return rows > 0;
        }

    }
}
