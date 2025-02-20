// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using TodoApi.Data;
// using TodoApi.Models;
//
// namespace TodoApi.Controllers;
//
// public class TodoController
// {
//     [Route("api/todos")]
//     [ApiController]
//     public class TodoController : ControllerBase
//     {
//         private readonly TodoDbContext _context;
//
//         public TodoController(TodoDbContext context)
//         {
//             _context = context;
//         }
//
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
//         {
//             return await _context.TodoItems.ToListAsync();
//         }
//         
//         [HttpGet("id")]
//         public async Task<ActionResult<TodoItem>> GetTodayBy(int id)
//         {
//             var todo = await _context.TodoItems.FindAsync(id);
//             if (todo == null) return NotFound();
//             return todo;
//         }
//     }
// }