using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GW2Integration.Server.Models.GOG
{
    public class Achievement
    {
        [JsonProperty("release_per_platform_id")]
        public string ReleasePerPlatformId { get; set; } = $"{Constants.PlatformId}_{Constants.GameId}";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("image_url_unlocked")]
        public string ImageUrlUnlocked { get; set; }

        [JsonProperty("image_url_locked")]
        public string ImageUrlLocked { get; set; }

        public Achievement()
        {

        }

        public Achievement(Models.GW2.Achievement gw2Achievement)
        {
            Name = gw2Achievement.Name;
            Description = gw2Achievement.Description;
            ApiKey = gw2Achievement.Id.ToString();
            ImageUrlUnlocked = gw2Achievement.Icon;
        } 
    }
}
