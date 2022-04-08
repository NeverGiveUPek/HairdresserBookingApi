using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace HairdresserBookingApi.IntegrationTests.Helpers;

public static class HttpContentHelper
{
    public static HttpContent ToJsonHttpContent(this object model)
    {
        var json = JsonConvert.SerializeObject(model);

        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        return httpContent;
    }
}