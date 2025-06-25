namespace Forum.Web.Services.Contracts
{
    public interface IContentModerationService
    {
        bool IsRudeAsync(string text);
    }
}
