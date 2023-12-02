using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs.ExternalAPI
{
    public record BasicJsonArray<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
