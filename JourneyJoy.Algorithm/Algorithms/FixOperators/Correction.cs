using JourneyJoy.Algorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Algorithms.FixOperators
{
    public static class Correction
    {
        public static void Execute(Genome genome, AlgorithmInformation information)
        {
            var missedAttractionsOrder = GetRandomOrder(0, genome.MissedAttractions.Count - 1);

        }
        private static int[] GetRandomOrder(int startRange, int endRange)
        {
            Random random = new();
            return Enumerable.Range(startRange, endRange).OrderBy(x => random.Next()).ToArray();
        }
    }
}
