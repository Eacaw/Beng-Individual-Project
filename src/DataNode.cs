using System;
using System.Collections.Generic;
using System.Text;

namespace BEng_Individual_Project.src
{
    public class DataNode
    {
        public DataNode[] neighbourNodes = new DataNode[8]; // Max Neighbours is 8 using NW -> W clockwise
        public float heightValue { get; set; }
        int[] graphPosition = new int[2];
        float hostileRiskValue;


        /**
         * Data Node Constructor
         */
        public DataNode(float heightValue, int xPosition, int yPosition)
        {
            this.heightValue = heightValue;
            this.graphPosition[0] = xPosition;
            this.graphPosition[1] = yPosition;
            this.hostileRiskValue = 0;
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
        /**
         * Overloaded method from above, takes in 
         */
        public float getCostValue(DataNode neighbourNode)
        {
            {
                float costValue;

                if (neighbourNode.heightValue != -1) // Ensure that the neighbour being indexed isn't an edge
                {
                    costValue = this.heightValue - neighbourNode.heightValue;

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

        /**
         * Add calculated hostileRiskValue to node data
         */
         public void addHostileRiskValue(float hostileRisk)
        {
            this.hostileRiskValue = hostileRisk;
        }

    }
}
