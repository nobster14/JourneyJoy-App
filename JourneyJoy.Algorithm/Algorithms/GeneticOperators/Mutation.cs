using JourneyJoy.Algorithm.Models;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Algorithms.GeneticOperators
{
    public static class Mutation
    {
        #region Methods
        /// <summary>
        /// Executes mutation process. Chooses type of mutation - each with probability of 0.5.
        /// </summary>
        /// <param name="genome"></param>
        /// <returns></returns>
        public static Genome Execute(Genome genome)
        {
            var rand = new Random();
            
             if (rand.NextDouble() < 0.5f)
                return ExecuteAttractionMutation(genome);
            else
                return ExecuteTwoAttractionsMutation(genome);

        }
        /// <summary>
        /// Executes mutation of one attraction - it can be moved or removed.
        /// </summary>
        /// <param name="genome"></param>
        /// <returns></returns>
        private static Genome ExecuteAttractionMutation(Genome genome)
        {
            var newGenome = new Genome(genome);

            var attractionToMutate = ChooseAttractionToMutation(newGenome);

            var random = new Random();

            var newDay = random.Next(newGenome.NumberOfDays + 1);

            for(int i = 0; i < newGenome.NumberOfDays; i++)
            {
                if (newGenome.DayChoiceMatrix[attractionToMutate, i] == true)
                {
                    newGenome.DayChoiceMatrix[attractionToMutate, i] = false;
                    newGenome.DayOrder[i].Remove(attractionToMutate);
                    newGenome.MissedAttractions.Add(attractionToMutate);

                    break;
                }
            }

            if (newDay != newGenome.NumberOfDays)
            {
                newGenome.DayChoiceMatrix[attractionToMutate, newDay] = true;
                var placeInDay = random.Next(newGenome.DayOrder[newDay].Count);
                newGenome.DayOrder[newDay].Insert(placeInDay, attractionToMutate);
                newGenome.MissedAttractions.Remove(attractionToMutate);
            }

            return newGenome;
        }

        /// <summary>
        /// Executes mutation of two attractions - they will switch places.
        /// </summary>
        /// <param name="genome"></param>
        /// <returns></returns>
        private static Genome ExecuteTwoAttractionsMutation(Genome genome)
        {
            (var attraction1, var attraction2) = ChooseTwoAttractionsToMutation(genome);

            var dayAttr1 = genome.GetDayIndexOfAttraction(attraction1);
            var dayAttr2 = genome.GetDayIndexOfAttraction(attraction2);
            
            var newGenome = new Genome(genome);
            
            if(dayAttr1 != -1 && dayAttr2 != -1)
            {
                SwitchAttractionsWhenExist(ref newGenome, attraction1, attraction2, dayAttr1, dayAttr2);
            }
            else if (dayAttr1 != -1 && dayAttr2 == -1)
            {
                SwitchAttractionsWhenDontExists(ref newGenome, attraction1, attraction2, dayAttr1);
            }
            else if(dayAttr1 == -1 && dayAttr2 != -1)
            {
                SwitchAttractionsWhenDontExists(ref newGenome, attraction2, attraction1, dayAttr2);
            }

            return newGenome;
        }
        
        /// <summary>
        /// Switches places for two attractions when one is missed.
        /// </summary>
        /// <param name="genome"></param>
        /// <param name="attrExists"></param>
        /// <param name="attrNotExists"></param>
        /// <param name="dayAttr"></param>
        private static void SwitchAttractionsWhenDontExists(ref Genome genome, int attrExists, int attrNotExists, int dayAttr)
        {
            var attrPlace = genome.DayOrder[dayAttr].FindIndex(x => x == attrExists);

            genome.DayChoiceMatrix[attrExists, dayAttr] = false;
            genome.DayChoiceMatrix[attrNotExists, dayAttr] = true;

            genome.DayOrder[dayAttr][attrPlace] = attrNotExists;

            genome.MissedAttractions.Remove(attrNotExists);
            genome.MissedAttractions.Add(attrExists);
        }

        /// <summary>
        /// Switches places of two attractions when neither of them is missed.
        /// </summary>
        /// <param name="genome"></param>
        /// <param name="attraction1"></param>
        /// <param name="attraction2"></param>
        /// <param name="dayAttr1"></param>
        /// <param name="dayAttr2"></param>
        private static void SwitchAttractionsWhenExist(ref Genome genome, int attraction1, int attraction2, int dayAttr1, int dayAttr2)
        {
            var attrPlace1 = genome.DayOrder[dayAttr1].FindIndex(x => x == attraction1);
            var attrPlace2 = genome.DayOrder[dayAttr2].FindIndex(x => x == attraction2);

            genome.DayChoiceMatrix[attraction1, dayAttr1] = genome.DayChoiceMatrix[attraction2, dayAttr2] = false;
            genome.DayChoiceMatrix[attraction1, dayAttr2] = genome.DayChoiceMatrix[attraction2, dayAttr1] = true;

            genome.DayOrder[dayAttr1][attrPlace1] = attraction2;
            genome.DayOrder[dayAttr2][attrPlace2] = attraction1;
        }

        /// <summary>
        /// Chooses attraction to mutate.
        /// </summary>
        /// <param name="genome"></param>
        /// <returns></returns>
        private static int ChooseAttractionToMutation(Genome genome)
        {
            var random = new Random();
            var attractionToMutate = random.Next(genome.NumberOfAttractions - 1);
            return attractionToMutate == genome.StartPoint ? genome.NumberOfAttractions - 1 : attractionToMutate;
        }

        /// <summary>
        /// Chooses two attractions to mutate.
        /// </summary>
        /// <param name="genome"></param>
        /// <returns></returns>
        private static (int attraction1, int attraction2) ChooseTwoAttractionsToMutation(Genome genome)
        {
            var attraction1 = ChooseAttractionToMutation(genome);
            var random = new Random();
            var attraction2 = random.Next(genome.NumberOfAttractions - 2);

            attraction2 = attraction2 == genome.StartPoint ? genome.NumberOfAttractions - 1 : attraction2;
            attraction2 = attraction2 == attraction1 ? genome.NumberOfAttractions - 2 : attraction2;

            return (attraction1, attraction2);
        }
        #endregion
    }

}
