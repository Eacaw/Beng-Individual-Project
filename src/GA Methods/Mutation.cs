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
            if (mutationChance > mutationValue) // mutation percentage of 2 = 0.98 (1 - (2/100))
            {
                int breakawayPointOne = prng.Next(1, childAgent.agentPath.getNodeCount() - 20 ); // Ensure that both starting and target nodes are never the breakaway points also not the end of the path
                int breakawayPointTwo = breakawayPointOne;
                while (breakawayPointTwo == breakawayPointOne && breakawayPointTwo - breakawayPointOne < 5) // Ensure the two indecies are different and at least 5 apart - arbitrarily chosen value
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
                if (findNewPath > 25)
                {
                    mutationTargetNode = childAgent.targetNode;
                    newpathEnding = 1;
                }


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

                //if (mutationPathCost > oldPathwayCost)
                //{
                //    //Console.WriteLine("Not Replaced");
                //    return childAgent;
                //}
                //else
                //{
                    //Console.WriteLine("break points: \t\t\t" + breakawayPointOne + "  " + breakawayPointTwo);
                    //Console.WriteLine("break points diff:\t\t" + (breakawayPointTwo - breakawayPointOne));
                    //Console.WriteLine("mut path nodes: \t\t" + newMutationPath.getNodeCount());

                    //int sizeDiff = (breakawayPointTwo - breakawayPointOne) - newMutationPath.getNodeCount();
                    //int nodeDiff = childAgent.agentPath.getNodeCount();

                    //Console.WriteLine("Child length before: \t\t" + childAgent.agentPath.getNodeCount());
                    childAgent.agentPath.splicePathSections(newMutationPath, newpathEnding);
                    //Console.WriteLine("Child length after: \t\t" + childAgent.agentPath.getNodeCount());

                    //nodeDiff -= childAgent.agentPath.getNodeCount();

                    //Console.WriteLine("Expected Diff: \t\t\t" + sizeDiff);
                    //Console.WriteLine("Child node diff: \t\t" + nodeDiff);

                //}
            }

            return childAgent;
        }


    }
}
