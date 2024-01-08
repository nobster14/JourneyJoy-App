using JourneyJoy.Algorithm.Algorithms.FixOperators;
using JourneyJoy.Algorithm.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Algorithms.GeneticOperators
{
    public static class GeneticOperations
    {
        #region Fields
        private static float EliteProbabilityOfMutation => 0.1f;
        private static float WorstProbabilityOfMutation => 0.3f;
        private static float MixedProbabilityOfMutation => 0.2f;
        #endregion

        #region Methods
        /// <summary>
        /// Generates 
        /// (worstPopulation size + bestPopulation size)! 
        /// / 
        /// (2! * (worstPopulation size + bestPopulation size - 2)!) offsprings.
        /// </summary>
        /// <param name="bestPopulation"></param>
        /// <param name="worstPopulation"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public static List<(Genome individual, double fitnessValue)> GenerateOffsprings(List<(Genome individual, double fitnessValue)> bestPopulation, List<(Genome individual, double fitnessValue)> worstPopulation, AlgorithmInformation information)
        {
            var goodOffsprings = GeneratePureOffsprings(bestPopulation, EliteProbabilityOfMutation, information);
            var badOffsprings = GeneratePureOffsprings(worstPopulation, WorstProbabilityOfMutation, information);
            var mixedOffsprings = GenerateMixedOffsprings(bestPopulation, worstPopulation, information);

            var allOffsprings = goodOffsprings.Concat(badOffsprings).Concat(mixedOffsprings).ToList();

            var evaluatedOffsprings = new List<(Genome individual, double fitnessValue)>();

            foreach (var offspring in allOffsprings)
            {
                evaluatedOffsprings.Add((offspring, FitnessFunction.CalculateResult(offspring, information)));
            }

            return evaluatedOffsprings.OrderByDescending(ind => ind.fitnessValue).ToList();
        }

        /// <summary>
        /// Generates offsprings from parents from the same population.
        /// </summary>
        /// <param name="population"></param>
        /// <param name="probabilityOfMutation"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public static List<Genome> GeneratePureOffsprings(List<(Genome individual, double fitnessValue)> population, float probabilityOfMutation, AlgorithmInformation information)
        {
            var offsprings = new List<Genome>();

            for (int i = 0; i < population.Count; i++)
            {
                for (int j = i + 1; j < population.Count; j++)
                {
                    (var child1, var child2) = Crossing.Execute(population[i].individual, population[j].individual);

                    var rand = new Random();

                    if (rand.NextDouble() < probabilityOfMutation)
                        child1 = Mutation.Execute(child1);

                    if (rand.NextDouble() < probabilityOfMutation)
                        child2 = Mutation.Execute(child2);

                    Extraction.Execute(child1, information);
                    Extraction.Execute(child2, information);

                    offsprings.Add(child1);
                    offsprings.Add(child2);
                }
            }

            return offsprings;
        }

        /// <summary>
        /// Generates offsprings from parents from different populations.
        /// </summary>
        /// <param name="bestPopulation"></param>
        /// <param name="worstPopulation"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public static List<Genome> GenerateMixedOffsprings(List<(Genome individual, double fitnessValue)> bestPopulation, List<(Genome individual, double fitnessValue)> worstPopulation, AlgorithmInformation information)
        {
            var offsprings = new List<Genome>();

            var rand = new Random();

            foreach (var parent1 in bestPopulation)
            {
                foreach (var parent2 in worstPopulation)
                {
                    (var child1, var child2) = Crossing.Execute(parent1.individual, parent2.individual);

                    if (rand.NextDouble() < MixedProbabilityOfMutation)
                        child1 = Mutation.Execute(child1);

                    if (rand.NextDouble() < MixedProbabilityOfMutation)
                        child2 = Mutation.Execute(child2);

                    Extraction.Execute(child1, information);
                    Extraction.Execute(child2, information);

                    offsprings.Add(child1);
                    offsprings.Add(child2);             
                }
            }

            return offsprings;
        }
        #endregion

    }
}
