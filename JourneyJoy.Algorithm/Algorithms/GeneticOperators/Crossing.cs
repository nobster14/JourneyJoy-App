using JourneyJoy.Algorithm.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Algorithms.GeneticOperators
{
    public static class Crossing
    {
        public static (Genome child1, Genome child2) Execute(Genome parent1, Genome parent2)
        {
            var parentChoice = GetRandomParentsOrder(parent1.NumberOfAttractions);

            var child1 = CreateChildFromParents(parent1, parent2, parentChoice);
            var child2 = CreateChildFromParents(parent2, parent1, parentChoice);

            return (child1, child2);
        }

        private static Genome CreateChildFromParents(Genome parent1, Genome parent2, int[] parentChoice)
        {
            Genome child = CopyDayChoiceFromParents(parent1, parent2, parentChoice);

            for (int i = 0; i < child.NumberOfDays; i++)
                CopyDayOrder(child, i, parentChoice, parent1, parent2);

            for (int i = 0; i < child.NumberOfAttractions; i++)
            {
                int counter = 0;
                for (int j = 0; j < child.NumberOfDays; j++)
                {
                    if (child.DayChoiceMatrix[i, j] == true)
                        counter++;
                }
                if (counter == 0 && child.StartPoint != i)
                    child.MissedAttractions.Add(i);
            }

            return child;
        }

        private static void CopyDayOrder(Genome child, int i, int[] parentChoice, Genome parent1, Genome parent2)
        {
            int beginningCnt = child.DayOrder[i].Count;

            List<int> attractions = new (child.DayOrder[i]);
            var attrCount = attractions.Count;

            child.DayOrder[i].Clear();

            if (attrCount > 0)
            {
                var parentTable = new int[attrCount];

                var par1 = new List<int>();
                var par2 = new List<int>();

                for (int j = 0; j < attrCount; j++)
                {
                    parentTable[j] = parentChoice[attractions[j]];

                    if (parentChoice[attractions[j]] == 0)
                        par1.Add(attractions[j]);
                    else
                        par2.Add(attractions[j]);
                }

                var indexes = GetRandomOrder(0, attrCount);
                var parentOrder = new int[attrCount];

                 for (int j = 0; j < attrCount; j++)
                     parentOrder[j] = parentTable[indexes[j]];

                var rand = new Random();

                for (int j = 0; j < attrCount; j++)
                {
                    int iterator1 = 0;
                    int iterator2 = 0;

                    if (parentOrder[j] == 0)
                        AddAttractionsFromParent(child.DayOrder[i], parent1.DayOrder[i], par1, ref iterator1);
                    else
                       AddAttractionsFromParent(child.DayOrder[i], parent2.DayOrder[i], par2, ref iterator2);
                }

                int endCnt = child.DayOrder[i].Count;
            }
        }


        private static void AddAttractionsFromParent(List<int> childDayOrder, List<int> parentDayOrder, List<int> selectedAttractions, ref int iterator)
        {
            while (iterator < parentDayOrder.Count)
            {
                var currAttr = parentDayOrder[iterator];
                iterator++;

                if (selectedAttractions.Contains(currAttr))
                {
                    childDayOrder.Add(currAttr);
                    selectedAttractions.Remove(currAttr);
                    break;
                }
            }
        }

        private static Genome CopyDayChoiceFromParents(Genome parent1, Genome parent2, int[] parentChoice)
        {
            Genome child = new(parent1.NumberOfAttractions, parent1.NumberOfDays);

            for (int i = 0; i < parentChoice.Length; i++)
            {
                int index = -1;

                if (parentChoice[i] == 0 && parent1.StartPoint != i)
                    index = parent1.GetDayIndexOfAttraction(i);
                else if (parentChoice[i] == 1 && parent2.StartPoint != i)
                    index = parent2.GetDayIndexOfAttraction(i);

                if (index != -1)
                {
                    child.DayChoiceMatrix[i, index] = true;
                    child.DayOrder[index].Add(i);
                }
            }

            return child;
        }

        private static int[] GetRandomOrder(int startRange, int endRange)
        {
            Random random = new();
            return Enumerable.Range(startRange, endRange).OrderBy(x => random.Next()).ToArray();
        }

        private static int[] GetRandomParentsOrder(int range)
        {
            Random random = new();

            int[] parentChoice = new int[range];

            for (int i = 0; i < range; i++)
                parentChoice[i] = random.Next(2);

            return parentChoice;
        }
    }
}
