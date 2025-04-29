using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiController : ControllerBase
{
    private readonly IOpenAiService _openAiService;

    public AiController(IOpenAiService openAiService)
    {
        _openAiService = openAiService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateText([FromBody] string prompt)
    {
        var response = await _openAiService.GenerateTextAsync(prompt);
        return Ok(response);
    }
    [HttpGet("testgeneratetask")]
    public async Task<IActionResult> TestAi()
    {
        var testPrompt = "Suggest a simple task for improving focus.";
        var result = await _openAiService.GenerateTextAsync(testPrompt);

        if (string.IsNullOrWhiteSpace(result))
        {
            return BadRequest("No response from OpenAI service.");
        }

        return Ok(new { suggestion = result });
    }
}