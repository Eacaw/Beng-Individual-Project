using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace BEng_Individual_Project.src
{
    public class DataNode
    {
        public DataNode[] neighbourNodes = new DataNode[8]; // Max Neighbours is 8 using NW -> W clockwise
        public float heightValue { get; set; }
        int[] graphPosition = new int[2];
        public float hostileRiskValue { get; set; }

        public int traversedCounter { get; set; }
        public float paintValue { get; set; } 


        // Variables used for the A* algorithm
        public float f {get; set;}
        public float h { get; set; }
        public float g { get; set; }

        public DataNode parent { get; set; }


        /**
         * Data Node Constructor
         */
        public DataNode(float heightValue, int xPosition, int yPosition)
        {
            this.heightValue = heightValue;
            this.graphPosition[0] = xPosition;
            this.graphPosition[1] = yPosition;
            this.hostileRiskValue = 0;

            this.paintValue = heightValue;
        }

        /**
         * Method to return cartesian position on the graph
         */
         public int[] getGraphLocation()
        {
            return this.graphPosition;
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

            // Temporary fix to kill any path that has sequential nodes that aren't neighbours
            // TODO: Figure out what is causing the breaks.
            if(neighbourIndex < 0 || neighbourIndex > 7)
            {
                return 1000000;
            }

            if (this.neighbourNodes[neighbourIndex] != null && this.neighbourNodes[neighbourIndex].heightValue > 0) // Ensure that the neighbour being indexed isn't an edge
            {
                costValue = this.heightValue - this.neighbourNodes[neighbourIndex].heightValue;

                float pathDistance = 0;

                if (neighbourIndex == 0 || neighbourIndex % 2 == 0)
                {
                    pathDistance = 1;
                }
                else
                {
                    pathDistance = 1.41f;
                }

                if (costValue > 0) // Path is Downhill
                {
                    costValue /= 2;
                    //costValue *= 8;
                    costValue += pathDistance;;
                    return costValue;
                }
                else if (costValue < 0) // Path is Uphill
                {
                    return ((Math.Abs(costValue)*2)+ pathDistance);
                }
                else // Path has no height difference
                {
                    return (pathDistance);
                }
            }

            return -1; // Neighbour was an edge, and no value can be found.
        }


        /**
         * Overloaded method from above, takes in refernce to data node neighbour
         */
        public float getCostValue(DataNode neighbourNode)
        {
            {
                if (neighbourNode.heightValue > 0) // Ensure that the neighbour being indexed isn't an edge
                {

                    int neighbourIndex = this.neighbourNodes.IndexOf(neighbourNode);

                    return getCostValue(neighbourIndex);
                }
                return -1; // Neighbour was an edge, and no value can be found.
            }
        }

        /**
         * Add calculated hostileRiskValue to node data
         */
         public void addHostileRiskValue(float hostileRisk)
        {
            this.hostileRiskValue += hostileRisk;
        }

        /**
         * Method to manualy set the height value
         */
         public void setHeightValue(float heightValue)
        {
            this.heightValue = heightValue;
        }

        public DataNode[] getNeighbours()
        {
            return this.neighbourNodes;
        }

    }
}
