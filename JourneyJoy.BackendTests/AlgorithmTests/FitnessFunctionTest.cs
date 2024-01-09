using JourneyJoy.Algorithm.Algorithms;
using JourneyJoy.Algorithm.Models;
using JourneyJoy.UnitTests.AlgorithmTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.UnitTests.AlgorithmTests
{
    public class FitnessFunctionTest
    {
        private AlgorithmInformation information;
        private static int Size => 5;

        private static int GetMaxDist()
        {
            int tmp = 0;
            for (int i = 1; i < Size; i++)
                tmp += i;

            return tmp;
        }

        [SetUp]
        public void Setup()
        {
            information = SetUpHelper.prepareSimpleAlgorithmInfo(Size);      
        }

        [Test]
        public void CheckIfMaximumNumberOfAttractionsSetsProperly()
        {
            FitnessFunction.SetMaxNumberOfAttractions(information);
            FitnessFunction.MaxNumberOfAttractions.Should().Be(Size);
        }

        [Test]
        public void CheckIfMaxDistanceCalculatesProperly()
        {
            FitnessFunction.CalculateMaxDistance(information);
            FitnessFunction.MaxPossibleDistance.Should().Be(GetMaxDist());
        }

        [Test]
        public void CheckIfMaxCostCalculatesProperly()
        {
            FitnessFunction.CalculateMaxCost(information);
            FitnessFunction.MaxCost.Should().Be(100 * (Size - 1));
        }

        [Test]
        public void CheckIfMaximumsCalculateProperly()
        {
            FitnessFunction.CalculateMaximums(information);
            FitnessFunction.MaxNumberOfAttractions.Should().Be(Size);
            FitnessFunction.MaxPossibleDistance.Should().Be(GetMaxDist());
            FitnessFunction.MaxCost.Should().Be(100 * (Size - 1));
        }

        [Test]
        public void CheckIfFitnessValueCalculatesProperlyGenome()
        {
            Genome genome = new Genome(Size, 1);
            genome.DayOrder[0] = new List<int>() { 1, 2, 3, 4 };

            FitnessFunction.CalculateMaximums(information);

            var result = FitnessFunction.CalculateResult(genome, information);
            result.Should().Be(0.5);
        }
    }
}
