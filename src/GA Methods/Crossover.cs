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
            int crossoverPointCount = countMatchingNodes(parentAgents);

            Random prng = new Random();

            // No crossover possible
            if (crossoverPointCount == 0)
            {
                return parentAgents.getParentA();
            }
            // Single Point crossover only
            else if(crossoverPointCount < 5)
            {
                int crossoverPoint = prng.Next(0, crossoverPointCount);
                return singlePointCrossover(parentAgents, crossoverPoint);
            }
            else // perform double point crossover
            {  
                int crossoverIndexA = prng.Next(0, crossoverPointCount-3);
                int crossoverIndexB = crossoverIndexA;

                while (crossoverIndexB == crossoverIndexA || crossoverIndexB < crossoverIndexA)
                {
                    crossoverIndexB = prng.Next(0, crossoverPointCount);
                }
                return doublePointCrossover(parentAgents, crossoverIndexA, crossoverIndexB);
            }

        }

        /**
         * Find the indecies for all of the crossover points between two
         * paths, ignoring the first two nodes in a path. 
         * Index 0 = edgeNode
         * Index 1 = startingNode
         */
        public static int countMatchingNodes(matingPartners parents)
        {
            // Create a list of indecies where paths meet
            int crossoverPointCount = 0;

            // Pull parentA's path out for use
            List<DataNode> parentAPath = parents.getParentA().agentPath.getPathway();
            List<DataNode> parentBPath = parents.getParentB().agentPath.getPathway();

            // Iterate over the path, ignoring index 0 and 1 as edge and starting nodes respectively
            // NOTE: only need to iterate over pathA because it's always set to the longest path
            for (int i = 2; i < parents.getParentA().agentPath.getNodeCount(); i++)
            {
                // Check if each node is contained within ParentB's path
                if (parentBPath.Contains(parentAPath[i]))
                {
                    parents.crossoverIndeciesA.Add(parentAPath.IndexOf(parentAPath[i]));
                    parents.crossoverIndeciesB.Add(parentBPath.IndexOf(parentAPath[i]));
                    crossoverPointCount++;
                }
            }

            return crossoverPointCount;
        }


        /**
         * single point crossover that produces a new child path
         * based on a single point between two paths
         */
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
            for (int i = 1; i < parents.crossoverIndeciesA[crossoverIndex]; i++)
            {
                childPath.addNodeToPath(parentAPath[i]);
            }

            // Add second half of path from parent B
            for (int i = parents.crossoverIndeciesB[crossoverIndex]; i < parentBPath.Count; i++)
            {
                childPath.addNodeToPath(parentBPath[i]); 
            }

            // Create the child agent
            Agent childAgent = new Agent(parents.getParentA(), childPath);

            childAgent.agentPath.getPathCost();

            return childAgent;
        }






        private static Agent doublePointCrossover(matingPartners parents, int crossoverPointA, int crossoverPointB)
        {
            // Pull both parent's paths out for use
            List<DataNode> parentAPath = parents.getParentA().agentPath.getPathway();
            List<DataNode> parentBPath = parents.getParentB().agentPath.getPathway();

            // Create a new path for the child
            // Paths created with both possibilities are generated
            // and the one with the lower cost is favoured
            Path childPath = new Path();
            //Path secondChildPath = new Path();

            // Add Edge Node to Path at index 0
            childPath.addNodeToPath(parentBPath[0]);
            //secondChildPath.addNodeToPath(parentAPath[0]);

            int parentACrossoverA = parents.crossoverIndeciesA[crossoverPointA];
            int parentACrossoverB = parents.crossoverIndeciesA[crossoverPointB];

            int parentBCrossoverA = parents.crossoverIndeciesB[crossoverPointA];
            int parentBCrossoverB = parents.crossoverIndeciesB[crossoverPointB];



            // Add the first half of the path from parent A
            for (int i = 1; i < parentACrossoverA; i++)
            {
                childPath.addNodeToPath(parentAPath[i]);
                //secondChildPath.addNodeToPath(parentBPath[i]);
            }

            // Add second half of path from parent B
            for (int i = parentBCrossoverA; i < parentBCrossoverB; i++)
            {
                childPath.addNodeToPath(parentBPath[i]);
                //secondChildPath.addNodeToPath(parentAPath[i]);
            }

            // Add the remaining path
            for (int i = parentACrossoverB; i < parentAPath.Count; i++)
            {
                childPath.addNodeToPath(parentAPath[i]);
            }

            Agent childAgent = new Agent(parents.getParentA(), childPath);
            childAgent.agentPath.getPathCost();


            return childAgent;
        }




    }
}
