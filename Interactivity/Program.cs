using DotNetEnv;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Interactivity.Commands;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Interactivity
{
    internal abstract class Program
    {
        public static string WeatherApiKey = null!;

        public static MongoClient? DbClient;

        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            Env.TraversePath().Load();
            var discordToken = Env.GetString("DEV_TOKEN");
            var guildId = Convert.ToUInt64(Env.GetString("GUILD_ID"));

            WeatherApiKey = Env.GetString("WEATHER_API_KEY");

            var uri = Env.GetString("MONGO_URI");
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("elementNameConvention", pack, _ => true);

            DbClient = new MongoClient(uri);

            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = discordToken,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged,
                MinimumLogLevel = LogLevel.Debug
            });

            discord.Ready += DiscordReady;

            var slash = discord.UseSlashCommands();

            slash.RegisterCommands<Current>(guildId);
            slash.RegisterCommands<Alerts>(guildId);

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task DiscordReady(DiscordClient client, ReadyEventArgs e)
        {
            await client.UpdateStatusAsync(new DiscordActivity("no storms?"), UserStatus.Online);
        }
    }
}