using JJAlgorithm.Models;
using JourneyJoy.Algorithm.Models;
using JourneyJoy.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        /// <summary>
        /// Zwraca informację czy atrakcję można odwiedzić o danej godzinie i wrócić do domu
        /// </summary>
        /// <param name="attraction"></param>
        /// <returns></returns>
        public static (bool ifPossible, Time EndTime) IfPossibleToVisit(this AttractionDTO attraction, Time arrivalTime, Time endOfDay, int weekday, int distanceToHome)
        {
            (Time open, Time close) = attraction.GetOpenAndCloseHourForWeekday(weekday);

            if (open == new Time() && close == new Time(24))
            {
                var endTime = arrivalTime + (int)attraction.TimeNeeded;

                if (endTime + distanceToHome <= endOfDay)
                    return (true, endTime);
                else
                    return (false, endTime);
            }

            var enterTime = arrivalTime <= open ? open : arrivalTime;
            var exitTime = enterTime + (int)attraction.TimeNeeded;

            if (exitTime <= close && exitTime + distanceToHome <= endOfDay)
                return (true, exitTime);

            return (false, exitTime);

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

        /// <summary>
        /// Zwraca godziny otwarcia w podany dzień tygodnia
        /// </summary>
        /// <param name="attraction"></param>
        /// <returns></returns>
        public static (Time open, Time close) GetOpenAndCloseHourForWeekday(this AttractionDTO attraction, int weekday)
        {
            if (attraction.LocationType == Model.Enums.LocationType.WithoutHours || attraction.OpenHours == null || attraction.OpenHours.Count() != 7 || attraction.OpenHours.Any(it => it.Count() != 2))
                return (new Time(), new Time(24));

            (int hour, int minute) StringToHourAndMinute(string hhmmString)
            {
                return (Int32.Parse(hhmmString.Substring(0, 2)), Int32.Parse(hhmmString.Substring(2, 2)));
            }

            var open = StringToHourAndMinute(attraction.OpenHours[weekday][0]);
            var close = StringToHourAndMinute(attraction.OpenHours[weekday][1]);

            return (new Time(open.hour, open.minute), new Time(close.hour, close.minute));

        }
    }
}
