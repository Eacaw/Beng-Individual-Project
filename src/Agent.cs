using System;
using System.Collections.Generic;
using System.Text;
using BEng_Individual_Project.lib;
using BEng_Individual_Project.src.Utilities;

namespace BEng_Individual_Project.src
{
    public class Agent
    {
        public Path agentPath { get; set; }

        public float pathCost { get; set; }
        public bool hitTarget { get; set; }
        public float riskValue { get; set; }
        public float distanceFromTarget { get; set; }

        DataNode startingNode, targetNode, currentNode;

        // GA variables
        public float fitnessScore { get; set; }



        public Agent(DataNode startingNode, DataNode targetNode, DataNode edgeNode)
        {
            this.agentPath = new Path();
            this.startingNode = startingNode;
            this.targetNode = targetNode;
            this.currentNode = startingNode;

            // Add the edgeNode to the pathway at index 0;
            this.agentPath.addNodeToPath(edgeNode);
            // Add the starting Node to pathway at index 1;
            this.agentPath.addNodeToPath(currentNode);

            this.hitTarget = false;

        }



        /**
         * Perform full blind search algorithm to generate
         * initial population of agents
         */
        public int performBlindSearch()
        {

            this.currentNode = this.startingNode;

            bool endOfPathReached = false;

            while (!endOfPathReached)
            {
                // Find the neighbour heuristically closest to the target
                int preferredNodeNeighbour = numericalUtilities.getPreferredNeighbour(this.currentNode, this.targetNode);

                if (preferredNodeNeighbour == -1)
                {
                    preferredNodeNeighbour = 0;
                }


                this.currentNode = blindSearchStep(this.currentNode, preferredNodeNeighbour);

                //TODO: Clean this the fuck up
                if (this.currentNode == this.targetNode) // Target reached
                {
                    endOfPathReached = true;
                    this.agentPath.addNodeToPath(this.currentNode);
                    this.pathCost = this.agentPath.getPathCost();
                    this.distanceFromTarget = numericalUtilities.getDistanceBetweenNodes(this.agentPath.getFinalNode(), this.targetNode);
                    return 255; // Writes Path as a white line
                }
                else if (this.currentNode.heightValue < 0) // Path can no longer continue
                {
                    endOfPathReached = true;
                    this.pathCost = this.agentPath.getPathCost();
                    this.distanceFromTarget = numericalUtilities.getDistanceBetweenNodes(this.agentPath.getFinalNode(), this.targetNode);
                    return 0; // Writes Path as a black line
                }
                else // End of path not yet reached
                {
                    this.agentPath.addNodeToPath(this.currentNode);
                }
            }
            // Failsafe statements - Never Reached in testing
            this.pathCost = this.agentPath.getPathCost();
            Console.WriteLine("Path Incomplete");
            return 0;

        }



        /**
         * Method to move the agent forward one step to a suitable neighbour node
         */
        public DataNode blindSearchStep(DataNode stepStartNode, int pref)
        {
            // Initialise new list to store all potential neighbours
            List<DataNode> potentialneighbours;

            // Pre-determine preferred Node
            DataNode preferredNode = stepStartNode.neighbourNodes[pref];

            // Check for possible neighbours by selecting all possibilities that aren't obstacle Nodes
            potentialneighbours = checkForObstacles(stepStartNode);

            // Remove all nodes from the list that are already in the agent's path
            //potentialneighbours = removeExistingNodes(potentialneighbours); ---- // STUART - This method has been removed as deemed unecessary

            // Remove any nodes that will trap the agent in one location
           potentialneighbours = removeEntrappingNodes(potentialneighbours);


            // Actions to add randomness to the potential neighbours list
            potentialneighbours.Reverse(); // Built in List method
            //potentialneighbours = ListShuffle.Shuffle(potentialneighbours); // Custom list shuffler -- NB -- BREAKS EVERYTHING


            // Add in the chance for the algorithm to pick the neighbour that would bring it closest to the target node
            if (potentialneighbours.Contains(preferredNode))
            {
                Random prng = new Random();
                double chance = prng.NextDouble();
                if (chance > 0.75) // Chance to select preferred neighbour - 50%
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
        private List<DataNode> checkForObstacles(DataNode currentNode)
        {
            List<DataNode> potentialNeighbours = new List<DataNode>();

            for (int i = 0; i < currentNode.neighbourNodes.Length - 1; i++)
            {
                // Remove any null references for neighbours of edge nodes
                if (currentNode.neighbourNodes[i] == null ||
                   this.agentPath.checkForExistingNode(currentNode.neighbourNodes[i]) ||  //---- STUART - This line doesn't add the nodes that already exist in the path - Faster Method but paths are always shorter
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
         * Check if the potential neighbours already exits in the path
         * and removes them if they do
         * 
         * Slower method, but allows the paths to be much longer
         */
        private List<DataNode> removeExistingNodes(List<DataNode> inputNodes)

        {

            for (int i = 0; i < inputNodes.Count - 1; i++)
            {
                if (this.agentPath.checkForExistingNode(inputNodes[i]))
                {
                    inputNodes.Remove(inputNodes[i]);
                    //Console.WriteLine("Remove");
                }
            }
            return inputNodes;
        }

        /**
         * Remove chance for entrapment
         */
        private List<DataNode> removeEntrappingNodes(List<DataNode> inputNodes)
        {
            for (int i = 0; i < inputNodes.Count; i++)
            {
                // Build a new potential neighbour list from the node
                // that is being checked's neighbours. An empty list will mean
                // the node would cause entrappment
                List<DataNode> entrappmentCheckNeighbours = checkForObstacles(inputNodes[i]);
                //entrappmentCheckNeighbours = removeExistingNodes(entrappmentCheckNeighbours); ---- // STUART - This method has been removed as deemed unecessary
                if (entrappmentCheckNeighbours.Count == 0)
                {
                    inputNodes.Remove(inputNodes[i]);
                }
            }

            return inputNodes;
        }



    }
}
