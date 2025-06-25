using Forum.Web.Models;
using Forum.Web.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    [Authorize(Roles = "Moderator,Admin")]
    public class ModerationController : Controller
    {
        private readonly IModerationService _moderationService;

        public ModerationController(IModerationService moderationService)
        {
            _moderationService = moderationService;
        }

        public async Task<IActionResult> Index()
        {
            var flaggedReplies = await _moderationService.GetFlaggedReplies();
            var flaggedRepliesViewModel = flaggedReplies.Select(x => new ReplyModerationViewModel
            {
                CreatedAt = x.CreatedAt,
                User = x.User,
                UserId = x.UserId,
                PostId = (int)x.PostId,
                PostTitle = x.Post.Title,
                Content = x.Content,
                Id = x.Id,
            }).ToList();
            return View("FlaggedReplies", flaggedRepliesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Review(int replyId, bool approve)
        {
            await _moderationService.ReviewReply(replyId, approve);
            return RedirectToAction(nameof(Index));
        }
    }
}
