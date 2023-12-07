using JourneyJoy.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Extensions
{
    public static class AttractionDTOExtension
    {
        public static (float Lat, float Lon) GetLanLon(this AttractionDTO attraction)
        {
            return ((float)attraction.Location.Latitude, (float)attraction.Location.Longitude);
        }
    }
}
