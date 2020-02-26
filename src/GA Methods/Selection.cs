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
            int poolSize = matingPool.selectionPool.Count -1;

            Random prng = new Random();

            int parentAIndex = prng.Next(0, poolSize);
            int parentBIndex = prng.Next(0, poolSize);

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
            int parentBIndex = prng.Next(0, populationSize);

            return new matingPartners(population[parentAIndex], population[parentBIndex]);

        }







    }
}
