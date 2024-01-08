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
    public class PopulationTest
    {
        private AlgorithmInformation information;
        private static int Size => 20;
        [SetUp]
        public void Setup()
        {
            information = SetUpHelper.prepareAlgorithmInfo(Size);
        }

      //  [Test]
      //  public void CheckIfPopulationGeneratesProperly()
      //  {
      //      var population = new Population(information, 100);
      //
      //     var populationSize = population.Individuals.Count;
      //  }

    }
}
