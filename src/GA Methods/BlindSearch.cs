using System;
using System.Collections.Generic;
using BEng_Individual_Project.src;
using BEng_Individual_Project.GA_Methods;
using BEng_Individual_Project.src.Utilities;

namespace BEng_Individual_Project.src.GAMethods
{
    public static class BlindSearch
    {
        /**
         * Overload Method that accepts an Agent as parameter
         * Used only when performing the initial blind search
         */
         public static Path performBlindSearch(Agent agentInstance)
        {
            return performBlindSearch(agentInstance.startingNode, agentInstance.targetNode);
        }


        /**
         * Perform full blind search algorithm to generate a path
         * used both for initial population and mutation
         */
        public static Path performBlindSearch(DataNode startingNode, DataNode targetNode)
        {

            DataNode currentNode = startingNode;

            Path blindSearchPath = new Path();

            blindSearchPath.addNodeToPath(startingNode);

            float pathCost = 0;

            bool endOfPathReached = false;

            while (!endOfPathReached)
            {
                // Find the neighbour heuristically closest to the target
                int preferredNodeNeighbour = numericalUtilities.getPreferredNeighbour(currentNode, targetNode);

                if (preferredNodeNeighbour == -1)
                {
                    preferredNodeNeighbour = 0;
                }


                currentNode = blindSearchStep(currentNode, preferredNodeNeighbour, blindSearchPath);

                //TODO: Clean this the fuck up
                if (currentNode == targetNode) // Target reached
                {
                    endOfPathReached = true;
                    blindSearchPath.addNodeToPath(currentNode);
                    return blindSearchPath; // Writes Path as a white line
                }
                else if (currentNode.heightValue < 0) // Path can no longer continue
                {
                    endOfPathReached = true;
                    return blindSearchPath; // Writes Path as a black line
                }
                else // End of path not yet reached
                {
                    blindSearchPath.addNodeToPath(currentNode);
                }
            }
            // Failsafe statements - Never Reached in testing
            Console.WriteLine("Path Incomplete");
            return blindSearchPath;

        }



        /**
         * Method to move the agent forward one step to a suitable neighbour node
         */
        public static DataNode blindSearchStep(DataNode stepStartNode, int pref, Path workingPath)
        {
            // Initialise new list to store all potential neighbours
            List<DataNode> potentialneighbours;

            // Pre-determine preferred Node
            DataNode preferredNode = stepStartNode.neighbourNodes[pref];

            // Check for possible neighbours by selecting all possibilities that aren't obstacle Nodes
            potentialneighbours = checkForObstacles(stepStartNode, workingPath);

            // Remove any nodes that will trap the agent in one location
            potentialneighbours = removeEntrappingNodes(potentialneighbours, workingPath);

            // Actions to add randomness to the potential neighbours list
            potentialneighbours.Reverse(); // Built in List method

            // Add in the chance for the algorithm to pick the neighbour that would bring it closest to the target node
            if (potentialneighbours.Contains(preferredNode))
            {
                Random prng = new Random();
                double chance = prng.NextDouble();
                if (chance > 0.8) // Chance to select preferred neighbour - DEFAULT: 25% (Value set to 0.75)
                {
                    return preferredNode;
                }
            }

            // If preferred neighbour wasn't selected, pick a random neighbour from potential list
            if (potentialneighbours.Count != 0)
            {
                Random prng = new Random();
                double selectedNeighbour = prng.NextDouble();

                selectedNeighbour *= potentialneighbours.Count - 1;

                selectedNeighbour = Math.Floor(selectedNeighbour);

                int selectedNeighbourIndex = (int)selectedNeighbour;

                return potentialneighbours[selectedNeighbourIndex];
            }


            // Agent reaches the end of the path and no further options are available
            // Return end of path node
            DataNode endOfPath = new DataNode(-3, -3, -3);
            return endOfPath;

        }

        /**
        * Check if any neighbours are obstacles
        * add any neighbours that aren't to potential neighbours array
        * obstacles defined as having a height value as less than 0
        * -1 = edgeNode
        * -2 = Obstacle
        */
        private static List<DataNode> checkForObstacles(DataNode currentNode, Path workingPath)
        {
            List<DataNode> potentialNeighbours = new List<DataNode>();

            for (int i = 0; i < currentNode.neighbourNodes.Length - 1; i++)
            {
                // Remove any null references for neighbours of edge nodes
                if (currentNode.neighbourNodes[i] == null ||
                   workingPath.checkForExistingNode(currentNode.neighbourNodes[i]) ||
                                        currentNode.neighbourNodes[i].heightValue < 0)
                {
                    continue;
                }
                else
                {
                    potentialNeighbours.Add(currentNode.neighbourNodes[i]);
                }

            }
            return potentialNeighbours;

        }


        /**
         * Remove chance for entrapment
         */
        private static List<DataNode> removeEntrappingNodes(List<DataNode> inputNodes, Path workingPath)
        {
            for (int i = 0; i < inputNodes.Count; i++)
            {
                // Build a new potential neighbour list from the node
                // that is being checked's neighbours. An empty list will mean
                // the node would cause entrappment
                List<DataNode> entrappmentCheckNeighbours = checkForObstacles(inputNodes[i], workingPath);
                if (entrappmentCheckNeighbours.Count == 0)
                {
                    inputNodes.Remove(inputNodes[i]);
                }
            }

            return inputNodes;
        }


    }
}
