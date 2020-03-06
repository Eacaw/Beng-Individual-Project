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
        public static float calculateWeightedFitness(Agent agent, int pathWeight, int riskWeight, float maxPathLength, float minPathLength, float maxDistance)
        {
            float Fitness = 0;
            float maxRisk= 0, minRisk = 0;

            Fitness += getFitnessFromPathLength(agent, maxPathLength, minPathLength) * pathWeight;
            Fitness += getFitnessFromDistanceToTarget(agent, maxDistance);
            //Fitness += getFitnessFromRiskValue(agent, minRisk, maxRisk) * riskWeight;

            return Fitness;
        }

        /**
         * Map the path length to a value between 0 and 100
         * to favour lower path lengths
         */
        private static float getFitnessFromPathLength(Agent agent, float maxPathLength, float minPathLength)
        {
            return numericalUtilities.mapValue(agent.pathCost, minPathLength, maxPathLength, 100, 0);
        }

        /**
         * Map the distance to target to a value between 0 and 100
         * favouring shorter remaining distances
         */
        private static float getFitnessFromDistanceToTarget(Agent agent, float maxDistance)
        {
            // Multiply the distance from target by 10 in order to give more preference to
            // those that reached the target, ensuring they are more likely to procreate
            return numericalUtilities.mapValue(agent.distanceFromTarget * 10, 0, maxDistance, 100, 0);
        }


        /**
         * Map the risk value to a value between 0 and 100
         * to favour lower risk values
         */
        private static float getFitnessFromRiskValue(Agent agent, float minRiskValue, float maxRiskValue)
        {
            // Map the path length between 0 and 255
            return numericalUtilities.mapValue(agent.riskValue, minRiskValue, maxRiskValue, 100, 0);
        }




    }
}
