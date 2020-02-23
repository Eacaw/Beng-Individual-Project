using System;
using System.Collections.Generic;
using System.Text;
using BEng_Individual_Project.src;
using BEng_Individual_Project.src.Utilities;

namespace BEng_Individual_Project.GA_Methods
{
    static class Fitness
    {

        /**
         * calculate the weighted fitness for the agent
         * ready for sorting and selection
         */
        public static float calculateWeightedFitness(Agent agent, int pathWeight, int riskWeight, float maxPathLength, float minPathLength)
        {
            float Fitness = 0;

            Fitness += getFitnessFromPathLength(agent, maxPathLength, minPathLength) * pathWeight;
            Fitness += getFitnessFromRiskValue(agent) * riskWeight;


            return Fitness;
        }

        private static float getFitnessFromPathLength(Agent agent, float maxPathLength, float minPathLength)
        {
            // Invert the cost so lower cost = higher fitness
            float invertPathLength = maxPathLength - agent.pathCost;

            // Map the path length between 0 and 255
            float fitnessFromPath = numericalUtilities.mapValue(agent.pathCost, minPathLength, maxPathLength, 0, 255);

            return 255 - fitnessFromPath;

        }

        private static float getFitnessFromRiskValue(Agent agent)
        {
            float fitnessFromRisk = 0;




            return fitnessFromRisk;
        }




    }
}
