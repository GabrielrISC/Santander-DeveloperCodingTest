using Models;
using Models.DTOs;
using Models.HackerNews;
using System.Text.Json;

namespace Services.HackerNews
{
    public class HackerStoriesItems
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/")
        };

        public static async Task<ResultTask<List<HackerNewStory>>> GetBestStoriesAsync(int limit)
        {

            try
            {
                if (limit <= 0)
                    return ResultTask<List<HackerNewStory>>.Failure("Limit only can be > 0");

                var idsResponse = await _httpClient.GetStringAsync("beststories.json");
                var storyIds = JsonSerializer.Deserialize<List<int>>(idsResponse);

                if (storyIds == null || storyIds.Count == 0)
                    return ResultTask<List<HackerNewStory>>.Failure("best stories not found");

                var limitedIds = storyIds.Take(Math.Min(limit, storyIds.Count)).ToList();

                var stories = new List<HackerNewStory>();

                foreach (var id in limitedIds)
                {
                    var itemResponse = await _httpClient.GetStringAsync($"item/{id}.json");
                    var story = JsonSerializer.Deserialize<HackerStoryItem>(itemResponse);

                    if (story != null)
                        stories.Add(new HackerNewStory(story));
                }

                return ResultTask<List<HackerNewStory>>.Success(stories);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return ResultTask<List<HackerNewStory>>.Failure("NetworkError");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error JSON: {ex.Message}");
                return ResultTask<List<HackerNewStory>>.Failure("Deserialize Error");
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Timeout: {ex.Message}");
                return ResultTask<List<HackerNewStory>>.Failure("HackerNews TimeOut");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return ResultTask<List<HackerNewStory>>.Failure("Result Not Valid");
            }
        }
    }
}
