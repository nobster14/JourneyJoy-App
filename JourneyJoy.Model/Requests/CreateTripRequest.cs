using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Requests
{
    public record CreateTripRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
    }
}
