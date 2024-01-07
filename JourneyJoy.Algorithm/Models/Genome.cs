using JJAlgorithm.Models;
using JourneyJoy.Algorithm.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public int NumberOfAttractions { get; set; }
        public int NumberOfDays { get; set; }
        public int StartPoint { get; set; }

        public Genome(int numberOfAttractions, int numberOfDays)
        {
            DayChoiceMatrix = new bool[numberOfAttractions, numberOfDays];
            DayOrder = new List<int>[numberOfDays];

            for (int i = 0; i < numberOfDays; i++)
            {
                DayOrder[i] = new List<int>();
            }

            MissedAttractions = new List<int>();
            NumberOfAttractions = numberOfAttractions;
            NumberOfDays = numberOfDays;
        }

        public Genome(AlgorithmInformation information, float boredomFactor)
        {
            NumberOfAttractions = information.NumberOfAttractions;
            NumberOfDays = information.NumberOfDays;

            MissedAttractions = new List<int>();
            DayChoiceMatrix = new bool[information.NumberOfAttractions, information.NumberOfDays];
            DayOrder = new List<int>[information.NumberOfDays];

            StartPoint = information.StartPoint;

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

        public Genome(Genome genome)
        {
            DayChoiceMatrix = genome.DayChoiceMatrix;
            DayOrder = new List<int>[genome.NumberOfDays];

            for (int i = 0; i < genome.NumberOfDays; i++)
            {
                DayOrder[i] = new List<int>(genome.DayOrder[i]);
            }

            MissedAttractions = new List<int>(genome.MissedAttractions);

            NumberOfDays = genome.NumberOfDays;
            NumberOfAttractions = genome.NumberOfAttractions;
            StartPoint = genome.StartPoint;

        }

        public void GenerateIndividualsDay(int dayNumber, AlgorithmInformation information, float boredomFactor)
        {
            Time currentTime = AlgorithmInformation.StartTime;

            Random random = new();

            int currentLocation = information.StartPoint;

            List<int> currentRoute = new();

            while (true)
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

                var result = information.Attractions[neighbour].IfPossibleToVisit(currentTime + distFromCurr, AlgorithmInformation.EndTime, weekday, distToHome);

                if (result.ifPossible)
                {
                    distances.Add((distFromCurr, neighbour, result.EndTime));
                    sum += distFromCurr;
                    currentTime = result.EndTime;
                }
            }
            if(distances.Count > 1)
                distances = distances.Select(item => (1 - item.probDistance / sum, item.index, item.endTime)).ToList();

            var sumProb = distances.Sum(item => item.probDistance);
            
            return distances.Select(item => (item.probDistance / sumProb, item.index, item.endTime)).ToList();
        }

        private static (int attraction, bool ifPossible, Time exitTime) ChooseRandomAttraction(List<(float probDistance, int index, Time endTime)> distances)
        {
            Random random = new();
            var randomNumber = random.NextDouble();

            double currSum = 0;

            for (int i = 0; i < distances.Count; i++)
            {
                if (randomNumber <= distances[i].probDistance + currSum)
                {
                    return (distances[i].index, true, distances[i].endTime);
                }
                currSum += distances[i].probDistance;
            }

            return (-1, false, new Time(23));
        }

        private void SaveDay(List<int> currentRoute, int daynumber)
        {
            DayOrder[daynumber] = currentRoute;

            foreach(var attraction in currentRoute)
                DayChoiceMatrix[attraction, daynumber] = true;
        }

        public int GetDayIndexOfAttraction(int attractionIndex)
        {
            for(int i = 0; i < NumberOfDays; i++)
            {
                if (DayChoiceMatrix[attractionIndex, i] == true)
                    return i;
            }

            return -1;
        }
    }
}
