using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs.ExternalAPI.TripAdvisor
{
    public record TripAdvisorAttractionDTO
    {
        [JsonProperty("location_id")]
        public int LocationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("distance")]
        public string Distance { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }

        [JsonProperty("bearing")]
        public string Bearing { get; set; }

        [JsonProperty("address_obj")]
        public TripAdvisorAddressDTO Address { get; set; }
    }
}
