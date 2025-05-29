using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class UserRegisterDto
{
    [Required] public required string Username { get; set; }

    [Required] 
    [EmailAddress] 
    public required string Email { get; set; }

    [Required] 
    public required string Password { get; set; }

    public bool Status { get; set; } // Renamed from isActive
    
    [Required] public required string FirstName { get; set; }

    [Required] public required string LastName { get; set; }

    [Required] public string Address { get; set; }
}