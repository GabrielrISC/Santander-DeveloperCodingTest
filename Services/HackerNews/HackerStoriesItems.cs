using Microsoft.Extensions.Caching.Memory;
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
            BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/"),
            Timeout = TimeSpan.FromSeconds(5),
        };
        private readonly IMemoryCache _cache;
        private const string BestStoriesIdsCacheKey = "BestStoriesIds";
        private static readonly MemoryCacheEntryOptions IdsCacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
        public HackerStoriesItems(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async Task<ResultTask<List<HackerNewStory>>> GetBestStoriesAsync(int limit)
        {

            try
            {
                if (limit <= 0)
                    return ResultTask<List<HackerNewStory>>.Failure("Limit only can be > 0");

                var storyIds = await GetBestStoryIdsAsync();

                if (storyIds == null || storyIds.Count == 0)
                    return ResultTask<List<HackerNewStory>>.Failure("best stories not found");

                var limitedIds = storyIds.Take(Math.Min(limit, storyIds.Count)).ToList();

                var storyTasks = limitedIds.Select(id => GetStoryItemAsync(id)).ToList();

                var storyResults = await Task.WhenAll(storyTasks);

                var stories = storyResults.Where(story => story != null).Select(story => new HackerNewStory(story)).ToList();

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
        private async Task<List<int>?> GetBestStoryIdsAsync()
        {
            return await _cache.GetOrCreateAsync(BestStoriesIdsCacheKey, async entry =>
            {
                entry.SetOptions(IdsCacheOptions);
                var idsResponse = await _httpClient.GetStringAsync("beststories.json");
                return JsonSerializer.Deserialize<List<int>>(idsResponse);
            });
        }

        private async Task<HackerStoryItem?> GetStoryItemAsync(int id)
        {
            string cacheKey = $"StoryItem_{id}";

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) 
            };

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetOptions(options);
                var itemResponse = await _httpClient.GetStringAsync($"item/{id}.json");
                return JsonSerializer.Deserialize<HackerStoryItem>(itemResponse);
            });
        }
    }
}
