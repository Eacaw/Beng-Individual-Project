using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BEng_Individual_Project.src.A_Star
{
    static class AStarMethod
    {

        public static Path runAStar(DataNode startingNode, DataNode targetNode)
        {
            List<DataNode> OpenList = new List<DataNode>();
            List<DataNode> ClosedList = new List<DataNode>();
            DataNode currentNode = startingNode;
            Path aStarPath = new Path();


            startingNode.f = 0;
            startingNode.g = 0;
            startingNode.h = (float)calculateHVal(startingNode, targetNode);
            OpenList.Add(startingNode);
            
            bool targetReached = false;

            while (!targetReached)
            {
                //Sort List according to f value
                 OpenList = OpenList.OrderBy(o => o.f).ToList();

                currentNode = OpenList[0];
                ClosedList.Add(currentNode);
                aStarPath.addNodeToPath(currentNode);

                OpenList.RemoveAt(0);

                DataNode[] neighbours = currentNode.getNeighbours();

                for ( int i = 0; i < neighbours.Length; i++)
                {

                    if (neighbours[i] == targetNode)
                    {
                        targetReached = true;
                        break;
                    }
                    // Add all neighbours to open list
                    if (!OpenList.Contains(neighbours[i]) && !ClosedList.Contains(neighbours[i]) && neighbours[i].heightValue > 0.01)
                    {
                        OpenList.Add(neighbours[i]);
                    }
                    // Set all neighbours parent to current node
                    neighbours[i].parent = currentNode;
                    // Generate the neighbours g value
                    neighbours[i].g = currentNode.f + currentNode.getCostValue(i);
                    // Generate the neighbours h value
                    neighbours[i].h = (float)calculateHVal(neighbours[i], targetNode);
                    // Generate the neighbours f value
                    neighbours[i].f = neighbours[i].g + neighbours[i].h;
                }



            }

            return aStarPath;
        }

        /**
         * Calculate the euclidean distance between current node
         * and target node for an estimated heuristic value
         */
        private static double calculateHVal(DataNode current, DataNode target)
        {
            int currentX = current.getGraphLocation()[0];
            int currentY = current.getGraphLocation()[1];
            int targetX = target.getGraphLocation()[0];
            int targetY = target.getGraphLocation()[1];

            double deltaX = currentX - targetX;
            double deltaY = currentY - targetY;

            return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
        }


    }






}
