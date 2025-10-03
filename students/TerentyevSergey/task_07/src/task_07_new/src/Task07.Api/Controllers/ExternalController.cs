using Microsoft.AspNetCore.Mvc;
using Task07.Api.Services;

namespace Task07.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalController : ControllerBase
    {
        private readonly IExternalApiService _externalApiService;
        private readonly ILogger<ExternalController> _logger;

        public ExternalController(IExternalApiService externalApiService, ILogger<ExternalController> logger)
        {
            _externalApiService = externalApiService;
            _logger = logger;
        }

        [HttpGet("posts")]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var posts = await _externalApiService.GetPostsAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ExternalController.GetPosts");
                return StatusCode(500, new { error = "Failed to fetch posts from external service" });
            }
        }

        [HttpGet("posts/{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            try
            {
                var post = await _externalApiService.GetPostAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ExternalController.GetPost for id {PostId}", id);
                return StatusCode(500, new { error = "Failed to fetch post from external service" });
            }
        }
    }
}