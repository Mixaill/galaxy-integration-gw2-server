using System.Runtime.InteropServices.ComTypes;

namespace GW2Integration.Server
{
    public class Constants
    {
        public static string PlatformId { get; } = "gw2";
        public static string GameId { get; }= "guild_wars_2";

        public static string ApiPrefix { get; } = "https://api.guildwars2.com/v2";

        public static string ApiAchievements { get; } = ApiPrefix + "/achievements";

        public static string PlaceholderEarned { get; } = "/assets/achievement-earned.png";

        public static string PlaceholderUnearned { get; } = "/assets/achievement-unearned.png";


    }
}
