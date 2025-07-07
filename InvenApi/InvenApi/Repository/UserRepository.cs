using Dapper;
using InvenApi.Models;
using System.Data;

namespace InvenApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;
        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> CreateUserAsync(Users user)
        {
            var sql = @"INSERT INTO Users (Username, Email, PasswordHash, Role, CreatedAt) VALUES (@Username, @Email, @PasswordHash, @Role, @CreatedAt) RETURNING Id";
            return await _db.QuerySingleAsync<int>(sql, user);
        }

        public async Task<IEnumerable<Users>> GetAllUserAsync()
        {
            var sql = @"SELECT * FROM Users";
            return await _db.QueryAsync<Users>(sql);
        }

        public async Task<Users?> GetUserByEmail(string email)
        {
            var sql = @"SELECT * FROM Users WHERE Email = @Email";
            return await _db.QueryFirstOrDefaultAsync<Users>(sql, new {Email=email});
        }
    }
}
