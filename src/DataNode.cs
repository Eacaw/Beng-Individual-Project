using System;
using System.Collections.Generic;
using System.Text;

namespace BEng_Individual_Project.src
{
    class DataNode
    {
        public DataNode[] neighbourNodes = new DataNode[8];
        public float heightValue { get; set; }


        /**
         * Data Node Constructor
         */
        public DataNode(float heightValue)
        {
            this.heightValue = heightValue;
        }

        /**
         * Method used to construct the graph, connecting all
         * nodes together with references to their surrounding nodes
         */
        public void addNeighbourNodeReference(int index, DataNode neighbourNode)
        {
            this.neighbourNodes[index] = neighbourNode;
        }

        /**
         * Returns the absolute value for the difference in height
         * values between nodes, uphill cost = height difference, downhill
         * cost = height difference/2
         */
        public float getCostValue(int neighbourIndex)
        {
            float costValue;

            if (this.neighbourNodes[neighbourIndex] != null) // Ensure that the neighbour being indexed isn't an edge
            {
                costValue = this.heightValue - this.neighbourNodes[neighbourIndex].heightValue;

                if (costValue > 0) // Path is Downhill
                {
                    return costValue / 2;
                }
                else if (costValue < 0) // Path is Uphill
                {
                    return Math.Abs(costValue);
                }
                else // Path has no height difference
                {
                    return 0;
                }
            }

            return -1; // Neighbour was an edge, and no value can be found.
        }
    }
}
