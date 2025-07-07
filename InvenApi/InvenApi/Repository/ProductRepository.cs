using Dapper;
using InvenApi.Models;
using System.Data;

namespace InvenApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _db;
        public ProductRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Products>> GetAllProductsAsync()
        {
            var sql = @"SELECT * FROM Products";
            return await _db.QueryAsync<Products>(sql);
        }

        public async Task<Products?> GetByIdAsync(int id)
        {
            var sql = @"SELECT * FROM Products WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Products>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(Products product)
        {
            var sql = @"INSERT INTO Products (Name, Sku, Description, Quantity, UnitPrice, Image_Url, Category_Id, CreatedAt)
                    VALUES (@Name, @Sku, @Description, @Quantity, @UnitPrice, @Image_Url, @Category_Id, @CreatedAt) RETURNING Id";
            return await _db.ExecuteScalarAsync<int>(sql, product);
        }

        public async Task<bool> UpdateAsync(Products product)
        {
            var sql = @"UPDATE Products SET Name = @Name, Sku = @Sku, Description = @Description, Quantity = @Quantity, UnitPrice = @UnitPrice, Image_Url = @Image_Url, Category_Id = @Category_Id, CreatedAt = @CreatedAt WHERE Id = @Id";
            var rows = await _db.ExecuteAsync(sql, product);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"DELETE FROM Products WHERE Id = @Id";
            var rows = await _db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<int> UpdateProductQuantityAsync(int productId, int quantity)
        {
            var sql = @"UPDATE Products SET Quantity = @Quantity WHERE Id = @ProductId";
            return await _db.ExecuteAsync(sql, new {Quantity = quantity, ProductId=productId });
        }
    }
}
