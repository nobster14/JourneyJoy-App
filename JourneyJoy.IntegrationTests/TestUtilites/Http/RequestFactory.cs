using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.IntegrationTests.TestUtilites.Http
{
    public static class RequestFactory
    {
        public static HttpRequestMessage RequestMessageWithBody<T>(string relativeApiPath, HttpMethod method, T bodyContent)
        {
            return new HttpRequestMessage
            {
                RequestUri = new Uri(relativeApiPath, UriKind.Relative),
                Method = method,
                Content = new StringContent(
                    JsonConvert.SerializeObject(bodyContent),
                    Encoding.UTF8,
                    "application/json")
            };
        }
    }
}
