using System.ComponentModel.DataAnnotations;

namespace InvenApi.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }

        public string Role { get; set; }
    }

    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }

    public class UserLoginResponse
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
    }
}
