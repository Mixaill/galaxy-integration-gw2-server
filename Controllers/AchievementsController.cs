using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using GW2Integration.Server.Extensions;

namespace GW2Integration.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AchievementsController : ControllerBase
    {
        private ILogger<AchievementsController> _logger { get; }
        private IMemoryCache _cache { get; }

        private int ConnectionLimit { get; } = 16;
        private int ChunkSize { get; } = 150;

        private HttpClient _client { get; } = new HttpClient();

        public AchievementsController(ILogger<AchievementsController> logger, IMemoryCache memoryCache)
        {
            ServicePointManager.DefaultConnectionLimit = ConnectionLimit;
            _logger = logger;
            _cache = memoryCache;
        }

        [HttpGet]
        public async Task<IEnumerable<Models.GOG.Achievement>> Get()
        {
            var achievements = await _cache.GetOrCreateAsync("achivements_gw2_get", entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(11));
                return getGW2AchievementsData();
            });

            //transform to gog format
            var gogAchievements = achievements.Select(x => new Models.GOG.Achievement(x.Value)).ToList();
            foreach (var gogAchievement in gogAchievements)
            {
                if (gogAchievement.ImageUrlUnlocked == null)
                {
                    gogAchievement.ImageUrlUnlocked = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Constants.PlaceholderEarned}";
                }

                if (gogAchievement.ImageUrlLocked == null)
                {
                    gogAchievement.ImageUrlLocked = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Constants.PlaceholderUnearned}";
                }
            }

            return gogAchievements;
        }

        [Route("Short")]
        [HttpGet]
        public async Task<Dictionary<long, string>> GetShort()
        {
            var achievements = await _cache.GetOrCreateAsync("achivements_gw2_get", entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(11));
                return getGW2AchievementsData();
            });

            var result = new Dictionary<long, string>();
            foreach(var achievement in achievements)
            {
                result[achievement.Value.Id] = achievement.Value.Name;
            }

            return result;
        }

        private async Task<Dictionary<long, Models.GW2.Achievement>> getGW2AchievementsData()
        {
            //get list of categories and achivements
            var achievementIdsList = JsonConvert.DeserializeObject<List<long>>(await _client.GetStringAsync(Constants.ApiAchievements));
            var categoryIdsList = JsonConvert.DeserializeObject<List<long>>(await _client.GetStringAsync(Constants.ApiAchievementCategories));


            //get categories info
            var categories = await Task.WhenAll(
                categoryIdsList.Select(
                    async categoryId => JsonConvert.DeserializeObject<Models.GW2.AchievementCategory>(await _client.GetStringAsync($"{Constants.ApiAchievementCategories}/{categoryId}"))
                )
            );

            //get achievements info
            var achievements = (await Task.WhenAll(achievementIdsList.ChunkBy(ChunkSize).Select(async achievementChunk =>
            {
                var sb = new StringBuilder();
                sb.Append($"{Constants.ApiAchievements}?ids=");
                foreach (var id in achievementChunk)
                {
                    sb.Append($"{id},");
                }

                return JsonConvert.DeserializeObject<Models.GW2.Achievement[]>(await _client.GetStringAsync(sb.ToString()));
            })))
                .SelectMany(x => x)
                .Select(x => new KeyValuePair<long, Models.GW2.Achievement>(x.Id, x))
                .ToDictionary(x => x.Key, x => x.Value);


            //fill icons
            foreach (var category in categories)
            {
                foreach (var achievementId in category.Achievements)
                {
                    if (string.IsNullOrEmpty(achievements[achievementId].Icon))
                    {
                        achievements[achievementId].Icon = category.Icon;
                    }
                }
            }

            return achievements;
        }
    }
}
