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

        public AlgorithmInformation(List<AttractionDTO> attractions, int[][] adjustmentMatrix, int startPoint, int numberOfDays)
        {
            Attractions = attractions;
            AdjustmentMatrix = adjustmentMatrix;
            StartPoint = startPoint;
            NumberOfDays = numberOfDays;
        }
    }
}
