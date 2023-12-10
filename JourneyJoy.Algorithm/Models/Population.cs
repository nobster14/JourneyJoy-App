using JourneyJoy.Model.DTOs;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Models
{
    public class Population
    {
        private static int PopulationSize => 10;
        private static DateTime StartTime => new DateTime(1, 1, 1, 7, 0, 0);
        private static DateTime MaxEndHour => new DateTime(1, 1, 1, 22, 0, 0);
        private static float BoredomFactor => 0.2f;
        public List<Genome> Individuals { get; set; }

        public Population(AlgorithmInformation information) 
        {
            Individuals = new List<Genome>();
            GeneratePopulation(information); 
        }

        public void GeneratePopulation(AlgorithmInformation information)
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Individuals.Add(GenerateIndividual(information));

            }
        }

        private static Genome GenerateIndividual(AlgorithmInformation information)
        {
            Genome individual = new(information.NumberOfAttractions, information.NumberOfDays);
            bool[] visited = new bool[information.NumberOfAttractions];

            List<int> attractionsToVisit = new();
            for (int i = 0; i < information.NumberOfAttractions; i++)
            {
                if (i != information.StartPoint)
                    attractionsToVisit.Add(i);
            }

            for (int j = 0; j < information.NumberOfDays; j++)
                GenerateIndividualsDay(ref individual, ref attractionsToVisit, j, information);

            return individual;
        }

        public static void GenerateIndividualsDay(ref Genome individual, ref List<int> attractionsToVisit, int dayNumber, AlgorithmInformation information)
        {
            Random random = new();
            bool nextMovePossible = true;

            int currentLocation = information.StartPoint;

            List<int> currentRoute = new();

            while (nextMovePossible)
            {
                Dictionary<double, int> probabilities = new();
                SortedList<int, int> distances = new();

                if (random.NextDouble() < BoredomFactor)
                {
                    // todo: powrót do domu, przejście do kolejnego dnia
                    return;
                }
                else
                {
                    foreach(var neighbour in attractionsToVisit)
                    {

                    }
                }
            }
        }
    }
}
