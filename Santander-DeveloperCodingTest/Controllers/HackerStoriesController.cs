using Microsoft.AspNetCore.Mvc;
using Services.HackerNews;

namespace Santander_DeveloperCodingTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HackerStoriesController : ControllerBase
    {
        [HttpGet("best")]
        public async Task<IActionResult> GetBestStories([FromQuery] int limit = 5)
        {
            var result = await HackerStoriesItems.GetBestStoriesAsync(limit);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}
