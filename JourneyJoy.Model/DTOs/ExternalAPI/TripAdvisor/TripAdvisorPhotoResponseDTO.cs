using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs.ExternalAPI.TripAdvisor
{
    public record TripAdvisorPhotoResponseDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("is_blessed")]
        public bool IsBlessed { get; set; }

        [JsonProperty("album")]
        public string Album { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("images")]
        public TripAdvisorPhotosDTO Images { get; set; }

        [JsonProperty("published_date")]
        public string PublishedDate { get; set; }

        [JsonProperty("source")]
        public TripAdvisorSourceDTO Source { get; set; }

        [JsonProperty("user")]
        public TripAdvisorUserDTO User { get; set; }
    }
}
