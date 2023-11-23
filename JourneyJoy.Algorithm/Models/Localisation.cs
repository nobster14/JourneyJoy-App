using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Models
{
    public class Localisation
    {
        public float X { get; set; }
        public float Y { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostCode { get; set; }
        public string? Country { get; set; }
    }
}
