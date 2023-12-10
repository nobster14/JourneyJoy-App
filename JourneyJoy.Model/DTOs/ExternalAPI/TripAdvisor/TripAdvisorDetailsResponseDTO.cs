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

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("hours")]
        public HoursDTO Hours { get; set; }

    }

    public record HoursDTO
    {
        [JsonProperty("periods")]
        public PeriodDTO[] Periods { get; set; }
    }

    public record PeriodDTO
    {
        [JsonProperty("open")]
        public HourDTO Open { get; set; }

        [JsonProperty("close")]
        public HourDTO Close { get; set; }
    }
    public record HourDTO
    {
        [JsonProperty("day")]
        public int Day { get; set; }

        /// <summary>
        /// In format "{hours}{minutes}"
        /// </summary>
        [JsonProperty("time")]
        public string Time { get; set; }
    }
}
