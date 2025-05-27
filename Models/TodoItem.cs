using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Task { get; set; } = string.Empty;
    public string TaskDetail { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    [Required]
    public int UserId { get; set; }
    public int TaskTypeId { get; set; }
    public int TaskSeverityId { get; set; }
    
}