using System.Net.Http.Headers;
using Newtonsoft.Json;
using WeatherCord.Call.Weather.Models;

namespace WeatherCord.Call.Weather;

public class CurrentCall
{
    public static RootObject? Get(string location, string unit)
    {
        var d = Geocoder.Geocoder.Get(location)![0];
        
        using var client = new HttpClient();
        client.BaseAddress = new Uri(
            $"https://api.openweathermap.org/data/2.5/weather?lat={d.lat}&lon={d.lon}&units={unit}&appid={Program.WeatherApiKey}");
        
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        
        var response = client.GetAsync(client.BaseAddress).Result;

        if (!response.IsSuccessStatusCode) return null;
        var res = Task.FromResult(response.Content.ReadAsStringAsync().Result).Result;

        return JsonConvert.DeserializeObject<RootObject>(res);
    }
}