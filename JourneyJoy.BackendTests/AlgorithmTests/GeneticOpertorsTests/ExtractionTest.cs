using JourneyJoy.Algorithm.Algorithms.GeneticOperators;
using JourneyJoy.Algorithm.Helpers;
using JourneyJoy.Algorithm.Models;
using JourneyJoy.Model.DTOs;
using JourneyJoy.UnitTests.AlgorithmTests.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.UnitTests.AlgorithmTests.GeneticOpertorsTests
{
    public class ExtractionTest
    {
        private AlgorithmInformation information;
        private static int Size => 20;

        [SetUp]
        public void Setup()
        {
            information = SetUpHelper.prepareAlgorithmInfo(Size);
        }

        [Test]
        public void CheckIfExtractionWorksProperly()
        {
            for (int i = 0; i < 100; i++)
            {
                var parent1 = new Genome(information, 0.1f);
                var parent2 = new Genome(information, 0.1f);

                (var child1, var child2) = Crossing.Execute(parent1, parent2);

                var child3 = Mutation.Execute(child1);
                var child4 = Mutation.Execute(child2);

                if (!Validator.Validate(child3, information))
                {
                    Extraction.Execute(child3, information);
                    var ifValid = Validator.Validate(child3, information);

                    ifValid.Should().BeTrue();
                }

                if (!Validator.Validate(child4, information))
                {
                    Extraction.Execute(child4, information);
                    var ifValid = Validator.Validate(child4, information);

                    ifValid.Should().BeTrue();
                }
            }
        }
    }
}
