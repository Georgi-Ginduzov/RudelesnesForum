using Azure;
using Forum.Web.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    [ApiController]
    [Route("api/threads/{threadId:long}/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _posts;
        public PostsController(IPostService posts)
            => _posts = posts;

        // GET /api/v1/threads/{threadId}/posts?limit=15&before=...
        [HttpGet]
        public async Task<IActionResult> GetForThread(
            long threadId,
            [FromQuery] int limit = 15,
            [FromQuery] DateTime? before = null)
        {
            var (list, next) =
                await _posts.GetForThreadAsync(threadId, limit, before);
            Response.Headers.Append("X-Next-Cursor", next ?? "");
            return Ok(list);
        }
    }
}
