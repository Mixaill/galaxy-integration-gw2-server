using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GW2Integration.Server.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace GW2Integration.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AchievementsController : ControllerBase
    {
        private ILogger<AchievementsController> _logger { get; }

        private HttpClient _client { get; }= new HttpClient();

        public AchievementsController(ILogger<AchievementsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Models.GOG.Achievement>> Get()
        {
            var gw2Achievements = new List<Models.GW2.Achievement>();
            var idsList = new List<int>();

            try
            {
                var responseList = await _client.GetStringAsync(Constants.ApiAchievements);
                idsList = JsonConvert.DeserializeObject<List<int>>(responseList);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return new List<Models.GOG.Achievement>();
            }


            var chunkSize = 150;
            foreach (var chunk in idsList.ChunkBy(chunkSize))
            {
                if (chunk.Count == 0)
                {
                    continue;
                }

                var sb = new StringBuilder();
                sb.Append($"{Constants.ApiAchievements}?ids=");
                foreach(var id in chunk)
                {
                    sb.Append($"{id},");
                }

                try
                {
                    var responseData = await _client.GetStringAsync(sb.ToString());
                    var parsedResponse = JsonConvert.DeserializeObject<List<Models.GW2.Achievement>>(responseData);
                    gw2Achievements.AddRange(parsedResponse);
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                }
            }

            return gw2Achievements.Select(x => new Models.GOG.Achievement(x));
        }
    }
}
