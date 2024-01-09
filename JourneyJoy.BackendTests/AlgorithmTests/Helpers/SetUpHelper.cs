using JourneyJoy.Algorithm.Models;
using JourneyJoy.Model.DTOs;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.UnitTests.AlgorithmTests.Helpers
{
    public static class SetUpHelper
    {
        public static AlgorithmInformation prepareAlgorithmInfo(int Size)
        {
            var adjustmentMatrix = new int[Size][];
            var randomizer = new Random();

            for (int i = 0; i < Size; i++)
                adjustmentMatrix[i] = new int[Size];

            for (int i = 0; i < Size; i++)
                for (int j = i + 1; j < Size; j++)
                    adjustmentMatrix[i][j] = adjustmentMatrix[j][i] = randomizer.Next(120);

            int startPoint = 0;
            int numberOfDays = 5;
            int weekdayAtStart = 0;

            AttractionDTO home = new()
            {
                LocationType = Model.Enums.LocationType.WithoutHours,
                Prices = new double[7]
            };
            AttractionDTO attraction2 = new()
            {
                LocationType = Model.Enums.LocationType.WithHours,
                OpenHours = new string[][]
                            {
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                            },
                Prices = new double[] { 100, 5, 20, 20, 20, 20, 100 },
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
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                            },
                Prices = new double[] { 0, 0, 0, 105, 105, 105, 105 },
                TimeNeeded = 60
            };
            AttractionDTO attraction4 = new()
            {
                LocationType = Model.Enums.LocationType.WithHours,
                OpenHours = new string[][]
                            {
                                new string[] { "0900", "1400" },
                                new string[] { "0900", "1400" },
                                new string[] { "0900", "1400" },
                                new string[] { "0900", "1400" },
                                new string[] { "0900", "1400" },
                                new string[] { "0900", "1400" },
                                new string[] { "0900", "1400" },
                            },
                Prices = new double[] { 0, 0, 0, 0, 0, 0, 0 },
                TimeNeeded = 180
            };
            AttractionDTO attraction5 = new()
            {
                LocationType = Model.Enums.LocationType.WithHours,
                OpenHours = new string[][]
                            {
                                new string[] { "0000", "0000" },
                                new string[] { "0000", "0000" },
                                new string[] { "0000", "0000" },
                                new string[] { "0000", "0000" },
                                new string[] { "0000", "0000" },
                                new string[] { "0000", "0000" },
                                new string[] { "0000", "0000" },
                            },
                Prices = new double[] { 0, 0, 0, 0, 0, 0, 0 },
                TimeNeeded = 180
            };

            List<AttractionDTO> list = new()
            {
                home,
                attraction2,
                attraction3,
                attraction4,
                attraction5,
                attraction2,
                attraction3,
                attraction4,
                attraction5,
                attraction2,
                attraction3,
                attraction4,
                attraction5,
                attraction2,
                attraction3,
                attraction4,
                attraction5,
                attraction2,
                    attraction3,
                    attraction4
                };

            return new AlgorithmInformation(list, adjustmentMatrix, startPoint, numberOfDays, weekdayAtStart);

        }

        public static AlgorithmInformation prepareSimpleAlgorithmInfo(int Size)
        {
            int[][] adjustmentMatrix = new int[Size][];
            for (int i = 0; i < Size; i++)
                adjustmentMatrix[i] = new int[Size];

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    if (i < j)
                        adjustmentMatrix[i][j] = 1;

            int startPoint = 0;
            int numberOfDays = 5;
            int weekdayAtStart = 0;

            AttractionDTO home = new()
            {
                LocationType = Model.Enums.LocationType.WithoutHours,
                Prices = new double[7]
            };

            AttractionDTO attraction = new()
            {
                LocationType = Model.Enums.LocationType.WithHours,
                OpenHours = new string[][]
                            {
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                                new string[] { "0800", "2000" },
                            },
                Prices = new double[] { 100, 5, 5, 5, 5, 5, 5 },
                TimeNeeded = 120
            };


            List<AttractionDTO> list = new();

            list.Add(home);

            for (int i = 1; i < Size; i++)
                list.Add(attraction);

            return new AlgorithmInformation(list, adjustmentMatrix, startPoint, numberOfDays, weekdayAtStart);

        }
    }
}
