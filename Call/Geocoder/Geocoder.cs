using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace WeatherCord.Call.Geocoder;

public class Geocoder
{
    public static string?[] Get(string location)
    {
        using var client = new HttpClient();
        client.BaseAddress =
            new Uri(
                $"https://api.openweathermap.org/geo/1.0/direct?q={location.Replace("\\s", "")}&limit=1&appid={Program.WeatherApiKey}");
        
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        
        var response = client.GetAsync(client.BaseAddress).Result;

        string?[] output = new string[] { };

        if (response.IsSuccessStatusCode)
        {
            var res = Task.FromResult(response.Content.ReadAsStringAsync().Result).Result;
            var getResult = JObject.Parse(res);
            
            var arr = new string?[2];
            arr[0] = getResult[0]?["lat"]?.ToString();
            arr[1] = getResult[0]?["lon"]?.ToString();

            output = arr;
        }

        return output;
    }
}

// remember, use return value with .Result