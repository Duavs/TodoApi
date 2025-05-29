using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace TodoApi.Models;

public class User
{

    public int Id{get;set;}
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string FirstName { get; set; }
    
    [Required]
    public required string LastName { get; set; }
    
    public required string Address { get; set; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [JsonIgnore]
    public string PasswordHash { get; set; }
    
    public string Role { get; set; } = "User";  
    
    //Hash password before storing
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
    public bool Status{get;set;}
}