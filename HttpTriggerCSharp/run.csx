using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log, IAsyncCollector<string> outputQueueItem)
{
    log.Info("C# HTTP trigger function processed a request.");

    string phone = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "phone", true) == 0)
        .Value;

    if (!string.IsNullOrEmpty(phone))
    {
        await outputQueueItem.AddAsync(phone);
    }

    return phone == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a phone on the query string or in the request body")
        : req.CreateResponse(HttpStatusCode.OK);
}