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
        public static List<int>[] FindBestRoute(AlgorithmInformation information)
        {
            var basePopulation = new Population(information);

            return basePopulation.Individuals[0].individual.DayOrder;
        }
    }
}
