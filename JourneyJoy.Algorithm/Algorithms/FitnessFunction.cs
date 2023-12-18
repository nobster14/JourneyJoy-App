using JourneyJoy.Algorithm.Models;
using JourneyJoy.Model.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Algorithms
{
    public static class FitnessFunction
    {
        public static double WeightVisitedAttractions => 0.4;
        public static double WeightTotalDistance => 0.3;
        public static double WeightTotalCost => 0.3;
        public static double PenaltyFactor => 0.1;

        public static int MaxPossibleDistance { get; private set; }
        public static int MaxNumberOfAttractions { get; private set; }
        public static double MaxCost { get; private set; }

        public static void CalculateMaxDistance(AlgorithmInformation information)
        {
            MaxPossibleDistance = 0;
            for(int i = 0; i < information.NumberOfAttractions; i++)
            {
                for(int j = i; j < information.NumberOfAttractions; j++)
                {
                    MaxPossibleDistance += Math.Max(information.DistanceBetweenAttractions(i, j), information.DistanceBetweenAttractions(j, i));
                }
            }
        }

        public static void CalculateMaxCost(AlgorithmInformation information)
        {
            MaxCost = 0;
            foreach(var attraction in information.Attractions)
                MaxCost += attraction.Prices.Max();

        }

        public static void SetMaxNumberOfAttractions(AlgorithmInformation information)
        {
            MaxNumberOfAttractions = information.NumberOfAttractions;
        }

        public static double CalculateResult2(Genome genome, AlgorithmInformation information)
        {
            int visitedAttractions = genome.DayOrder.Sum(day => day.Count);

            int totalDistance = 0;
            foreach (var day in genome.DayOrder)
            {
                for (int i = 0; i < day.Count - 1; i++)
                {
                    totalDistance += information.AdjustmentMatrix[day[i]][day[i + 1]];
                }
            }

            double totalCost = 0.0;

            for (int i = 0; i < genome.DayOrder.Length; i++)
            {
                foreach (var attraction in genome.DayOrder[i])
                {
                    totalCost += information.Attractions[attraction].Prices[(i + information.WeekdayAtStart) % 7];
                }
            }

            double minVisitedAttractionsPenalty = Math.Max(0, MaxNumberOfAttractions / 2 - visitedAttractions);

            double normalizedVisitedAttractions = visitedAttractions / MaxNumberOfAttractions;
            double normalizedTotalDistance = 1.0 - (totalDistance / MaxPossibleDistance);
            double normalizedTotalCost = 1.0 - (totalCost / MaxCost);

            double fitness = (normalizedVisitedAttractions * WeightVisitedAttractions) +
                             (normalizedTotalDistance * WeightTotalDistance) +
                             (normalizedTotalCost * WeightTotalCost) -
                             (PenaltyFactor * minVisitedAttractionsPenalty);

            return fitness;
        }
    }
}
