using JJAlgorithm.Models;
using JourneyJoy.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Models
{
    public class AlgorithmInformation
    {
        #region Fields
        public int[][] AdjustmentMatrix { get; set; }
        public int StartPoint { get; set; }
        public int NumberOfDays { get; set; }
        public List<AttractionDTO> Attractions { get; set; }
        public int NumberOfAttractions => Attractions.Count;
        public int WeekdayAtStart { get; set; }
        public static Time StartTime => new(7);
        public static Time EndTime => new(23);
        #endregion

        public AlgorithmInformation(List<AttractionDTO> attractions, int[][] adjustmentMatrix, int startPoint, int numberOfDays, int weekdayAtStart)
        {
            Attractions = attractions;
            AdjustmentMatrix = adjustmentMatrix;
            StartPoint = startPoint;
            NumberOfDays = numberOfDays;
            WeekdayAtStart = weekdayAtStart;
        }

        /// <summary>
        /// Returns distance from attraction to home.
        /// </summary>
        /// <param name="attractionIndex"></param>
        /// <returns></returns>
        public int DistanceToHome(int attractionIndex)
        {
            return AdjustmentMatrix[attractionIndex][StartPoint];
        }

        /// <summary>
        /// Return distance between two attractions.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public int DistanceBetweenAttractions(int from, int to)
        {
            return AdjustmentMatrix[from][to];
        }
    }
}
