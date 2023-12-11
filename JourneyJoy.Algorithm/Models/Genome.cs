using JJAlgorithm.Helpers;
using JJAlgorithm.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Models
{
    public class Genome
    {
        public bool[,] DayChoiceMatrix { get; set; }
        public List<int>[] DayOrder { get; set; }
        public List<int> MissedAttractions { get; set; }
        public Time[] StartTime { get; set; }
        public Time[] EndTime { get; set; }

        public Genome(int numberOfAttractions, int numberOfDays)
        {
            DayChoiceMatrix = new bool[numberOfAttractions, numberOfDays];
            DayOrder = new List<int>[numberOfDays];

            StartTime = new Time[numberOfDays];
            EndTime = new Time[numberOfDays];

            for (int i = 0; i < numberOfDays; i++)
            {
                StartTime[i].Hour = 7;
                EndTime[i].Hour = 23;
            }
        }

        public Genome(AlgorithmInformation information, Time startHour, Time endHour, float boredomFactor)
        {
            bool[] visited = new bool[information.NumberOfAttractions];

            StartTime = new Time[information.NumberOfDays];
            EndTime = new Time[information.NumberOfDays];

            for (int i = 0; i < information.NumberOfDays; i++)
            {
                StartTime[i].Hour = 7;
                EndTime[i].Hour = 23;
            }

            MissedAttractions = new();
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
            Time currentTime = StartTime[dayNumber];

            Random random = new();
            bool nextMovePossible = true;

            int currentLocation = information.StartPoint;

            List<int> currentRoute = new();

            while (nextMovePossible)
            {
                if (random.NextDouble() < boredomFactor)
                {
                    // todo: powrót do domu, przejście do kolejnego dnia
                    return;
                }
                else
                {
                    var ret = GetRandomAttraction(information, currentLocation);

                }
            }
        }

        private (int attraction, bool ifPossible) GetRandomAttraction(AlgorithmInformation information, int currentLocation, Time currentTime)
        {
            List<(float dist, int index)> distances = new();
            List<(double probability, int index)> probabilities = new();

            Random random = new();

            MissedAttractions.Sort(new DistanceComparer(information.AdjustmentMatrix[currentLocation]));

            int sum = 0;

            foreach (var neighbour in MissedAttractions)
            {
                if (CheckIfMovePossible(currentLocation, neighbour, currentTime, information))
                {
                    distances.Add((information.AdjustmentMatrix[currentLocation][neighbour] + sum, neighbour));
                    sum += information.AdjustmentMatrix[currentLocation][neighbour];
                }   
            }

            foreach (var dist in distances)
            {
                probabilities.Add((dist.dist / sum, dist.index));
            }

            var randomNumber = random.NextDouble();

            for(int i = 0; i < probabilities.Count; i++)
            {
                if(randomNumber <= probabilities[i].probability)
                {
                    return (i, true);
                }
            }

            return (-1, false);
        }

        private bool CheckIfMovePossible(int from, int to, Time currentTime, AlgorithmInformation information)
        {
            return true;
        }
    }
}
