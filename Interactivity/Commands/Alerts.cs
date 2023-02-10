using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MongoDB.Bson;

namespace Interactivity.Commands;

public class Alerts : ApplicationCommandModule
{
    [SlashCommandGroup("alerts", "national weather alerts")]
    public class AlertGroup
    {
        // still need to check for correct channel type and if the server is already subscribed
        [SlashCommand("subscribe", "subscribe to alerts")]
        public static async Task SubscribeCommand(InteractionContext ctx,
            [Option("channel", "the channel to send alerts to")]
            DiscordChannel channel)
        {
            var db = Program.DbClient?.GetDatabase("add");
            var coll = db?.GetCollection<DataModel>("stuff");

            var insert = new DataModel
            {
                Guild = ctx.Guild.Id,
                Channel = channel.Id
            };

            coll?.InsertOneAsync(insert);

            await ctx.CreateResponseAsync($"Subscribed to alerts in {channel.Mention}");
        }
    }
}

internal class DataModel
{
    public ObjectId Id { get; set; }
    public ulong Guild { get; set; }
    public ulong Channel { get; set; }
}