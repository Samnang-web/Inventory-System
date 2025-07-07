using InvenApi.DTOs;
using InvenApi.Models;

namespace InvenApi.Service
{
    public interface IUserService
    {
        Task<IEnumerable<Users>> GetAll();
        Task<UserLoginResponse?> LoginAsync(UserLoginDto dto);
        Task<UserRegisterDto?> RegisterAsync(UserRegisterDto dto);
    }
}
