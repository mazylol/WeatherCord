using DSharpPlus.SlashCommands;

namespace Interactivity.Commands;

public class Forecast : ApplicationCommandModule
{
    [SlashCommand("forecast", "weather forecast")]
    public static async Task ForecastCommand(InteractionContext ctx,
        [Option("location", "town, city, address, etc")]
        string location,
        [Option("unit", "units of measure (defaults to metric)"), Choice("metric", "metric"),
         Choice("kelvin", "standard"),
         Choice("imperial", "imperial")]
        string unit = "metric")
    {
        await ctx.CreateResponseAsync("stuff goes here");
    }
}