using JJAlgorithm.Helpers;
using JJAlgorithm.Models;
using JourneyJoy.Algorithm.Algorithms;
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
        public static int PopulationSize => 999;
        public static float BoredomFactor => 0.2f;
        public List<(Genome individual, double fitnessValue)> Individuals { get; set; }

        public Population(AlgorithmInformation information)
        {
            Individuals = new List<(Genome, double)>();
            GeneratePopulation(information);
        }

        public void GeneratePopulation(AlgorithmInformation information)
        {
            FitnessFunction.CalculateMaximums(information);
            for (int i = 0; i < PopulationSize; i++)
            {
                var individual = new Genome(information, BoredomFactor);
                Individuals.Add((individual, FitnessFunction.CalculateResult(individual, information)));
            }
        }

        //public (Population bestPopulation, Population secondPopulation, Population thirdPopulation) SplitPopulation()
        //{
        //    Individuals = Individuals.OrderByDescending(x => x.fitnessValue).ToList();
        // }

    }
}
