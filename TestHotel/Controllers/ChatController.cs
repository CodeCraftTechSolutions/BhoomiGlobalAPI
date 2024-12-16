using BhoomiGlobalAPI.DTOs;
using BhoomiGlobalAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace BhoomiGlobalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly OpenAIService _openAIService;

        public ChatController(OpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _openAIService.SendRequestAsync(request.Prompt);
                return Ok(new { success = true, response });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }

}
