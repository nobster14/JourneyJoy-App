using JJAlgorithm.Models;
using JourneyJoy.Algorithm.Algorithms;
using JourneyJoy.Model.DTOs;
using Microsoft.Identity.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Models
{
    public class Population
    {
        public int PopulationSize { get; private set; }
        public static float BoredomFactor => 0.1f;
        public List<(Genome individual, double fitnessValue)> Individuals { get; set; }

        public Population(AlgorithmInformation information, int populationSize)
        {
            PopulationSize = populationSize;
            Individuals = new List<(Genome, double)>();

            GeneratePopulation(information);
        }

        public Population(List<(Genome individual, double fitnessValue)> individuals) 
        {
            Individuals = new List<(Genome individual, double fitnessValue)>(individuals);
            PopulationSize = individuals.Count;
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

        public (Population bestPopulation, Population worstPopulation) DividePopulation()
        {
            Individuals = Individuals.OrderByDescending(ind => ind.fitnessValue).ToList();

            int halfCount = PopulationSize / 2;

            var bestIndividuals = Individuals.Take(halfCount).ToList();
            var worstIndividuals = Individuals.Skip(halfCount).ToList();

            return (new Population(bestIndividuals), new Population(worstIndividuals));
        }

    }
}
