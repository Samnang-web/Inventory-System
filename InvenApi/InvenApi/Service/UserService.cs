using InvenApi.DTOs;
using InvenApi.Helpers;
using InvenApi.Models;
using InvenApi.Repository;
using System.Security.Cryptography;
using System.Text;

namespace InvenApi.Service
{
    public class UserService : IUserService
    {
        private readonly TokenService _token;
        private readonly IUserRepository _repo;

        public UserService(TokenService token, IUserRepository repo)
        {
            _token = token;
            _repo = repo;
        }

        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _repo.GetAllUserAsync();
        }

        public async Task<UserLoginResponse?> LoginAsync(UserLoginDto dto)
        {
            var user = await _repo.GetUserByEmail(dto.Email);
            if (user == null)
                throw new Exception("User not found with the given email.");

            if (!VerifyPassword(dto.Password, user.PasswordHash))
                throw new Exception("Invalid password.");

            var token = _token.GenerateToken(user.Id.ToString(), user.Username, user.Role);

            return new UserLoginResponse
            {
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
                Token = token
            };
        }

        public async Task<UserRegisterDto?> RegisterAsync(UserRegisterDto dto)
        {
            var existingUser = await _repo.GetUserByEmail(dto.Email);
            if (existingUser != null)
                throw new Exception("Email is already registered.");

            var hashedPassword = HashPassword(dto.Password ?? throw new ArgumentNullException(nameof(dto.Password)));

            var user = new Users
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            user.Id = await _repo.CreateUserAsync(user);

            return new UserRegisterDto
            {
                Username = user.Username,
                Email = user.Email,
                Password = null ,
                Role = dto.Role,
            };
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storeHash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == storeHash;
        }
    }
}
