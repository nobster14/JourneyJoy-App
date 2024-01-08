using JJAlgorithm.Models;
using JourneyJoy.Algorithm.Extensions;
using JourneyJoy.Algorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Helpers
{
    public static class Validator
    {
        /// <summary>
        /// Validates genome.
        /// </summary>
        /// <param name="genome"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public static bool Validate(Genome genome, AlgorithmInformation information)
        {
            for (int i = 0; i < information.NumberOfAttractions; i++)
            {
                int sum = 0;
                for (int j = 0; j < information.NumberOfDays; j++)
                {
                    if (genome.DayChoiceMatrix[i, j])
                        sum++;
                }
                if (sum > 1) return false;
            }

            int weekday = information.WeekdayAtStart;
            foreach (var day in genome.DayOrder)
            {
                if (!ValidateDay(day, information, weekday))
                    return false;
                weekday = (weekday + 1) % 7;
            }
            return true;
        }

        /// <summary>
        /// Validates genome.
        /// </summary>
        /// <param name="genome"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public static bool Validate(List<int>[] genome, AlgorithmInformation information)
        {
            int weekday = information.WeekdayAtStart;
            foreach (var day in genome)
            {
                if (!ValidateDay(day, information, weekday))
                    return false;
                weekday = (weekday + 1) % 7;
            }
            return true;
        }

        /// <summary>
        /// Validates day.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="information"></param>
        /// <param name="weekday"></param>
        /// <returns></returns>
        public static bool ValidateDay(List<int> day, AlgorithmInformation information, int weekday)
        {
            int currentLocation = information.StartPoint;
            Time currentTime = AlgorithmInformation.StartTime;

            foreach (var attr in day)
            {
                var (open, close) = information.Attractions[attr].GetOpenAndCloseHourForWeekday(weekday);

                currentTime += information.DistanceBetweenAttractions(currentLocation, attr);

                var enterTime = currentTime <= open ? open : currentTime;
                var exitTime = enterTime + (int)information.Attractions[attr].TimeNeeded;

                if (exitTime > close)
                    return false;

                currentTime = exitTime;
                currentLocation = attr;
            }

            if (currentTime > AlgorithmInformation.EndTime)
                return false;

            return true;
        }

    }
}
