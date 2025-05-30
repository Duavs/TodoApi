using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers;

    [Route("api/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext _context;

        public TodoController(TodoDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
        {
            var todos = await _context.TodoItems.Where(t => t.IsDeleted == false).ToListAsync();
            return Ok(todos);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoById(int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);
            if (todo == null) return NotFound();
            return todo;
        }
        [HttpPost]
        public async Task<ActionResult<TodoItem>> AddTodo([FromBody] TodoItem todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // ✅ return 400 Bad Request if model is invalid
            }

            try
            {
                _context.TodoItems.Add(todo);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Internal server error: {ex.Message}");
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, TodoItem todo)
        {
            if (id != todo.Id) return BadRequest();
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteTask(int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);
            if (todo == null) return NotFound();

            todo.IsDeleted = true;
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
