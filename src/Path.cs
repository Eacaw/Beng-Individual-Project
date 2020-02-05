using System;
using System.Collections.Generic;

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
         * Return Path Length
         */
        public int getPathLength()
        {
            return this.pathway.Count;
        }

    }
}
