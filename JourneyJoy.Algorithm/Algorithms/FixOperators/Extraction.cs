using JJAlgorithm.Models;
using JourneyJoy.Algorithm.Extensions;
using JourneyJoy.Algorithm.Helpers;
using JourneyJoy.Algorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Algorithms.FixOperators
{
    public static class Extraction
    {
        public static void Execute(Genome genome, AlgorithmInformation information)
        {
            int weekday = information.WeekdayAtStart;

            for (int i = 0; i < information.NumberOfDays; i++)
            {
                (var itemsToLeave, var itemsToRemove) = ExecuteDay(information, genome.DayOrder[i], ref weekday);
                genome.DayOrder[i] = new List<int>(itemsToLeave);

                foreach (var item in itemsToRemove)
                {
                    genome.MissedAttractions.Add(item);
                }
            }
        }

        private static (List<int> itemsToLeave, List<int> itemsToRemove) ExecuteDay(AlgorithmInformation information, List<int> day, ref int weekday)
        {
            int currentLocation = information.StartPoint;
            Time currentTime = AlgorithmInformation.StartTime;

            var itemsToLeave = new List<int>();
            var itemsToRemove = new List<int>();

            foreach (var attr in day)
            {
                ProcessAttraction(information, attr, weekday, ref currentLocation, ref currentTime, itemsToLeave, itemsToRemove);
            }

            weekday = (weekday + 1) % 7;

            return(itemsToLeave, itemsToRemove);
        }

        private static void ProcessAttraction(AlgorithmInformation information, int attr, int weekday, ref int currentLocation, ref Time currentTime, List<int> itemsToLeave, List<int> itemsToRemove)
        {
            var (open, close) = information.Attractions[attr].GetOpenAndCloseHourForWeekday(weekday);

            var arrivalTime = currentTime + information.DistanceBetweenAttractions(currentLocation, attr);

            var enterTime = arrivalTime <= open ? open : arrivalTime;
            var exitTime = enterTime + (int)information.Attractions[attr].TimeNeeded;

            if (exitTime > close || exitTime + information.DistanceToHome(attr) > AlgorithmInformation.EndTime)
            {
                itemsToRemove.Add(attr);
            }
            else
            {
                itemsToLeave.Add(attr);
                currentTime = exitTime;
                currentLocation = attr;
            }
        }

    }
}
