using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<FM_TaskSeverity> FM_TaskSeverities { get; set; }
    public DbSet<FM_TaskStatus> FM_TaskStatuses { get; set; }
    public DbSet<FM_TaskType> FM_TaskTypes { get; set; }

}