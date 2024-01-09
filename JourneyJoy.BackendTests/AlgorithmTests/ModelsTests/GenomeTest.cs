using Azure.Messaging.EventGrid.SystemEvents;
using JourneyJoy.Algorithm.Helpers;
using JourneyJoy.Algorithm.Models;
using JourneyJoy.Model.DTOs;
using JourneyJoy.UnitTests.AlgorithmTests.Helpers;
using JourneyJoy.Utils.Security.HashAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.UnitTests.AlgorithmTests.ModelsTests
{
    public class GenomeTest
    {
        private AlgorithmInformation information;
        private static int Size => 20;

        [SetUp]
        public void Setup()
        {
            information = SetUpHelper.prepareAlgorithmInfo(Size);
        }

        [Test]
        public void CheckIfGenomeIsGeneratedProperly()
        {
            for (int i = 0; i < 1000; i++)
            {
                var genome = new Genome(information, 0.1f);

                var ifValidated = Validator.Validate(genome, information);

                genome.Should().NotBeNull();
                ifValidated.Should().BeTrue();

            }
        }
    }
}
