using System;
using System.Collections.Generic;
namespace BEng_Individual_Project.src.GAMethods
{
    public class matingPool
    {

        public List<Agent> selectionPool { get; set; }

        public matingPool()
        {
            this.selectionPool = new List<Agent>();
        }

        /**
         * Roulette Selection Methdod
         * Randomly select a mating partner with the probability
         * of being selected proportional to the fitness of the agent
         */
        public List<Agent> linearRouletteSelectionFitness(List<Agent> population)
        {

            List<Agent> matingPool = new List<Agent>();

            //Iterate for each agent
            for (int agentCount = 0; agentCount < population.Count; agentCount++)
            {
                // Iterate based on the fitness of the current agent
                // Adds a quantity equal to the agent's fitness to the mating pool
                for (int agentFitness = 0; agentFitness < population[agentCount].fitnessScore; agentFitness++)
                {
                    this.selectionPool.Add(population[agentCount]);
                }
            }
            return this.selectionPool;
        }

        /**
         * Roulette Selection Methdod
         * Randomly select a mating partner with the probability
         * of being selected proportional to the fitness of the agent
         * Pool filled with number of agents equal to their rank in the total population
         */
        public List<Agent> linearRouletteSelectionRank(List<Agent> population)
        {

            List<Agent> matingPool = new List<Agent>();

            //Iterate for each agent
            for (int agentCount = 0; agentCount < population.Count; agentCount++)
            {
                //Iterate based on the agent's rank in the population
                for (int agentRank = 0; agentRank < agentCount + 1; agentCount++)
                {
                    this.selectionPool.Add(population[agentCount]);
                }
            }

            Random prng = new Random();

            int parentAIndex = prng.Next(0, matingPool.Count);
            int parentBIndex = prng.Next(0, matingPool.Count);


            return this.selectionPool;
        }


        /**
         * Elitism filled mating pool
         * mating pool filled with the top given percentage from the population
         */
         public List<Agent> elitismSelection(List<Agent> population, int elitePercentage)
        {
            // Double check whether the population has been sorted in ascending
            // or descending orer and ensure the top agents are being selected for breeding
            int startingIndex = 0;
            int incrementalStep = 1;
            if(population[0].fitnessScore < population[population.Count - 1].fitnessScore)
            {
                startingIndex = population.Count - 1;
                incrementalStep = -1;
            }

            // Calculate how many agents should be in the pool
            int totalAgentCountForSelection = (int)Math.Floor((double)(population.Count / elitePercentage));

            // Incrementally add the top agents to the pool
            for (int i = 0; i < totalAgentCountForSelection + 1; i++)
            {
                int agentIndex = startingIndex + (i * incrementalStep);
                this.selectionPool.Add(population[agentIndex]);
            }

            return this.selectionPool;
        }




    }
}
