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

        /// <summary>
        /// Zwraca kolekcję godzin otwarcia - W przypadku braku godzin otwarcia zwracamy pustą kolekcję
        /// </summary>
        /// <param name="attraction"></param>
        /// <returns></returns>
        public static IEnumerable<((int hour, int minute) open, (int hour, int minute) close)> GetOpenAndCloseHoursForAttraction(this AttractionDTO attraction)
        {
            // błędny format/brak danych - uznajemy, że godzin otwarcia nie ma
            if (attraction.LocationType == Model.Enums.LocationType.WithoutHours || attraction.OpenHours == null || attraction.OpenHours.Count() != 7 || attraction.OpenHours.Any(it => it.Count() != 2))
                return Enumerable.Empty<((int hour, int minute) open, (int hour, int minute) close)>();

            (int hour, int minute) StringToHourAndMinute(string hhmmString)
            {
                return (Int32.Parse(hhmmString.Substring(0, 2)), Int32.Parse(hhmmString.Substring(2, 2)));
            }

            return attraction.OpenHours.Select(it => (StringToHourAndMinute(it[0]), StringToHourAndMinute(it[1])));
        }
    }
}
