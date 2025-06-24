using Forum.Web.Dtos;
using Forum.Web.Dtos.Thread;
using Forum.Web.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml.Linq;

namespace Forum.Web.Controllers
{
    [Controller]
    [Route("api/threads")]
    public class ThreadsController : ControllerBase
    {
        private readonly IThreadService threads;
        private readonly IPostService posts;
        public ThreadsController(IThreadService threads, IPostService posts)
        {
            this.threads = threads;
            this.posts = posts;
        }

        // GET /api/v1/threads?limit=15&before=2025-06-23T12:00:00Z
        [HttpGet]
        public async Task<IActionResult> GetRecent(
            [FromQuery] int limit = 15,
            [FromQuery] DateTime? before = null)
        {
            var (threads, nextCursor) =
                await this.threads.GetRecentAsync(limit, before);
            Response.Headers.Append("X-Next-Cursor", nextCursor ?? "");
            return Ok(threads);
        }

        // GET /api/v1/threads/{id}?limit=15&beforePosts=...
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(
            long id,
            [FromQuery(Name = "limit")] int limit = 15,
            [FromQuery(Name = "beforePosts")] DateTime? beforePosts = null)
        {
            var thread = await threads.GetByIdAsync(id, limit, beforePosts);
            if (thread == null) return NotFound();
            return Ok(thread);
        }

        // POST /api/v1/threads
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateThreadDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var newId = await threads.CreateAsync(userId, dto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = newId },
                null);
        }

        // POST /api/v1/threads/{id}/posts
        [HttpPost("{id:long}/posts")]
        [Authorize]
        public async Task<IActionResult> AddPost(
            long id,
            [FromBody] CreatePostDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var postId = await posts.AddAsync(userId, id, dto);
            return CreatedAtAction(
                actionName: nameof(PostsController.Index),
                controllerName: "Posts",
                routeValues: new { threadId = id },
                value: null);
        }
    }
}
