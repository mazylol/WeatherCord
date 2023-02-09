using DSharpPlus.SlashCommands;
using MongoDB.Bson;

namespace Interactivity.Commands;

public class Alerts : ApplicationCommandModule
{
    [SlashCommand("add", "add some data to the database")]
    public static async Task AddCommand(InteractionContext ctx, [Option("input", "the data to add")] string input)
    {
        if (input == "")
        {
            await ctx.CreateResponseAsync("Empty input");
        }
        else
        {
            var db = Program.DbClient?.GetDatabase("add");
            var coll = db?.GetCollection<DbAdd>("stuff");

            var insert = new DbAdd
            {
                Author = ctx.Member.DisplayName,
                Input = input
            };

            coll?.InsertOneAsync(insert);

            await ctx.CreateResponseAsync("Added to database!");
        }
    }
}

internal class DbAdd
{
    public ObjectId Id { get; set; }
    public string? Author { get; set; }
    public string? Input { get; set; }
}