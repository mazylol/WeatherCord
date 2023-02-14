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
            DiscordChannel channel)
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

            var insert = new DataModel
            {
                Guild = ctx.Guild.Id,
                Channel = channel.Id
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
}