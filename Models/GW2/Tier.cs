using Newtonsoft.Json;

namespace GW2Integration.Server.Models.GW2
{
    public class Tier
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("points")]
        public long Points { get; set; }
    }
}
