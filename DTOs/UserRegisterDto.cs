using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class UserRegisterDto
{
    [Required] public string Username { get; set; }

    [Required] 
    [EmailAddress] 
    public string Email { get; set; }

    [Required] 
    public string Password { get; set; }

    public bool Status { get; set; } // Renamed from isActive
    
    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    [Required] public string Address { get; set; }
}