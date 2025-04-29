using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdviceController : ControllerBase
    {
        private readonly IAdviceService _adviceService;

        public AdviceController(IAdviceService adviceService)
        {
            _adviceService = adviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdvice()
        {
            var advice = await _adviceService.GetRandomAdvice();
            return Ok(new { advice });
        }
    }
}