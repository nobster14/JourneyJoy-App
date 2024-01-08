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
        #region Fields
        public static double WeightVisitedAttractions => 0.3;
        public static double WeightTotalDistance => 0.5;
        public static double WeightTotalCost => 0.1;
        public static double PenaltyFactor => 0.2;

        public static int MaxPossibleDistance { get; private set; }
        public static int MaxNumberOfAttractions { get; private set; }
        public static double MaxCost { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates value of a fitness function for specified route.
        /// </summary>
        /// <param name="genome"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public static double CalculateResult(Genome genome, AlgorithmInformation information)
        {
            int visitedAttractions = genome.DayOrder.Sum(day => day.Count);
            int totalDistance = 0;
            double totalCost = 0.0;

            foreach (var day in genome.DayOrder)
                for (int i = 0; i < day.Count - 1; i++)
                    totalDistance += information.AdjustmentMatrix[day[i]][day[i + 1]];

            for (int i = 0; i < genome.DayOrder.Length; i++)
                foreach (var attraction in genome.DayOrder[i])
                    totalCost += information.Attractions[attraction].Prices[(i + information.WeekdayAtStart) % 7];

            double minVisitedAttractionsPenalty = Math.Max(0, MaxNumberOfAttractions - visitedAttractions - 1);

            double normalizedVisitedAttractions = visitedAttractions / MaxNumberOfAttractions;
            double normalizedTotalDistance = 1.0 - (totalDistance / MaxPossibleDistance);
            double normalizedTotalCost = 1.0 - (totalCost / MaxCost);

            double fitness = (normalizedVisitedAttractions * WeightVisitedAttractions) +
                             (normalizedTotalDistance * WeightTotalDistance) +
                             (normalizedTotalCost * WeightTotalCost) -
                             (PenaltyFactor * minVisitedAttractionsPenalty);

            return fitness;
        }

        /// <summary>
        /// Calculates maximum number of attractions, maximum distance and maximum cost.
        /// </summary>
        /// <param name="information"></param>
        public static void CalculateMaximums(AlgorithmInformation information)
        {
            CalculateMaxCost(information);
            CalculateMaxDistance(information);
            SetMaxNumberOfAttractions(information);
        }

        /// <summary>
        /// Calculates sum of all edges in specified graph.
        /// </summary>
        /// <param name="information"></param>
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

        /// <summary>
        /// Calculates sum of maximum costs for each attraction.
        /// </summary>
        /// <param name="information"></param>
        public static void CalculateMaxCost(AlgorithmInformation information)
        {
            MaxCost = 0;
            foreach(var attraction in information.Attractions)
                MaxCost += attraction.Prices.Max();

        }

        /// <summary>
        /// Sets maximum number of chosen attractions.
        /// </summary>
        /// <param name="information"></param>
        public static void SetMaxNumberOfAttractions(AlgorithmInformation information)
        {
            MaxNumberOfAttractions = information.NumberOfAttractions;
        }

        #endregion
    }
}
