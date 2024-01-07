using JourneyJoy.Algorithm.Algorithms.GeneticOperators;
using JourneyJoy.Algorithm.Models;
using JourneyJoy.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Algorithms
{
    public static class GeneticAlgorithm
    {
        private static int StagnationFactor => 20;
        private static int PopulationSize => 100;

        public static List<int>[] FindBestRoute(AlgorithmInformation information)
        {
            var basePopulation = new Population(information, PopulationSize);

            (var bestPopulation, var worstPopulation) = basePopulation.DividePopulation();

            var stagnation = 0;
            for (int i = 0; i < 10000; i++)
            {
                bool ifBestIndividualChanged = ExecuteAlgorithmStep(ref bestPopulation, ref worstPopulation, information);
                if (!ifBestIndividualChanged)
                    stagnation++;
                else
                {
                    stagnation = 0;
                }

                if (stagnation > StagnationFactor)
                    break;
            }

            return bestPopulation.Individuals.First().individual.DayOrder;
        }

        public static bool ExecuteAlgorithmStep(ref Population bestPopulation, ref Population worstPopulation, AlgorithmInformation information)
        {
            bool ifBestIndividualChanged = false;

            var bestIndividuals = new List<(Genome individual, double fitnessValue)>(bestPopulation.Individuals);
            var worstIndividuals = new List<(Genome individual, double fitnessValue)>(worstPopulation.Individuals);

            var bestParents = RouletteWheelSelector.Execute(20, bestPopulation.Individuals);
            var worstParents = RouletteWheelSelector.Execute(10, worstPopulation.Individuals);

            var offsprings = GeneticOperations.GenerateOffsprings(bestParents, worstParents, information);

            if (offsprings.First().fitnessValue > bestPopulation.Individuals.First().fitnessValue)
                ifBestIndividualChanged = true;

            if (offsprings.First().fitnessValue > bestPopulation.Individuals.Last().fitnessValue)
                bestIndividuals = bestIndividuals.Concat(offsprings).ToList().OrderByDescending(ind => ind.fitnessValue).ToList();
            else
                worstIndividuals = worstIndividuals.Concat(offsprings).ToList().OrderByDescending(ind => ind.fitnessValue).ToList();

            bestPopulation.Individuals = bestIndividuals.Take(bestPopulation.PopulationSize).ToList();

            worstIndividuals = worstIndividuals.Concat(bestIndividuals.Skip(bestPopulation.PopulationSize)).ToList();

            worstPopulation.Individuals = RouletteWheelSelector.Execute(worstPopulation.PopulationSize, worstPopulation.Individuals).OrderByDescending(ind => ind.fitnessValue).ToList();

            return ifBestIndividualChanged;
        }

    }
}
