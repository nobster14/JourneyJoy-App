using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Models
{
    public class Attraction
    {
        public Guid ID { get; }
        public Guid UserID { get; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Picture { get; set; }
        public int Type { get; }
        public Localisation Address { get; set; }
        public int[][]? OpenHours { get; set; } 
        public float[]? Prices { get; set; }
        public float? TimeNeeded { get; set; }
        public (float Lat, float Lon) GetLatLon()
        {
            return (Address.X, Address.Y);
        }
    }
}
