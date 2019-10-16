using Newtonsoft.Json;

namespace GW2Integration.Server.Models.GW2
{
    public class Bit
    {
        [JsonProperty("type")] 
        public string Type { get; set; }

        [JsonProperty("id")] 
        public long Id { get; set; }
    }
}
