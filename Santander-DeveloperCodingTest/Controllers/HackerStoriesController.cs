using Microsoft.AspNetCore.Mvc;
using Services.HackerNews;

namespace Santander_DeveloperCodingTest.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class HackerStoriesController : ControllerBase
    {
        private readonly HackerStoriesItems _hackerStoriesItems;
        public HackerStoriesController(HackerStoriesItems hackerStoriesItems)
        {
            _hackerStoriesItems = hackerStoriesItems;
        }
        [HttpGet("best")]
        public async Task<IActionResult> GetBestStories([FromQuery] int limit = 5)
        {
            var result = await _hackerStoriesItems.GetBestStoriesAsync(limit);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}
