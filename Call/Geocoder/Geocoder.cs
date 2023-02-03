using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherCord.Call.Geocoder.Models;

namespace WeatherCord.Call.Geocoder;

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
        
        if (response.IsSuccessStatusCode)
        {
            var res = Task.FromResult(response.Content.ReadAsStringAsync().Result).Result;

            List<RootObject>? data = JsonConvert.DeserializeObject<List<RootObject>>(res);

            return data;
        }

        return null;
    }
}

// remember, use return value with .Result