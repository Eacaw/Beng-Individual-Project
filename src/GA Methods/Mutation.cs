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

            // Check if mutation chance is within range
            if (mutationChance > (1 - mutationPercentage / 100)) // mutation percentage of 2 = 0.98 (1 - (2/100))
            {
                int breakawayPointOne = prng.Next(1, childAgent.agentPath.getNodeCount() - 2); // Ensure that both starting and target nodes are never the breakaway points
                int breakawayPointTwo = breakawayPointOne;
                while (breakawayPointTwo == breakawayPointOne) // Ensure the two indecies are different
                {
                    breakawayPointTwo = prng.Next(1, childAgent.agentPath.getNodeCount() - 2);
                }

                Console.WriteLine("One: " + breakawayPointOne + " Two: " + breakawayPointTwo);

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

                float oldPathwayCost = childAgent.agentPath.getSectionCost(mutationStartNode, mutationTargetNode);

                float distanceFromMutationTarget = numericalUtilities.getDistanceBetweenNodes(mutationStartNode, mutationTargetNode);

                // Perform the blind search until the mutation has reached it's target
                while(distanceFromMutationTarget > 0)
                {
                    newMutationPath = src.GAMethods.BlindSearch.performBlindSearch(mutationStartNode, mutationTargetNode);
                    distanceFromMutationTarget = numericalUtilities.getDistanceBetweenNodes(newMutationPath.getFinalNode(), mutationTargetNode);
                }

                // Check to see if the new path is shorter or longer than the old path, discard mutation if no improvement made
                float mutationPathCost = newMutationPath.getPathCost();

                if (mutationPathCost > oldPathwayCost)
                {
                    Console.WriteLine("Not Replaced");
                    return childAgent;
                }
                else
                {
                    Console.WriteLine("Replaced");
                    childAgent.agentPath.splicePathSections(newMutationPath);
                }
            }

            return childAgent;
        }


    }
}
