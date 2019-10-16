using System.Collections.Generic;

using Newtonsoft.Json;

namespace GW2Integration.Server.Models.GW2
{
    public class Achievement
    {
        [JsonProperty("id")] 
        public long Id { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("name")] 
        public string Name { get; set; }

        [JsonProperty("description")] 
        public string Description { get; set; }

        [JsonProperty("requirement")] 
        public string Requirement { get; set; }

        [JsonProperty("locked_text")] 
        public string LockedText { get; set; }

        [JsonProperty("type")] 
        public string Type { get; set; }

        [JsonProperty("flags")] 
        public List<string> Flags { get; set; }

        [JsonProperty("tiers")] 
        public List<Tier> Tiers { get; set; }

        [JsonProperty("rewards")] 
        public List<Reward> Rewards { get; set; }

        [JsonProperty("bits", NullValueHandling = NullValueHandling.Ignore)]
        public List<Bit> Bits { get; set; }
    }
}
