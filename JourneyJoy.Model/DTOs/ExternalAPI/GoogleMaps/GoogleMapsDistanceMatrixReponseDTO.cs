using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs.ExternalAPI.GoogleMaps
{
    public record GoogleMapsDistanceMatrixReponseDTO
    {
        [JsonProperty("rows")]
        public RowDTO[] Rows { get; set; }
    }

    public record RowDTO
    {
        [JsonProperty("elements")]
        public DistanceDTO[] Elements { get; set; }
    }
    public record DistanceDTO
    {
        [JsonProperty("distance")]
        public TextValueDTO Distance { get; set; }

        [JsonProperty("duration")]
        public TextValueDTO Duration { get; set; }
    }

    public record TextValueDTO
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }
}
