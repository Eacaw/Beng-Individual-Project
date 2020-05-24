using System;
using System.Collections.Generic;

namespace BEng_Individual_Project.src.GAMethods
{
    public static class Selection
    {

        /**
         * From given mating pool, select two partners at random
         * used when a mating pool has already been formed
         */
        public static matingPartners randomSelectionFromPool(matingPool matingPool)
        {
            int poolSize = matingPool.selectionPool.Count;

            Random prng = new Random();

            int parentAIndex = prng.Next(0, poolSize);
            int parentBIndex = parentAIndex;

            while (parentBIndex == parentAIndex)
            {
                parentBIndex = prng.Next(0, poolSize);
            }

            return new matingPartners(matingPool.selectionPool[parentAIndex], matingPool.selectionPool[parentBIndex]);
        }

        /**
         * From given population, select two partners at random
         * method used where no favouritism is in place
         */
        public static matingPartners randomSelectionFromPopulation(List<Agent> population)
        {
            int populationSize = population.Count;

            Random prng = new Random();

            int parentAIndex = prng.Next(0, populationSize);
            int parentBIndex = parentAIndex;

            while (parentBIndex == parentAIndex)
            {
                parentBIndex = prng.Next(0, populationSize);
            }

            return new matingPartners(population[parentAIndex], population[parentBIndex]);

        }


        /**
         * 
         * 
         */
        public static matingPartners tournamentSelectionPool(matingPool matingPool, int firstRoundSize)
        {

            Random prng = new Random();

            List<Agent> SelectedParents = new List<Agent>();

            List<int> FirstRoundSelection = new List<int>();

            bool parentsSelected = false;

            // Repeat tournament selection to select two different parents

            while (!parentsSelected)
            {
                // Select the indices of the first round of parents from the pool
                // Ensuring unique indices for variation in the population
                while (FirstRoundSelection.Count < firstRoundSize)
                {
                    int parentIndexFromPool = prng.Next(0, matingPool.selectionPool.Count - 1);

                    if (!FirstRoundSelection.Contains(parentIndexFromPool))
                    {
                        FirstRoundSelection.Add(parentIndexFromPool);
                    }
                }

                // Index of winner from pool
                int tournamentWinner = 0;

                float prevFitness = 0;

                for (int i = 0; i < FirstRoundSelection.Count - 1; i++)
                {

                    float nextFitness = matingPool.selectionPool[FirstRoundSelection[i]].fitnessScore;

                    if (nextFitness > prevFitness)
                    {
                        tournamentWinner = FirstRoundSelection[i];
                        //Console.WriteLine("i = " + i + " next: " + nextFitness + " prev: " + prevFitness + " index: " + tournamentWinner);
                    }

                    prevFitness = matingPool.selectionPool[tournamentWinner].fitnessScore;
                }

                if (SelectedParents.Count < 2)
                {
                        SelectedParents.Add(matingPool.selectionPool[tournamentWinner]);
                        if (SelectedParents.Count == 2)
                        {
                            parentsSelected = true;
                        }
                }

                
            }

            matingPartners parents = new matingPartners(SelectedParents[0], SelectedParents[1]);

            return parents;
        }







    }
}
