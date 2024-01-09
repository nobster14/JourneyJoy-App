using JourneyJoy.Algorithm.Algorithms.GeneticOperators;
using JourneyJoy.Algorithm.Models;
using JourneyJoy.Model.DTOs;
using JourneyJoy.UnitTests.AlgorithmTests.Helpers;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.UnitTests.AlgorithmTests.GeneticOpertorsTests
{
    public class MutationTest
    {
        private AlgorithmInformation information;
        private Genome parent;
        private static int Size => 20;

        [SetUp]
        public void Setup()
        {
            information = SetUpHelper.prepareAlgorithmInfo(Size);
            parent = new Genome(information, 0.1f);
        }

        [Test]
        public void CheckIfnMutationWorksProperly()
        {
            var child = Mutation.Execute(parent);

            child.Should().NotBeNull();

            for (int i = 0; i < child.NumberOfAttractions; i++)
            {
                if (i != child.StartPoint)
                {
                    int indexOfDay = child.GetDayIndexOfAttraction(i);
                    if (indexOfDay == -1)
                        child.MissedAttractions.Should().Contain(i);
                    else
                        child.DayOrder[indexOfDay].Should().Contain(i);
                }
            }

        }

        [Test]
        public void CheckIfOneAttractionMutationWorksProperly()
        {
            MethodInfo oneAttrMutationMethod = typeof(Mutation).GetMethod("ExecuteAttractionMutation", BindingFlags.NonPublic | BindingFlags.Static);

            if (oneAttrMutationMethod != null)
            {
                var child = (Genome)oneAttrMutationMethod.Invoke(null, new object[] { parent });

                child.Should().NotBeNull();

                for (int i = 0; i < child.NumberOfAttractions; i++)
                {
                    if (i != child.StartPoint)
                    {
                        int indexOfDay = child.GetDayIndexOfAttraction(i);
                        if (indexOfDay == -1)
                            child.MissedAttractions.Should().Contain(i);
                        else
                            child.DayOrder[indexOfDay].Should().Contain(i);
                    }
                }
            }
        }

        [Test]
        public void CheckIfTwoAttractionsMutationWorksProperly()
        {
            MethodInfo twoAttrMutationMethod = typeof(Mutation).GetMethod("ExecuteTwoAttractionsMutation", BindingFlags.NonPublic | BindingFlags.Static);

            if (twoAttrMutationMethod != null)
            {
                var child = (Genome)twoAttrMutationMethod.Invoke(null, new object[] { parent });

                child.Should().NotBeNull();

                for (int i = 0; i < child.NumberOfAttractions; i++)
                {
                    if (i != child.StartPoint)
                    {
                        int indexOfDay = child.GetDayIndexOfAttraction(i);
                        if (indexOfDay == -1)
                            child.MissedAttractions.Should().Contain(i);
                        else
                            child.DayOrder[indexOfDay].Should().Contain(i);
                    }
                }

            }
        }

        [Test]
        public void CheckIfChooseAttractionToMutationWorks()
        {
            MethodInfo chooseAttrMethod = typeof(Mutation).GetMethod("ChooseAttractionToMutation", BindingFlags.NonPublic | BindingFlags.Static);

            if (chooseAttrMethod != null)
            {
                var attrToMutate = (int)chooseAttrMethod.Invoke(null, new object[] { parent });
                attrToMutate.Should().NotBe(parent.StartPoint);
            }
        }

        [Test]
        public void CheckIfChooseTwoAttractionsToMutationWorks()
        {
            MethodInfo chooseAttrMethod = typeof(Mutation).GetMethod("ChooseTwoAttractionsToMutation", BindingFlags.NonPublic | BindingFlags.Static);

            if (chooseAttrMethod != null)
            {
                (var attrToMutate1, var attrToMutate2) = ((int, int))chooseAttrMethod.Invoke(null, new object[] { parent });
                attrToMutate1.Should().NotBe(parent.StartPoint);
                attrToMutate2.Should().NotBe(parent.StartPoint);
                attrToMutate1.Should().NotBe(attrToMutate2);
            }
        }
    }
}
