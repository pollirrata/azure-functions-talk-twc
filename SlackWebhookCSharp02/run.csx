#r "Newtonsoft.Json"

using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"WeatherWebhookCSharp1 was triggered from Slack!");

    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");

    HttpResponseMessage response = await client.GetAsync($"weather?q=Guadalajara&units=metric&appid={ConfigurationManager.AppSettings["owappid"]}");
    string tempString = string.Empty;
    string responseMessage = $"";

    if (response.IsSuccessStatusCode)
    {
        tempString = await response.Content.ReadAsStringAsync();
        WeatherResult wr = JsonConvert.DeserializeObject<WeatherResult>(tempString);
        string option = "beer";
        if (wr.Main.Temp < 18)
        {
            option = "coat";
        }

        responseMessage = $"Hello, the current weater conditions are {wr.Weather.First().Main},  humidity {wr.Main.Humidity}%, temperature {wr.Main.Temp}*C (Min {(wr.Main.Temp_Min ?? 0)} / Max {(wr.Main.Temp_Max ?? 0)}). We recommend you to get a {option}.";
    }
    else
    {
        responseMessage = "error";
    }

    return req.CreateResponse(HttpStatusCode.OK, new
    {
        text = responseMessage
    });
}

public class WeatherResult
{
    public List<WeatherValue> Weather { get; set; }
    public Main Main { get; set; }
}
public class WeatherValue
{
    public int ID { get; set; }
    public string Main { get; set; }
}
public class Main
{
    public double Temp { get; set; }
    public double? Humidity { get; set; }
    public double? Temp_Min { get; set; }
    public double? Temp_Max { get; set; }
}