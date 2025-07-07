using InvenApi.Models;

namespace InvenApi.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllUserAsync();
        Task<Users?> GetUserByEmail(string email);
        Task<int> CreateUserAsync(Users user);
    }
}
