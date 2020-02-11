using System;
using System.Collections.Generic;
using System.Text;
using BEng_Individual_Project.lib;

namespace BEng_Individual_Project.src
{
    class Agent
    {
        public Path agentPath { get; set; }
        DataNode startingNode, targetNode, currentNode;

        // GA variables
        float fitnessScore { get; set; }

        

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

        }




        public int performBlindSearch()
        {

            bool endOfPathReached = false;

            while (!endOfPathReached)
            {
                currentNode = blindSearchStep(currentNode);

                if (currentNode == targetNode) {
                    Console.WriteLine("Target Reached");
                    endOfPathReached = true;
                    return 255;
                } else if ( currentNode.heightValue < -2)
                {
                    Console.WriteLine("Auto Kill");
                    endOfPathReached = true;
                    return 0;
                } else
                {
                    this.agentPath.addNodeToPath(currentNode);
                }

            }
            //Console.WriteLine("Path Complete");
            return 0;

        }



        /**
         * Method to move the agent forward one step to a suitable neighbour node
         */
        public DataNode blindSearchStep(DataNode startNode)
        {
            // Check for possible neighbours by selecting all possibilities
            // that aren't obstacle Nodes
            List<DataNode> potentialneighbours = new List<DataNode>();
            potentialneighbours = checkForObstacles(startNode);

            //Console.WriteLine("After Obstacles: " + potentialneighbours.Count);

            // Remove all nodes from the list that are already in the agent's path
            potentialneighbours = removeExistingNodes(potentialneighbours);

            //Console.WriteLine("After Remove Existing: " + potentialneighbours.Count);

            // Remove any nodes that will trap the agent in one location
            potentialneighbours = removeEntrappingNodes(potentialneighbours);

            //Console.WriteLine("After entrapment: " + potentialneighbours.Count);

            potentialneighbours.Reverse();

            potentialneighbours = ListShuffle.Shuffle(potentialneighbours);

            if (potentialneighbours.Count != 0)
            {
                Random prng = new Random();
                int selectedNeighbour = prng.Next(0, potentialneighbours.Count - 1);

                //Console.WriteLine(potentialneighbours.Count);
                //Console.WriteLine(selectedNeighbour);
                //Console.WriteLine("-----");
                
                return potentialneighbours[selectedNeighbour];
            }

            

            DataNode endOfPath = new DataNode(-3, -3, -3);
            return endOfPath;

        }


        /**
         * Check if the potential neighbours already exits in the path
         */
         private List<DataNode> removeExistingNodes(List<DataNode> inputNodes)

        {

            List<DataNode> potentialNeighbours = inputNodes;


            for (int i = 0; i < potentialNeighbours.Count; i++)
            {
                if (this.agentPath.checkForExistingNode(potentialNeighbours[i]))
                {
                    //Console.WriteLine("NeighbourCountBeforeRemove: " + potentialNeighbours.Count);
                    //Console.WriteLine("Item Exists In Path");
                    potentialNeighbours.Remove(potentialNeighbours[i]);
                    //Console.WriteLine("NeighbourCountAfterRemove: " + potentialNeighbours.Count);
                }
            }

            return potentialNeighbours;
        }

        /**
         * Remove chance for entrapment
         */
         private List<DataNode> removeEntrappingNodes(List<DataNode> inputNodes)
        {

            List<DataNode> potentialNodes = inputNodes;
            for (int i = 0; i < potentialNodes.Count; i++)
            {
                // Build a new potential neighbour list from the node
                // that is being checked's neighbours. An empty list will mean
                // the node would cause entrappment
                List<DataNode> entrappmentCheckNeighbours = checkForObstacles(potentialNodes[i]);
                entrappmentCheckNeighbours = removeExistingNodes(entrappmentCheckNeighbours);
                if (entrappmentCheckNeighbours.Count == 0)
                {
                    potentialNodes.Remove(potentialNodes[i]);
                }
            }

            return potentialNodes;
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

            for (int i = 0; i < currentNode.neighbourNodes.Length; i++)
            {
                // Remove any null references for neighbours of edge nodes
                if(currentNode.neighbourNodes[i] == null)
                {
                    continue;
                }
                // Only add the nodes inside the graph to the list
                if (currentNode.neighbourNodes[i].heightValue >= 0)
                {
                    potentialNeighbours.Add(currentNode.neighbourNodes[i]);
                }
            }
            return potentialNeighbours;

        }

    }
}
