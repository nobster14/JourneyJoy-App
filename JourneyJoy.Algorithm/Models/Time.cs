using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJAlgorithm.Models
{
    public class Time
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int TimeInMinutes => 60 * Hour + Minute;

        public Time() 
        {
            Hour = 0;
            Minute = 0;
        }
        public Time(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
        }

        public Time(int hour)
        {
            Hour = hour;
            Minute = 0;
        }

        /// <summary>
        /// Adds time specified in minutes.
        /// </summary>
        /// <param name="minutes"></param>
        public void AddMinutes(int minutes)
        {
            var totalMinutes = TimeInMinutes + minutes;
            Hour = (int)(totalMinutes / 60);
            Minute = totalMinutes % 60;
        }

        public static Time operator +(Time time, int minutes)
        {
            int totalMinutes = time.TimeInMinutes + minutes;
            int newHour = totalMinutes / 60;
            int newMinute = totalMinutes % 60;

            return new Time(newHour, newMinute);
        }
        public static bool operator ==(Time time1, Time time2)
        {
            return time1.Hour == time2.Hour && time1.Minute == time2.Minute;
        }

        public static bool operator !=(Time time1, Time time2)
        {
            return !(time1 == time2);
        }

        public static bool operator <=(Time time1, Time time2)
        {
            return time1.TimeInMinutes <= time2.TimeInMinutes;
        }
        public static bool operator >=(Time time1, Time time2)
        {
            return time1.TimeInMinutes >= time2.TimeInMinutes;
        }
        public static bool operator <(Time time1, Time time2)
        {
            return time1.TimeInMinutes < time2.TimeInMinutes;
        }
        public static bool operator >(Time time1, Time time2)
        {
            return time1.TimeInMinutes > time2.TimeInMinutes;
        }
    }
}
