using Azure;
using Forum.Web.Data.Entities;
using Forum.Web.Dtos.Flag;
using Forum.Web.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Web.Controllers
{
    [ApiController]
    [Route("api/v1/flags")]
    [Authorize(Roles = "Moderator,Admin")]
    public class FlagsController : ControllerBase
    {
        private readonly IFlagService _flags;
        public FlagsController(IFlagService flags)
            => _flags = flags;

        // GET /api/v1/flags?limit=20&before=2025-06-23T12:00:00Z
        [HttpGet]
        public async Task<IActionResult> GetFlags(
            [FromQuery] int limit = 20,
            [FromQuery] DateTime? before = null)
        {
            var (list, next) = await _flags.GetFlagsAsync(limit, before);
            if (!string.IsNullOrEmpty(next))
                Response.Headers.Append("X-Next-Cursor", next);
            return Ok(list);
        }

        // GET /api/v1/flags/{scanId}
        [HttpGet("{scanId:long}")]
        public async Task<IActionResult> GetFlag(long scanId)
        {
            var detail = await _flags.GetFlagByIdAsync(scanId);
            if (detail is null)
                return NotFound();
            return Ok(detail);
        }

        // POST /api/v1/flags/{scanId}/reviews
        [HttpPost("{scanId:long}/reviews")]
        public async Task<IActionResult> AddReview(
            long scanId,
            [FromBody] CreateReviewDto dto)
        {
            var reviewerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _flags.AddReviewAsync(reviewerId, scanId, dto.ShouldBePosted);
            return NoContent();
        }
    }
}
