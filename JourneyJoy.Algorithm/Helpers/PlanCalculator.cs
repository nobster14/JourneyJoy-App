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
    public static class PlanCalculator
    {
        /// <summary>
        /// Calculates plan of a route.
        /// </summary>
        /// <param name="genome"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public static List<(int attraction, Time enterTime, Time exitTime)>[] CalculatePlan(Genome genome, AlgorithmInformation information)
        {
            var calculatedPlan = new List<(int attraction, Time enterTime, Time exitTime)>[information.NumberOfDays];
            int weekday = information.WeekdayAtStart;

            for (int i = 0; i < information.NumberOfDays; i++)
            {
                calculatedPlan[i] = CalculateDay(genome.DayOrder[i], information, weekday);
                weekday = (weekday + 1) % 7;
            }

            return calculatedPlan;
        }

        /// <summary>
        /// Calculates plan for a day.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="information"></param>
        /// <param name="weekday"></param>
        /// <returns></returns>
        public static List<(int attraction, Time enterTime, Time exitTime)> CalculateDay(List<int> day, AlgorithmInformation information, int weekday)
        {
            int currentLocation = information.StartPoint;
            Time currentTime = AlgorithmInformation.StartTime;

            var listForTheDay = new List<(int, Time, Time)>();

            foreach (var attr in day)
            {
                var (open, close) = information.Attractions[attr].GetOpenAndCloseHourForWeekday(weekday);

                currentTime += information.DistanceBetweenAttractions(currentLocation, attr);

                var enterTime = currentTime <= open ? open : currentTime;
                var exitTime = enterTime + (int)information.Attractions[attr].TimeNeeded;

                listForTheDay.Add((attr, enterTime, exitTime));

                currentTime = exitTime;
                currentLocation = attr;
            }

            return listForTheDay;
        }
    }
}
