using JourneyJoy.Algorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Algorithms.GeneticOperators
{
    public static class RouletteWheelSelector
    {
        public static List<(Genome individual, double fitnessValue)> Execute(int numberOfIndividuals, List<(Genome individual, double fitnessValue)> individuals)
        {
            var random = new Random();
            var selectedIndividuals = new List<(Genome individual, double fitnessValue)>();

            var sumFitnessValue = individuals.Sum(ind => ind.fitnessValue);
            var normalizedIndividuals = individuals.Select(ind => (ind.individual, ind.fitnessValue / sumFitnessValue)).ToList();

            for(int i = 0; i < numberOfIndividuals; i++)
            {
                var probability = random.NextDouble();
                var accumulatedProbability = 0.0;
                var selectedInividual = normalizedIndividuals.First();

                foreach (var normalizedIndividual in normalizedIndividuals)
                {
                    accumulatedProbability += normalizedIndividual.Item2;

                    if (probability <= accumulatedProbability)
                    {
                        selectedInividual = normalizedIndividual;
                        break;
                    }
                }

                normalizedIndividuals.Remove(selectedInividual);
                selectedIndividuals.Add(selectedInividual);
                sumFitnessValue -= selectedInividual.Item2 * sumFitnessValue;
            }

            return selectedIndividuals;
        }
    }
}
