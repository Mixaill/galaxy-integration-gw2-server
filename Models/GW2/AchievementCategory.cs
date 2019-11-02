using System;
using Newtonsoft.Json;

namespace GW2Integration.Server.Models.GW2
{
    public class AchievementCategory
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("achievements")]
        public long[] Achievements { get; set; }
    }
}
