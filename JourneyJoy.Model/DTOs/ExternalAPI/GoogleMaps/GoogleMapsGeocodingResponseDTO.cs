using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs.ExternalAPI.GoogleMaps
{
    public record GoogleMapsGeocodingResponseDTO
    {
        [JsonProperty("results")]
        public ResultDTO[] Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
    public record ResultDTO
    {
        [JsonProperty("geometry")]
        public GeometryDTO Geometry { get; set; }
    }
    public record GeometryDTO
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("location_type")]
        public string LocationType { get; set; }

    }


    public record Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

}
