using JourneyJoy.Algorithm.Algorithms;
using JourneyJoy.Algorithm.Algorithms.GeneticOperators;
using JourneyJoy.Algorithm.Models;
using JourneyJoy.Model.DTOs;
using JourneyJoy.UnitTests.AlgorithmTests.Helpers;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.UnitTests.AlgorithmTests.GeneticOpertorsTests
{
    public class CrossingTest
    {
        private AlgorithmInformation information;
        private AlgorithmInformation simpleInformation;
        private Genome Parent1;
        private Genome Parent2;
        private static int Size => 20;

        [SetUp]
        public void Setup()
        {
            information = SetUpHelper.prepareAlgorithmInfo(Size);
            simpleInformation = SetUpHelper.prepareSimpleAlgorithmInfo(5);

            Parent1 = new Genome(information, 0.2f);
            Parent2 = new Genome(information, 0.2f);
        }

        [Test]
        public void CheckIfCrossingWorks()
        {
            (var child1, var child2) = Crossing.Execute(Parent1, Parent2);

            var sumMissedAttractions = child1.MissedAttractions.Count + child2.MissedAttractions.Count;

            child1.Should().NotBeNull();
            child2.Should().NotBeNull();

            sumMissedAttractions.Should().Be(Parent1.MissedAttractions.Count + Parent2.MissedAttractions.Count);
        }

        [Test]
        public void CheckIfChildGeneratesProperly()
        {
            MethodInfo getRandomParentsMethod = typeof(Crossing).GetMethod("GetRandomParentsOrder", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo createChildMethod = typeof(Crossing).GetMethod("CreateChildFromParents", BindingFlags.NonPublic | BindingFlags.Static);

            if (getRandomParentsMethod != null && createChildMethod != null)
            {
                var randomParentsOrder = (int[])getRandomParentsMethod.Invoke(null, new object[] { Parent1.NumberOfAttractions });
                var child = (Genome)createChildMethod.Invoke(null, new object[] { Parent1, Parent2, randomParentsOrder });

                child.Should().NotBeNull();

                for (int i = 0; i < child.NumberOfAttractions; i++)
                {
                    if (randomParentsOrder[i] == 0)
                        child.GetDayIndexOfAttraction(i).Should().Be(Parent1.GetDayIndexOfAttraction(i));
                    else
                        child.GetDayIndexOfAttraction(i).Should().Be(Parent2.GetDayIndexOfAttraction(i));

                }

            }
        }

        [Test]
        public void CheckIfCopyDayChoiceWorksProperly()
        {
            MethodInfo getRandomParentsMethod = typeof(Crossing).GetMethod("GetRandomParentsOrder", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo copyDayMethod = typeof(Crossing).GetMethod("CopyDayChoiceFromParents", BindingFlags.NonPublic | BindingFlags.Static);

            if (getRandomParentsMethod != null && copyDayMethod != null)
            {
                var randomParentsOrder = (int[])getRandomParentsMethod.Invoke(null, new object[] { Parent1.NumberOfAttractions });
                var child = (Genome)copyDayMethod.Invoke(null, new object[] { Parent1, Parent2, randomParentsOrder });

                child.Should().NotBeNull();

                for (int i = 0; i < child.NumberOfAttractions; i++)
                {
                    if (randomParentsOrder[i] == 0)
                        child.GetDayIndexOfAttraction(i).Should().Be(Parent1.GetDayIndexOfAttraction(i));
                    else
                        child.GetDayIndexOfAttraction(i).Should().Be(Parent2.GetDayIndexOfAttraction(i));

                }

            }
        }

        [Test]
        public void CheckIfCopyDayOrderWorksProperly()
        {
            MethodInfo copyDayMethod = typeof(Crossing).GetMethod("CopyDayOrder", BindingFlags.NonPublic | BindingFlags.Static);

            var parent1 = new Genome(simpleInformation.NumberOfAttractions, simpleInformation.NumberOfDays);
            var parent2 = new Genome(simpleInformation.NumberOfAttractions, simpleInformation.NumberOfDays);
            var child = new Genome(simpleInformation.NumberOfAttractions, simpleInformation.NumberOfDays);

            parent1.DayOrder[0] = new List<int> { 1, 2 };
            parent2.DayOrder[0] = new List<int> { 4, 3 };
            child.DayOrder[0] = new List<int> { 2, 1, 3, 4 };

            if (copyDayMethod != null)
            {
                var randomParentsOrder = new int[] { 0, 0, 0, 1, 1 };
                copyDayMethod.Invoke(null, new object[] { child, 0, randomParentsOrder, parent1, parent2 });

                int iterator1 = 0;
                int iterator2 = 0;

                foreach (var attraction in child.DayOrder[0])
                {
                    if (randomParentsOrder[attraction] == 0)
                    {
                        parent1.DayOrder[0][iterator1].Should().Be(attraction);
                        iterator1++;
                    }
                    else
                    {
                        parent2.DayOrder[0][iterator2].Should().Be(attraction);
                        iterator2++;
                    }
                }
            }
        }

        [Test]
        public void CheckIfAttractionAddsFromParentProperly()
        {
            MethodInfo copyAttractionMethod = typeof(Crossing).GetMethod("AddAttractionFromParent", BindingFlags.NonPublic | BindingFlags.Static);

            var parent = new Genome(simpleInformation.NumberOfAttractions, simpleInformation.NumberOfDays);
            var child = new Genome(simpleInformation.NumberOfAttractions, simpleInformation.NumberOfDays);

            parent.DayOrder[0] = new List<int> { 1, 2 };
            child.DayOrder[0] = new List<int>();

            var selectedAttractions = new List<int> { 4, 2, 3 };

            if (copyAttractionMethod != null)
            {
                int iterator = 0;
                copyAttractionMethod.Invoke(null, new object[] { child.DayOrder[0], parent.DayOrder[0], selectedAttractions, iterator });

                child.DayOrder[0].Should().Contain(2);
            }
        }
    }
}
