using JourneyJoy.Algorithm.Algorithms;
using JourneyJoy.Algorithm.Helpers;
using JourneyJoy.Algorithm.Models;
using JourneyJoy.Model.DTOs;
using JourneyJoy.UnitTests.AlgorithmTests.Helpers;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.UnitTests.AlgorithmTests.ModelsTests
{
    public class PopulationTest
    {
        private AlgorithmInformation information;
        private static int Size => 20;
        [SetUp]
        public void Setup()
        {
            information = SetUpHelper.prepareAlgorithmInfo(Size);
        }

        [Test]
        public void CheckIfPopulationGeneratesProperly()
        {
            var basePopulation = new Population(information, 100);
            FitnessFunction.CalculateMaximums(information);

            foreach((var individual, var fitnessValue) in basePopulation.Individuals)
            {
                var ifValid = Validator.Validate(individual, information);
                ifValid.Should().BeTrue();
                fitnessValue.Should().BeApproximately(FitnessFunction.CalculateResult(individual, information), 0.0001f);
            }
        }

        [Test]
        public void CheckIfPopulationDividesProperly()
        {
            var basePopulation = new Population(information, 100);
            (var bestPopulation, var worstPopulation) = basePopulation.DividePopulation();

            FitnessFunction.CalculateMaximums(information);

            foreach ((_, var fitnessValueB) in bestPopulation.Individuals)
                foreach((_, var fitnessValueW) in worstPopulation.Individuals)
                    fitnessValueW.Should().BeLessThanOrEqualTo(fitnessValueB);
        }

    }
}
