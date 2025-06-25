using Forum.Web.Models;
using Forum.Web.Services;
using Forum.Web.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdministrationService administrationService;

        public AdminController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        public async Task<IActionResult> Index()
        {
            /*var flaggedReplies = await administrationService.GetFlaggedReplies();
            var flaggedRepliesViewModel = flaggedReplies.Select(x => new ReplyModerationViewModel
            {
                CreatedAt = x.CreatedAt,
                User = x.User,
                UserId = x.UserId,
                PostId = (int)x.PostId,
                PostTitle = x.Post.Title,
                Content = x.Content,
                Id = x.Id,
            }).ToList();*/
            return View("FlaggedReplies", /*flaggedRepliesViewModel*/ null);
        }

        [HttpPost]
        public async Task<IActionResult> Review(int replyId, bool approve)
        {
            //await administrationService.ReviewReply(replyId, approve);
            return RedirectToAction(nameof(Index));
        }
    }
}
