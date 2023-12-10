using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Models
{
    public class Genome
    {
        public bool[,] DayChoiceMatrix { get; set; }
        public List<int>[] DayOrder { get; set; }
        public List<int> MissedAttractions { get; set; }

        public Genome(int numberOfAttractions, int numberOfDays)
        {
            DayChoiceMatrix = new bool[numberOfAttractions, numberOfDays];
            DayOrder = new List<int>[numberOfDays];
        }

        public Genome(bool[,] dayChoiceMatrix, List<int>[] dayOrder)
        {
            DayChoiceMatrix = dayChoiceMatrix;
            DayOrder = dayOrder;
        }
    }
}
