using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MongoDB.Bson;
using MongoDB.Driver;
using static MongoDB.Driver.Builders<Interactivity.Commands.DataModel>;

namespace Interactivity.Commands;

public class Alerts : ApplicationCommandModule
{
    [SlashCommandGroup("alerts", "national weather alerts")]
    public class AlertGroup
    {
        private static readonly IMongoDatabase? Db = Program.DbClient?.GetDatabase("add");
        private static readonly IMongoCollection<DataModel>? Coll = Db?.GetCollection<DataModel>("stuff");

        [SlashCommand("subscribe", "subscribe to alerts")]
        public static async Task SubscribeCommand(InteractionContext ctx,
            [Option("channel", "the channel to send alerts to")]
            DiscordChannel channel, [Option("location", "town, city, etc")] string location,
            [Option("unit", "units of measure (defaults to metric)"), Choice("metric", "metric"),
             Choice("kelvin", "standard"), Choice("imperial", "imperial")]
            string unit = "metric")
        {
            var filter = Filter.Eq("guild", ctx.Guild.Id);

            if (await Coll.Find(filter).AnyAsync())
            {
                await ctx.CreateResponseAsync("This server is already subscribed to alerts");
                return;
            }

            if (channel.Type != ChannelType.Text)
            {
                await ctx.CreateResponseAsync("Channel must be a text channel");
                return;
            }

            var d = Call.Geocoder.Geocoder.Get(location)![0];

            var insert = new DataModel
            {
                Guild = ctx.Guild.Id,
                Channel = channel.Id,
                Location = new[] { d.Lat, d.Lon },
                Unit = unit
            };

            Coll?.InsertOneAsync(insert);

            await ctx.CreateResponseAsync($"Subscribed to alerts in {channel.Mention}");
        }

        [SlashCommand("unsubscribe", "unsubscribe from alerts")]
        public static async Task UnsubscribeCommand(InteractionContext ctx)
        {
            var filter = Filter.Eq("guild", ctx.Guild.Id);

            if (!await Coll.Find(filter).AnyAsync())
            {
                await ctx.CreateResponseAsync("This server is not subscribed to alerts");
                return;
            }

            Coll?.DeleteOneAsync(filter);

            await ctx.CreateResponseAsync("Unsubscribed from alerts");
        }
    }
}

internal class DataModel
{
    public ObjectId Id { get; set; }
    public ulong Guild { get; set; }
    public ulong Channel { get; set; }
    public double?[] Location { get; set; }
    public string Unit { get; set; }
}