using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class UserResponseDto
{
    public int Id { get; set; }
    
    public required string Username { get; set; }

    public required string Email { get; set; }

    public string Role { get; set; }

    public bool Status { get; set; } // Represents if the user is active
}