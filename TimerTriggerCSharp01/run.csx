using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri("https://hooks.slack.com/services/");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    string path = ConfigurationManager.AppSettings["slkincoming"];
    string message = $"{{\"text\": \"Timesheet time { DateTime.Now.ToLongTimeString() }!!\"}}";
    HttpResponseMessage response = client.PostAsync(path, new StringContent(message, Encoding.UTF8, "application/json")).Result;
    if (!response.IsSuccessStatusCode)
    {
        log.Error(response.StatusCode + "\n" + response.ReasonPhrase + "\n" + response.RequestMessage);
    }
}