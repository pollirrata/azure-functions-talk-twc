#r "Newtonsoft.Json"

using System;
using System.Text;
using System.Net;
using Newtonsoft.Json;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"SlackWebhookCSharp1 was triggered!");

    string formDataStr = await req.Content.ReadAsStringAsync();

    return req.CreateResponse(HttpStatusCode.OK, new
    {
        text = $"Thanks! * ~Annoying~ Help request #{new Random().Next()}*\n I got \n>" + formDataStr.Replace("&", "\n>")
    });
}