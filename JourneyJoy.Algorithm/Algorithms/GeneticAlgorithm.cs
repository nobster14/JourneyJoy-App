using JourneyJoy.Algorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Algorithms
{
    public static class GeneticAlgorithm
    {
        public static List<int> FindShortestRoute(List<Attraction> attractions, int populationSize, int numberOfGenerations, double crossoverProbability, double mutationProbability)
        {
            var adjustmentMatrix = CreateAdjustmentMatrix(attractions);

            var population = InitializePopulation(populationSize, attractions.Count);
            var bestRoute = new List<int>();
            var bestRouteLength = float.MaxValue;

            for (int i = 0; i < numberOfGenerations; i++)
            {
                var parents = SelectParents(population, crossoverProbability);
                var offspring = GenerateOffspring(parents);

                Mutate(offspring, mutationProbability);

                var populationWithOffspring = CombinePopulation(population, offspring);
                population = SelectSurvivors(populationWithOffspring, populationSize, adjustmentMatrix);

                var currentBestRoute = population.OrderBy(route => ObjectiveFunction.CalculateResult(route, adjustmentMatrix)).First();
                var currentBestRouteLength = ObjectiveFunction.CalculateResult(currentBestRoute, adjustmentMatrix);

                if (currentBestRoute[0] != 0)
                {
                    var zeroIndex = Array.IndexOf(currentBestRoute, 0);
                    currentBestRoute = currentBestRoute.Skip(zeroIndex).Concat(currentBestRoute.Take(zeroIndex)).ToArray();
                    currentBestRouteLength = ObjectiveFunction.CalculateResult(currentBestRoute, adjustmentMatrix);
                }

                if (currentBestRouteLength < bestRouteLength)
                {
                    bestRoute = currentBestRoute.ToList();
                    bestRouteLength = currentBestRouteLength;
                }
            }

            return bestRoute;
        }

        private static float[,] CreateAdjustmentMatrix(List<Attraction> attractions)
        {
            var adjustmentMatrix = new float[attractions.Count, attractions.Count];
            for (int i = 0; i < attractions.Count; i++)
            {
                var attr1 = attractions[i].GetLatLon();

                for (int j = i + 1; j < attractions.Count; j++)
                {
                    var attr2 = attractions[j].GetLatLon();
                    adjustmentMatrix[i, j] = adjustmentMatrix[j, i] = Haversine.CalculateFormula(attr1.Lat, attr1.Lon, attr2.Lat, attr2.Lon);
                }
            }
            return adjustmentMatrix;
        }

        private static List<int[]> InitializePopulation(int populationSize, int numberOfVertices)
        {
            var population = new List<int[]>();
            var vertices = Enumerable.Range(1, numberOfVertices - 1).ToArray();
            Random random = new();

            for (int i = 0; i < populationSize; i++)
            {
                var individual = vertices.OrderBy(v => random.Next()).ToArray();
                population.Add(individual);
            }

            return population;
        }

        private static List<int[]> SelectParents(List<int[]> population, double crossoverProbability)
        {
            var parents = new List<int[]>();
            Random random = new();

            for (int i = 0; i < population.Count; i++)
            {
                if (random.NextDouble() < crossoverProbability)
                {
                    parents.Add(population[i]);
                }
            }

            return parents;
        }

        private static List<int[]> GenerateOffspring(List<int[]> parents)
        {
            var offspring = new List<int[]>();

            for (int i = 0; i < parents.Count; i += 2)
            {
                var parent1 = parents[i];
                var parent2 = i + 1 < parents.Count ? parents[i + 1] : parents[0];
                var child = Crossover(parent1, parent2);
                offspring.Add(child);
            }

            return offspring;
        }

        private static int[] Crossover(int[] parent1, int[] parent2)
        {
            Random random = new();
            int[] child = new int[parent1.Length];
            var cutPoint1 = random.Next(0, parent1.Length);
            var cutPoint2 = random.Next(0, parent1.Length);

            if (cutPoint1 > cutPoint2)
            {
                (cutPoint2, cutPoint1) = (cutPoint1, cutPoint2);
            }

            for (int i = cutPoint1; i <= cutPoint2; i++)
            {
                child[i] = parent1[i];
            }

            int j = 0;
            for (int i = 0; i < parent2.Length; i++)
            {
                if (parent2[i] != 0 && !child.Contains(parent2[i]))
                {
                    while (child[j] != 0)
                    {
                        j++;
                    }

                    child[j] = parent2[i];
                }
            }

            return child;
        }

        private static void Mutate(List<int[]> population, double mutationProbability)
        {
            Random random = new();
            foreach (var solution in population)
            {
                if (random.NextDouble() < mutationProbability)
                {
                    var index1 = random.Next(0, solution.Length);
                    var index2 = random.Next(0, solution.Length);
                    var temp = solution[index1];
                    solution[index1] = solution[index2];
                    solution[index2] = temp;
                }
            }
        }

        private static List<int[]> CombinePopulation(List<int[]> population1, List<int[]> population2)
        {
            return population1.Concat(population2).ToList();
        }

        private static List<int[]> SelectSurvivors(List<int[]> population, int populationSize, float[,] adjustmentMatrix)
        {
            var sortedPopulation = population.OrderBy(solution => ObjectiveFunction.CalculateResult(solution, adjustmentMatrix)).ToList();
            return sortedPopulation.GetRange(0, populationSize);
        }
    }
}
