using System.ComponentModel.DataAnnotations;
namespace TodoApi.DTOs;

public class UserRegisterDto
{
    public class UsersRegisterDto
    {
        [Required] public string Username { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }
        [Required] public string Password { get; set; }

        public bool isActive { get; set; } = true;
    }
}