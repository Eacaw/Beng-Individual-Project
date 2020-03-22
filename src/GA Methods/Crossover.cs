using System;
using System.Collections.Generic;
using System.Text;
using BEng_Individual_Project.src;
using BEng_Individual_Project.GA_Methods;

namespace BEng_Individual_Project.GA_Methods
{
    public static class Crossover
    {


        /**
         * Top level crossover method
         * will check the number of matching nodes before
         * selecting the most appropriate crossover method 
         * for the mating partners it is given
         */
         public static Agent performCrossover(matingPartners parentAgents)
        {
            List<int> crossoverPointIndecies = countMatchingNodes(parentAgents);

            if (crossoverPointIndecies.Count == 0)
            {
                return parentAgents.getParentA();
            }

            Random prng = new Random();

            int crossoverPoint = prng.Next(0, crossoverPointIndecies.Count);

            return singlePointCrossover(parentAgents, crossoverPoint);


        }

        /**
         * Find the indecies for all of the crossover points between two
         * paths, ignoring the first two nodes in a path. 
         * Index 0 = edgeNode
         * Index 1 = startingNode
         */
        public static List<int> countMatchingNodes(matingPartners parents)
        {
            // Create a list of indecies where paths meet
            List<int> crossoverPointIndecies = new List<int>();

            // Pull parentA's path out for use
            List<DataNode> parentAPath = parents.getParentA().agentPath.getPathway();

            // Iterate over the path, ignoring index 0 and 1 as edge and starting nodes respectively
            for (int i = 2; i < parents.getParentA().agentPath.getNodeCount(); i++)
            {
                // Check if each node is contained within ParentB's path
                if (parents.getParentB().agentPath.checkForExistingNode(parentAPath[i]))
                {
                    // Add the index to the list
                    crossoverPointIndecies.Add(i);
                }
            }

            return crossoverPointIndecies;
        }

        private static Agent singlePointCrossover(matingPartners parents, int crossoverIndex)
        {
            // Pull both parent's paths out for use
            List<DataNode> parentAPath = parents.getParentA().agentPath.getPathway();
            List<DataNode> parentBPath = parents.getParentB().agentPath.getPathway();

            // Create a new path for the child
            Path childPath = new Path();

            // Add Edge Node to Path at index 0
            childPath.addNodeToPath(parentBPath[0]);

            // Add the first half of the path from parent A
            for (int i = 1; i < crossoverIndex; i++)
            {
                childPath.addNodeToPath(parentAPath[i]);
            }

            // Add second half of path from parent B
            for (int i = crossoverIndex; i < parentBPath.Count; i++)
            {
                childPath.addNodeToPath(parentBPath[i]); 
            }

            // Create the child agent
            Agent childAgent = new Agent(parents.getParentA(), childPath); 

            return childAgent;
        }

        private static Agent doublePointCrossover(matingPartners parents)
        {

            Agent childAgent = new Agent(parents.getParentA(), parents.getParentA().agentPath);

            return childAgent;
        }




    }
}
