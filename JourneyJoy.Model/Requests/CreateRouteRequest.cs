using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Requests
{
    public record CreateRouteRequest
    {
        [Range(0, int.MaxValue)]
        public int NumberOfDays { get; set; }

        [Range(0, int.MaxValue)]
        public int StartDay { get; set; }
    }
}
