using Dapper;
using InvenApi.Models;
using System.Data;

namespace InvenApi.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly IDbConnection _db;
        public StockRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Stock>> GetAllAsync()
        {
            var sql = @"SELECT * FROM Stock_Transactions";
            return await _db.QueryAsync<Stock>(sql);
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            var sql = @"SELECT * FROM Stock_Transactions WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Stock>(sql, new {Id=id});
        }

        public async Task<int> CreateAsync(Stock stock)
        {
            var sql = @"INSERT INTO Stock_Transactions (Product_Id, Quantity, Types, Note, Created_by, CreatedAt)
                       VALUES (@Product_Id, @Quantity, @Types, @Note, @Created_by, @CreatedAt) RETURNING Id";
            return await _db.ExecuteScalarAsync<int>(sql, stock);
        }

        public async Task<bool> UpdateAsync(Stock stock)
        {
            var sql = @"UPDATE Stock_Transactions SET Product_Id = @Product_Id, Quantity = @Quantity, Types = @Types, Note = @Note, Created_by = @Created_by, CreatedAt = @CreatedAt WHERE Id = @Id";
            var rows = await _db.ExecuteAsync(sql, stock);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"DELETE FROM Stock_Transactions WHERE Id = @Id";
            var rows = await _db.ExecuteAsync(sql, new {Id=id});
            return rows > 0;
        }

        public async Task<int> GetTotalQuantityByTypeAsync(int productId, string type)
        {
            var sql = @"SELECT COALESCE(SUM(Quantity), 0) FROM Stock_Transactions WHERE Product_Id = @Product_Id AND Types = @Types";
            return await _db.ExecuteScalarAsync<int>(sql, new { Product_Id = productId, Types = type });
        }
    }
}
