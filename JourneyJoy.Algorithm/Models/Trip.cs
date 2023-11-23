using JourneyJoy.Algorithm.Algorithms
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Models
{
    public class Trip
    {
        public Guid ID { get; }
        public Guid UserID { get; }
        public string? Name { get; }
        public string? Description { get; set; }
        public string? Picture { get; }
        public List<Attraction> Attractions { get; }


       // public Route RoutePlanner { get; }
       

    }
}
