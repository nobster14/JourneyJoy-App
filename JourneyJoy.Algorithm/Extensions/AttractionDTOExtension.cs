using JourneyJoy.Algorithm.Models;
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

        public static bool CheckIfPossibleToVisit(this AttractionDTO attraction, DateTime time, int weekday)
        {
            //todo
            return true;
        }
    }
}
