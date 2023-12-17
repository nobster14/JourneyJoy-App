using JJAlgorithm.Helpers;
using JJAlgorithm.Models;
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
        public static float BoredomFactor => 0.2f;
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
                Individuals.Add(new Genome(information, BoredomFactor));
            }
        }

    }
}
