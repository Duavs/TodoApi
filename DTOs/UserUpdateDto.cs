namespace TodoApi.DTOs;

public class UserUpdateDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    // 🚫 Do NOT include PasswordHash here
}