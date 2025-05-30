using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public class TodoItem
{
    public int Id { get; set; }
    public required string Task { get; set; } = string.Empty;
    public required string TaskDetail { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public required int UserId { get; set; }
    public required int TaskTypeId { get; set; }
    public required int TaskSeverityId { get; set; }
    public required int TaskStatusId { get; set; }
}