using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs.ExternalAPI.TripAdvisor
{
    public record TripAdvisorUserDTO
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("reviewer_badge")]
        public string ReviewerBadge { get; set; }

        [JsonProperty("review_count")]
        public int ReviewCount { get; set; }
    }
}
