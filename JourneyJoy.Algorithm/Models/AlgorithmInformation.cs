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
        public int[][] AdjustmentMatrix { get; set; }
        public int StartPoint { get; set; }
        public int NumberOfDays { get; set; }
        public List<AttractionDTO> Attractions { get; set; }
        public int NumberOfAttractions => Attractions.Count;
        public int WeekdayAtStart { get; set; }
        public Time StartTime => new Time(7);
        public Time EndTime => new Time(23);

        public AlgorithmInformation(List<AttractionDTO> attractions, int[][] adjustmentMatrix, int startPoint, int numberOfDays, int weekdayAtStart)
        {
            Attractions = attractions;
            AdjustmentMatrix = adjustmentMatrix;
            StartPoint = startPoint;
            NumberOfDays = numberOfDays;
            WeekdayAtStart = weekdayAtStart;
        }

        public int DistanceToHome(int attractionIndex)
        {
            return AdjustmentMatrix[attractionIndex][StartPoint];
        }

        public int DistanceBetweenAttractions(int from, int to)
        {
            return AdjustmentMatrix[from][to];
        }
    }
}
