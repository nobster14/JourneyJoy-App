using JourneyJoy.Algorithm.Algorithms;
using JourneyJoy.Algorithm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JourneyJoy.UnitTests.AlgorithmTests.Helpers;
using System.Reflection;

namespace JourneyJoy.UnitTests.AlgorithmTests
{
    public class AlgorithmTest
    {
        private AlgorithmInformation information;
        private static int Size => 20;

        [SetUp]
        public void Setup()
        {
            information = SetUpHelper.prepareAlgorithmInfo(Size);
        }


        [Test]
        public void CheckIfAlgorithmWorks()
        {
            var bestRoute = GeneticAlgorithm.FindBestRoute(information);

            var valid = Algorithm.Helpers.Validator.Validate(bestRoute, information);

            valid.Should().BeTrue();
            bestRoute.Should().NotBeNull();
        }

        [Test]
        public void CheckIfAlgorithmStepWorks()
        {
            var basePopulation = new Population(information, 500);
            (var bestPopulation, var worstPopulation) = basePopulation.DividePopulation();

            var bestIndivValue = bestPopulation.Individuals.First().fitnessValue;

            MethodInfo methodInfo = typeof(GeneticAlgorithm).GetMethod("ExecuteAlgorithmStep", BindingFlags.Public | BindingFlags.Static);

            if (methodInfo != null)
            {
                var result = methodInfo.Invoke(null, new object[] { bestPopulation, worstPopulation, information });
                result.Should().NotBeNull();

                if (bestIndivValue < bestPopulation.Individuals.First().fitnessValue)
                    result.Should().Be(true);
                else
                    result.Should().Be(false);
            }

        }
    }
}
