using JJAlgorithm.Helpers;
using JJAlgorithm.Models;
using JourneyJoy.Algorithm.Extensions;
using Microsoft.Extensions.Azure;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Models
{
    public class Genome
    {
        public bool[,] DayChoiceMatrix { get; set; }
        public List<int>[] DayOrder { get; set; }
        public List<int> MissedAttractions { get; set; }

        public Genome(int numberOfAttractions, int numberOfDays)
        {
            DayChoiceMatrix = new bool[numberOfAttractions, numberOfDays];
            DayOrder = new List<int>[numberOfDays];

            for (int i = 0; i < numberOfDays; i++)
            {
                DayOrder[i] = new List<int>();
            }
            MissedAttractions = new List<int>();
        }

        public Genome(AlgorithmInformation information, float boredomFactor)
        {
            MissedAttractions = new List<int>();
            DayChoiceMatrix = new bool[information.NumberOfAttractions, information.NumberOfDays];
            DayOrder = new List<int>[information.NumberOfDays];

            for (int i = 0; i < information.NumberOfDays; i++)
            {
                DayOrder[i] = new List<int>();
            }

            for (int i = 0; i < information.NumberOfAttractions; i++)
            {
                if (i != information.StartPoint)
                    MissedAttractions.Add(i);
            }

            for (int j = 0; j < information.NumberOfDays; j++)
                GenerateIndividualsDay(j, information, boredomFactor);
        }

        public void GenerateIndividualsDay(int dayNumber, AlgorithmInformation information, float boredomFactor)
        {
            Time currentTime = information.StartTime;

            Random random = new();
            bool nextMovePossible = true;

            int currentLocation = information.StartPoint;

            List<int> currentRoute = new();

            while (nextMovePossible)
            {
                if (random.NextDouble() < boredomFactor)
                {
                    SaveDay(currentRoute, dayNumber);
                    return;
                }

               var (attraction, ifPossible, exitTime) = GetRandomAttraction(information, currentLocation, currentTime, (dayNumber + information.WeekdayAtStart) % 7);

                if (!ifPossible)
                {
                    SaveDay(currentRoute, dayNumber);
                    return;
                }

                currentTime = exitTime;
                currentRoute.Add(attraction);
                MissedAttractions.Remove(attraction);
                currentLocation = attraction;
            }
        }

        private (int attraction, bool ifPossible, Time exitTime) GetRandomAttraction(AlgorithmInformation information, int currentLocation, Time currentTime, int weekday)
        {
            MissedAttractions = MissedAttractions.OrderBy(neighbour => information.AdjustmentMatrix[currentLocation][neighbour]).ToList();

            List<(float probDistance, int index, Time endTime)> distances = CalculateNormalizedDistances(information, currentLocation, currentTime, weekday);

            return ChooseRandomAttraction(distances);
        }

        private List<(float probDistance, int index, Time endTime)> CalculateNormalizedDistances(AlgorithmInformation information, int currentLocation, Time currentTime, int weekday)
        {
            List<(float probDistance, int index, Time endTime)> distances = new();

            int sum = 0;

            foreach (var neighbour in MissedAttractions)
            {
                var distFromCurr = information.DistanceBetweenAttractions(currentLocation, neighbour);
                var distToHome = information.DistanceToHome(neighbour);

                var result = information.Attractions[neighbour].IfPossibleToVisit(currentTime + distFromCurr, information.EndTime, weekday, distToHome);

                if (result.ifPossible)
                {
                    distances.Add((distFromCurr + sum, neighbour, result.EndTime));
                    sum += distFromCurr;
                }
            }

            return distances.Select(item => (1 - item.probDistance / sum, item.index, item.endTime)).ToList();
        }

        private static (int attraction, bool ifPossible, Time exitTime) ChooseRandomAttraction(List<(float probDistance, int index, Time endTime)> distances)
        {
            Random random = new();
            var randomNumber = random.NextDouble();

            for (int i = 0; i < distances.Count; i++)
            {
                if (randomNumber <= distances[i].probDistance)
                {
                    return (i, true, distances[i].endTime);
                }
            }

            return (-1, false, new Time(23));
        }

        private void SaveDay(List<int> currentRoute, int daynumber)
        {
            DayOrder[daynumber] = currentRoute;

            foreach(var attraction in currentRoute)
                DayChoiceMatrix[attraction, daynumber] = true;
        }
    }
}
