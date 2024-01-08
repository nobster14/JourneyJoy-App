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
    public class MutationTest
    {
        private AlgorithmInformation information;
        private static int Size => 20;

        [SetUp]
        public void Setup()
        {
            information = SetUpHelper.prepareAlgorithmInfo(Size);
        }

        [Test]
        public void CheckIfOfOneAttractionMutationWorks()
        {
            Genome parent1 = new Genome(information, 0.2f);

            var child = Mutation.ExecuteAttractionMutation(parent1);

            child.Should().NotBeNull();
        }

        [Test]
        public void CheckIfOfTwoAttractionsMutationWorks()
        {
            Genome parent1 = new Genome(information, 0.2f);

            var child = Mutation.ExecuteTwoAttractionsMutation(parent1);

            child.Should().NotBeNull();
        }
    }
}
