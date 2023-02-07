namespace Interactivity.Call.Geocoder.Models;

public class RootObject
{
    public string name { get; set; }
    public Local_names local_names { get; set; }
    public double lat { get; set; }
    public double lon { get; set; }
    public string country { get; set; }
    public string state { get; set; }
}