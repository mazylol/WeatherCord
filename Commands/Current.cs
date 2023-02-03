using System.Globalization;
using DSharpPlus.SlashCommands;
using WeatherCord.Call.Geocoder;

namespace WeatherCord.Commands;

public class Current : ApplicationCommandModule
{
    [SlashCommand("current", "current weather")]
    public async Task CurrentCommand(InteractionContext ctx,
        [Option("location", "town, city, address, etc")]
        string location,
        [Option("unit", "units of measure (defaults to metric)"), Choice("metric", "metric"),
         Choice("kelvin", "standard"),
         Choice("imperial", "imperial")]
        string unit = "metric")
    {
        var d = Geocoder.Get(location)![0];
        await ctx.CreateResponseAsync(d.lat.ToString(CultureInfo.InvariantCulture));
    }
}