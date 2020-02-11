using System;
using System.Collections.Generic;
using System.Text;

namespace BEng_Individual_Project.src
{
    public class Path
    {

        List<DataNode> pathway { get; set; }

        public Path()
        {
            this.pathway = new List<DataNode>();
        }

        /**
         * Iterate through the path list and sum
         * all heigh cost values
         */
        public float getPathCost()
        {
            float totalCostValue = 0;
            for (int i = 0; i < pathway.Count - 1; i++)
            {
                float tempCost = this.pathway[i].getCostValue(this.pathway[i + 1]);
                if(tempCost != -1) // Cost value of -1 means edge node has been found
                {
                    totalCostValue += tempCost;
                } else
                {
                    break; // Break out of loop to avoid out of bounds error.
                }
            }

            return totalCostValue;
        }

        /**
         * Add a node to the path
         */
         public void addNodeToPath(DataNode newPathNode)
        {
            this.pathway.Add(newPathNode);
        }

        /**
         * Return Path Length - Total number of nodes in path
         */
        public int getPathLength()
        {
            return this.pathway.Count;
        }

        /**
         * Check if node is in path already
         */
         public bool checkForExistingNode(DataNode checkNode)
        {
            if (this.pathway.Contains(checkNode))
            {
                return true;
            } else
            {
                return false;
            }
        }

        /**
         * Method to print the pathway in cartesian form
         */
         public void printPathway()
        {
            for(int i = 0; i < this.pathway.Count; i++)
            {
                StringBuilder GraphPos = new StringBuilder("x: ");
                GraphPos.Append(pathway[i].getGraphLocation()[0]);
                GraphPos.Append(" y: ");
                GraphPos.Append(pathway[i].getGraphLocation()[1]);
                Console.WriteLine(GraphPos.ToString());
            }
        }

        /**
         * Mehtod paints all nodes in the path with a value of 0
         */
         public void paintPathway()
        {
            for (int i = 0; i < this.pathway.Count; i++)
            {
                this.pathway[i].setHeightValue(0);
            }
        }

    }
}
