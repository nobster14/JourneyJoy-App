using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs.ExternalAPI.TripAdvisor
{
    public record TripAdvisorPhotosDTO
    {
        [JsonProperty("thumbnail")]
        public TripAdvisorPhotoDTO Thumbnail { get; set; }
        [JsonProperty("small")]
        public TripAdvisorPhotoDTO Small { get; set; }
        [JsonProperty("medium")]
        public TripAdvisorPhotoDTO Medium { get; set; }
        [JsonProperty("large")]
        public TripAdvisorPhotoDTO Large { get; set; }
        [JsonProperty("original")]
        public TripAdvisorPhotoDTO Original { get; set; }
    }
    public record TripAdvisorPhotoDTO
    {
        /// <summary>
        /// Photo width
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Photo height
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Link to Photo
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

    }
}
