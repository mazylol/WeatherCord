using DotNetEnv;
using DSharpPlus;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace AlertService
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

            WeatherApiKey = Env.GetString("WEATHER_API_KEY");

            var uri = Env.GetString("MONGO_URI");
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("elementNameConvention", pack, _ => true);

            DbClient = new MongoClient(uri);

            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = discordToken,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged,
                MinimumLogLevel = LogLevel.Debug
            });

            await discord.ConnectAsync();

            var db = DbClient.GetDatabase("add");
            var coll = db.GetCollection<DataModel>("stuff");

            var cursor = coll.AsQueryable();

            foreach (var document in cursor)
            {
                Console.WriteLine(document.Guild);
            }

            Environment.Exit(0);
        }
    }
}

internal class DataModel
{
    public ObjectId Id { get; set; }
    public ulong Guild { get; set; }
    public ulong Channel { get; set; }
}