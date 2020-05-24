using BEng_Individual_Project.src;
using BEng_Individual_Project.src.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BEng_Individual_Project.GA_Methods
{
    public static class Mutation
    {

        public static Agent mutatePathWithoutLimit(Agent childAgent, int mutationPercentage)
        {
            Random prng = new Random();

            // Generate a random number to see if mutation should occur
            double mutationChance = prng.NextDouble();

            double mutationValue = 1 - ((double)mutationPercentage / 100);

            // Check if mutation chance is within range
            if (mutationChance > mutationValue && childAgent.agentPath.getNodeCount() > 5) // mutation percentage of 2 = 0.98 (1 - (2/100))
            {
                int breakawayPointOne = prng.Next(1, childAgent.agentPath.getNodeCount() - 2); // Ensure that both starting and target nodes are never the breakaway points also not the end of the path
                int breakawayPointTwo = breakawayPointOne;
                while (breakawayPointTwo == breakawayPointOne && breakawayPointTwo - breakawayPointOne < 2) // Ensure the two indecies are different and at least 5 apart - arbitrarily chosen value
                {
                    breakawayPointTwo = prng.Next(1, childAgent.agentPath.getNodeCount() - 2);
                }

                // Make sure breakaway point one is the lower of the two
                if (breakawayPointOne > breakawayPointTwo)
                {
                    int temp = breakawayPointOne;
                    breakawayPointOne = breakawayPointTwo;
                    breakawayPointTwo = temp;
                }

                // Create a new path for the mutation
                Path newMutationPath = new Path();
                // Create references to the target and start points
                DataNode mutationStartNode = childAgent.agentPath.getPathway()[breakawayPointOne];
                DataNode mutationTargetNode = childAgent.agentPath.getPathway()[breakawayPointTwo];


                // add a 25% chance that the mutation will mutate away from the original path and try to
                // reach the target node independantly of the original path.
                // method will significantly increase computation time but will allow a much higher diversity rate in the population
                int findNewPath = prng.Next(0, 100);
                int newpathEnding = 0;
                if (findNewPath > 50)
                {
                    mutationTargetNode = childAgent.targetNode;
                    newpathEnding = 1;
                }

                float distanceFromMutationTarget = numericalUtilities.getDistanceBetweenNodes(mutationStartNode, mutationTargetNode);

                // Perform the blind search until the mutation has reached it's target
                childAgent.findDistanceToTarget();
                float oldDistanceFromTarget = childAgent.distanceFromTarget;

                int attemptCount = 0;
                //for(int attempts = 0; attempts < 5000; attempts++) // Give the blind search 50 attempts to reach the target
                if (newpathEnding == 1)
                {
                    while (distanceFromMutationTarget > oldDistanceFromTarget && attemptCount < 5000)
                    {
                        newMutationPath = src.GAMethods.BlindSearch.performBlindSearch(mutationStartNode, mutationTargetNode, 1);
                        distanceFromMutationTarget = numericalUtilities.getDistanceBetweenNodes(newMutationPath.getFinalNode(), mutationTargetNode);
                        attemptCount++;
                    }
                } else
                {
                    while(distanceFromMutationTarget > 0 )//&& attemptCount < 10000)
                    {
                        newMutationPath = src.GAMethods.BlindSearch.performBlindSearch(mutationStartNode, mutationTargetNode, 1);
                        distanceFromMutationTarget = numericalUtilities.getDistanceBetweenNodes(newMutationPath.getFinalNode(), mutationTargetNode);
                        attemptCount++;
                    }
                    // = src.GAMethods.BlindSearch.performBlindSearch(mutationStartNode, mutationTargetNode, 1);
                }

                //if (distanceFromMutationTarget == 0)
                //{
                childAgent.agentPath.splicePathSections(newMutationPath, newpathEnding);
                //}
            }

            return childAgent;
        }


    }
}
