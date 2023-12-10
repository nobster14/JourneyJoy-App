using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs.ExternalAPI.TripAdvisor
{
    public record TripAdvisorDetailsResponseDTO
    {
        [JsonProperty("location_id")]
        public int LocationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("web_url")]
        public string WebUrl { get; set; }

        [JsonProperty("address_obj")]
        public TripAdvisorAddressDTO Address { get; set; }
    }
}
