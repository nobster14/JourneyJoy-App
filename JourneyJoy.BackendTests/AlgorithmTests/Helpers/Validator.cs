using JJAlgorithm.Models;
using JourneyJoy.Algorithm.Extensions;
using JourneyJoy.Algorithm.Models;
using JourneyJoy.Model.Database.Tables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.UnitTests.AlgorithmTests.Helpers
{
    public static class Validator
    {
        public static bool Validate(Genome genome, AlgorithmInformation information) 
        {
            // Sprawdzenie czy atrakcja jest maksymalnie na jeden dzień
            for(int i = 0; i < information.NumberOfAttractions; i++)
            {
                int sum = 0;
                for(int j = 0; j < information.NumberOfDays; j++)
                {
                    if (genome.DayChoiceMatrix[i, j])
                        sum++;
                }
                if (sum > 1) return false;
            }

            int weekday = information.WeekdayAtStart;
            // Sprawdzenie poprawności każdego dnia
            foreach(var day in genome.DayOrder)
            {
                if (!ValidateDay(day, information, weekday))
                    return false;
                weekday = (weekday + 1) % 7;
            }
            return true;
        }

        public static bool ValidateDay(List<int> day, AlgorithmInformation information, int weekday)
        {
            int currentLocation = information.StartPoint;
            Time currentTime = AlgorithmInformation.StartTime;

            foreach(var attr in day)
            {
                var (open, close) = information.Attractions[attr].GetOpenAndCloseHourForWeekday(weekday);

                currentTime += information.DistanceBetweenAttractions(currentLocation, attr);

                var enterTime = currentTime <=  open ? open : currentTime;
                var exitTime = enterTime + (int)information.Attractions[attr].TimeNeeded;

                if (exitTime > close)
                    return false;

                currentTime = exitTime;
            }

            if (currentTime > AlgorithmInformation.EndTime)
                return false;

            return true;
        }
    }
}
