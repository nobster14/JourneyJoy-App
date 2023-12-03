using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Requests
{
    public record TakeSkipRequest
    {
        [Range(0, int.MaxValue)]
        public int Take { get; set; }

        [Range(0, int.MaxValue)]
        public int Skip { get; set; }
    }
}
