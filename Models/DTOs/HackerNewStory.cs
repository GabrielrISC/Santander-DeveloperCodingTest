using Models.HackerNews;

namespace Models.DTOs
{
    public class HackerNewStory
    {
        public string title { get; set; } = string.Empty;
        public string uri { get; set; } = string.Empty;
        public string postedBy { get; set; } = string.Empty;
        public DateTime time { get; set; }
        public int score { get; set; }
        public int commentCount { get; set; }
        public HackerNewStory()
        {
        }
        public HackerNewStory(HackerStoryItem hackerStoryItem)
        {
            this.title = hackerStoryItem.title ?? string.Empty;
            this.uri = hackerStoryItem.url ?? string.Empty;
            this.postedBy = hackerStoryItem.by ?? string.Empty;
            this.time = DateTimeOffset.FromUnixTimeSeconds(hackerStoryItem.time).DateTime;
            this.score = hackerStoryItem.score;
            this.commentCount = hackerStoryItem.descendants;
        }
    }
}
