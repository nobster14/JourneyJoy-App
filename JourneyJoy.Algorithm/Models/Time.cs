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

        public void AddMinutes(int minutes)
        {
            var newTime = TimeInMinutes + minutes;
            Hour = (int)(newTime / 60);
            Minute = newTime - Hour * 60;
        }
    }
}
