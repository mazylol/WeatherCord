using System.Net.Http.Headers;
using Interactivity.Call.Geocoder.Models;
using Newtonsoft.Json;

namespace Interactivity.Call.Geocoder;

public class Geocoder
{
    public static List<RootObject>? Get(string location)
    {
        using var client = new HttpClient();
        client.BaseAddress =
            new Uri(
                $"https://api.openweathermap.org/geo/1.0/direct?q={location.Replace("\\s", "")}&limit=1&appid={Program.WeatherApiKey}");

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        var response = client.GetAsync(client.BaseAddress).Result;

        if (!response.IsSuccessStatusCode) return null;
        var res = Task.FromResult(response.Content.ReadAsStringAsync().Result).Result;

        return JsonConvert.DeserializeObject<List<RootObject>>(res);
    }
}