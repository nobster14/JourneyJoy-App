using JourneyJoy.Algorithm.Algorithms.FixOperators;
using JourneyJoy.Algorithm.Algorithms.GeneticOperators;
using JourneyJoy.Algorithm.Helpers;
using JourneyJoy.Algorithm.Models;
using JourneyJoy.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.UnitTests.AlgorithmTests
{
    public class ExtractionTest
    {
        private AlgorithmInformation information;

        [SetUp]
        public void Setup()
        {
            int[][] adjustmentMatrix = new int[][]
            {
                new int[]{0, 1, 2, 3, 4, 5, 5, 6, 7},
                new int[]{2, 0, 3, 2, 1, 2, 6, 6, 6},
                new int[]{3, 4, 0, 5, 3, 2, 3, 3, 3},
                new int[]{3, 4, 2, 0, 3, 2, 7, 7, 7},
                new int[]{3, 4, 2, 5, 0, 2, 7, 7, 7},
                new int[]{3, 4, 7, 5, 3, 0, 2, 2, 2},
                new int[]{4, 1, 2, 3, 4, 5, 0, 6, 7},
                new int[]{2, 8, 3, 2, 1, 2, 6, 0, 6},
                new int[]{3, 4, 5, 5, 3, 2, 3, 3, 0}
            };

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    adjustmentMatrix[i][j] *= 50;
                }
            }

            int startPoint = 0;
            int numberOfDays = 3;
            int weekdayAtStart = 0;
            AttractionDTO attraction1 = new()
            {
                LocationType = Model.Enums.LocationType.WithoutHours,
                Prices = new double[7]
            };
            AttractionDTO attraction2 = new()
            {
                LocationType = Model.Enums.LocationType.WithHours,
                OpenHours = new string[][]
                            {
                                new string[] { "0800", "1000" },
                                new string[] { "0800", "1500" },
                                new string[] { "0800", "1400" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                            },
                Prices = new double[] { 7, 3, 4, 2, 1, 5, 5 },
                TimeNeeded = 120
            };
            AttractionDTO attraction3 = new()
            {
                LocationType = Model.Enums.LocationType.WithHours,
                OpenHours = new string[][]
                            {
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "1600" },
                                new string[] { "0800", "1200" },
                                new string[] { "0800", "1000" },
                            },
                Prices = new double[] { 5, 5, 5, 5, 5, 5, 5 },
                TimeNeeded = 60
            };
            List<AttractionDTO> list = new()
            {
                attraction1,
                attraction2,
                attraction3,
                attraction2,
                attraction3,
                attraction2,
                attraction3,
                attraction2,
                attraction3
            };

            information = new AlgorithmInformation(list, adjustmentMatrix, startPoint, numberOfDays, weekdayAtStart);
        }

        [Test]
        public void CheckIfExtractionWorksProperly()
        {
            for(int i = 0; i < 100; i++)
            {
                var parent1 = new Genome(information, 0.1f);
                var parent2 = new Genome(information, 0.1f);

                (var child1, var child2) = Crossing.Execute(parent1, parent2);
            
                var child3 = Mutation.ExecuteAttractionMutation(child1);
                var child4 = Mutation.ExecuteTwoAttractionsMutation(child2);

                if(!Validator.Validate(child3, information))
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
