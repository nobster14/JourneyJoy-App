using JourneyJoy.Algorithm.Algorithms.GeneticOperators;
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

namespace JourneyJoy.UnitTests.AlgorithmTests
{
    public class CrossingTest
    {
        private AlgorithmInformation information;
        private static int Size => 20;

        [SetUp]
        public void Setup()
        {
            information = SetUpHelper.prepareAlgorithmInfo(Size);
        }

        [Test]
        public void CheckIfCrossingWorks()
        {
            Genome parent1 = new Genome(information, 0.2f);
            Genome parent2 = new Genome(information, 0.2f);

            (var child1, var child2) = Crossing.Execute(parent1, parent2);

            child1.Should().NotBeNull();
            child2.Should().NotBeNull();
        }
    }
}
