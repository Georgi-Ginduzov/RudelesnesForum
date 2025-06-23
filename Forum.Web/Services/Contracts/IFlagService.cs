namespace Forum.Web.Services.Contracts
{
    public interface IFlagService
    {
        /// <summary>
        /// List pending/flagged scans, newest first.
        /// </summary>
        Task<(IEnumerable<FlagDto> Flags, string? NextCursor)>
            GetFlagsAsync(int limit, DateTime? before);

        /// <summary>
        /// Get full details for one scan, including any reviews.
        /// </summary>
        Task<FlagDetailDto?>
            GetFlagByIdAsync(long scanId);

        /// <summary>
        /// Add a moderator review and update scan status accordingly.
        /// </summary>
        Task AddReviewAsync(int reviewerId, long scanId, bool shouldBePosted);
    }
}
