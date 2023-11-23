using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Database.Tables
{
    public record Location
    {
        [MaxLength(200)]
        public string City { get; set; } = null!;

        [MaxLength(200)]
        public string StreetName { get; set; } = null!;
        public double X { get; set; }
        public double Y { get; set; }
    }
}
