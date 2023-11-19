using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.ExternalAPI
{
    public record ExternalAPIOptions
    {
        public string TripAdvisorAPIKey { get; set; } = null!;
        public bool IsTripAdvisorAPIEnabled { get; set; }
    }
}
