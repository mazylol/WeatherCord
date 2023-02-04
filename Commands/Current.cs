using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using WeatherCord.Call.Weather;

namespace WeatherCord.Commands;

public class Current : ApplicationCommandModule
{
    [SlashCommand("current", "current weather")]
    public static async Task CurrentCommand(InteractionContext ctx,
        [Option("location", "town, city, address, etc")]
        string location,
        [Option("unit", "units of measure (defaults to metric)"), Choice("metric", "metric"),
         Choice("kelvin", "standard"),
         Choice("imperial", "imperial")]
        string unit = "metric")
    {
        var w = CurrentCall.Get(location, unit)!;

        var abbr = unit switch
        {
            "imperial" => "F",
            "standard" => "K",
            _ => "C"
        };

        var embed = new DiscordEmbedBuilder
        {
            Title = $"Current weather for {w.name}",
            Description = w.weather[0].description,
            Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
            {
                Url = $"https://openweathermap.org/img/w/{w.weather[0].icon}.png"
            },
            Color = new DiscordColor("#00AE86"),
            Timestamp = DateTimeOffset.Now
        };

        embed.AddField("Temperature", $"{w.main.temp} {abbr}", true);
        embed.AddField("High", $"{w.main.temp_max} {abbr}", true);
        embed.AddField("Low", $"{w.main.temp_min} {abbr}", true);
        embed.AddField("Humidity", $"{w.main.humidity}%", true);
        embed.AddField("Pressure", $"{w.main.pressure} hPa", true);
        embed.AddField("Wind Speed", $"{w.wind.speed} m/s", true);

        await ctx.CreateResponseAsync(embed: embed.Build(), true);
    }
}