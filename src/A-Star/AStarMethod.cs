using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BEng_Individual_Project.src.A_Star
{
    static class AStarMethod
    {

        public static Path runAStar(DataNode startingNode, DataNode targetNode, terrainGraph graph)
        {
            List<DataNode> OpenList = new List<DataNode>();
            List<DataNode> ClosedList = new List<DataNode>();
            DataNode currentNode = startingNode;
            Path aStarPath = new Path();



            currentNode.g = 0;
            currentNode.h = (float)calculateHVal(currentNode, targetNode);
            currentNode.f = currentNode.h + currentNode.g;
            OpenList.Add(currentNode);

            bool targetReached = false;

            while (!targetReached)
            {


                DataNode[] neighbours = currentNode.getNeighbours();
                if (neighbours[0] == null)
                {
                    targetReached = true;

                    targetNode.parent = currentNode;



                    aStarPath = generatePath(targetNode);
                    Console.WriteLine("Path Cost: " + aStarPath.getPathCost());
                    break;
                }

                for (int i = 0; i < neighbours.Length; i++)
                {
                    if (neighbours[i] == targetNode || neighbours == null)
                    {
                        targetReached = true;

                        targetNode.parent = currentNode;



                        aStarPath = generatePath(targetNode);
                        Console.WriteLine("Path Cost: " + aStarPath.getPathCost());
                        break;
                    }
                    else
                    {
                        // Add all neighbours to open list
                        if (!OpenList.Contains(neighbours[i]) && !ClosedList.Contains(neighbours[i]) && neighbours[i].heightValue > 0)
                        {
                            OpenList.Add(neighbours[i]);
                            // Set all neighbours parent to current node
                            neighbours[i].parent = currentNode;
                            // Generate the neighbours g value
                            neighbours[i].g = currentNode.f + (currentNode.getCostValue(i)* 1000);
                            // Generate the neighbours h value
                            neighbours[i].h = (float)calculateHVal(neighbours[i], targetNode);
                            // Generate the neighbours f value
                            float newF = neighbours[i].g + neighbours[i].h;
                            if (newF < neighbours[i].f || neighbours[i].f == 0)
                            {
                                neighbours[i].f = newF;
                            }
                        }

                    }
                }
                ClosedList.Add(currentNode);
                OpenList.Remove(currentNode);

                //Sort List according to f value
                OpenList = OpenList.OrderBy(o => o.f).ToList();

                currentNode = OpenList[0];

                neighbours = new DataNode[8];



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

        private static Path generatePath(DataNode target)
        {
            List<DataNode> endPath = new List<DataNode>
            {
                target
            };
            DataNode current = target;
            while(current.parent != null)
            {
                endPath.Add(current.parent);
                current = current.parent;
            }
            return new Path().pathFromList(endPath);
        }


    }






}
