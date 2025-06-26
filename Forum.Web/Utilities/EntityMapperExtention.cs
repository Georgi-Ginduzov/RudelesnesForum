using Forum.Web.Data.Entities;
using Forum.Web.Models;

namespace Forum.Web.Utilities
{
    public static class EntityMapperExtention
    {
        public static LatestDiscussion ToDto(this Post post)
        {
            var replies = post.Replies;
            var lastReply = replies?.LastOrDefault();
            var authorName = post.User?.UserName ?? post?.User?.Email;
            var replyCount = replies is null ? 0 : replies.Count(r => r.IsReviewed == true);


            return new LatestDiscussion()
            {
                Id = post.PostId,
                Title = post.Title,
                AuthorName = authorName!,
                CreatedAt = post.CreatedAt,
                ReplyCount = replyCount,
                LastReplyBy = lastReply?.User?.UserName ?? lastReply?.User?.Email!,
                LastReplyAt = lastReply?.CreatedAt,
                Category = "Category column needs to be added",
                IsPinned = true,
                IsLocked = false,
            };
        }

        public static IEnumerable<LatestDiscussion> ToDtos(this IEnumerable<Post> posts)
        {
            var mapped = new List<LatestDiscussion>();
            foreach (var post in posts) 
                mapped.Add(ToDto(post));
            return mapped;
        }
    }
}
