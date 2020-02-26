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
            for (int i = 0; i < pathway.Count -1; i++)
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
         * Return the path as a tab delimited string
         * used for saving paths to a file for future use
         */
        public string getPathAsString()
        {
            StringBuilder pathAsString = new StringBuilder();

            for (int i = 0; i < this.pathway.Count; i++)
            {
                pathAsString.Append(pathway[i].getGraphLocation()[0]);
                pathAsString.Append("\t");
                pathAsString.Append(pathway[i].getGraphLocation()[1]);
                pathAsString.Append("\t");
            }

            return pathAsString.ToString();


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
         public void paintPathway(int heightValueToPaint)
        {
            for (int i = 0; i < this.pathway.Count; i++)
            {
                this.pathway[i].setHeightValue(heightValueToPaint);
            }
        }

        /* take in a list of nodes and
         * convert them into a path object
         */
        public Path pathFromList(List<DataNode> inputList)
        {
            Path outputPath = new Path();
            for(int i = 0; i < inputList.Count -1; i++)
            {
                outputPath.addNodeToPath(inputList[i]);
            }
            return outputPath;
        }


        /*
         * Check used to see if a mutation path is more or
         * less efficient than the previous path
         */
        public bool checkForImprovedPath(Path newPath, DataNode startNode, DataNode endNode, float existingCost)
        {
            if (this.pathway.Contains(startNode) && this.pathway.Contains(endNode))
            {
                Path pathSection = new Path();
                pathSection.addNodeToPath(startNode);

                int startIndex = this.pathway.IndexOf(startNode);
                int endIndex = this.pathway.IndexOf(endNode);

                for (int i = startIndex; i < endIndex + 1; i++)
                {
                    pathSection.addNodeToPath(this.pathway[i]);
                }

                float newPathCost = newPath.getPathCost();
                float sectionPathCost = pathSection.getPathCost();

                int newPathSize = newPath.getNodeCount();
                int sectionPathSize = endIndex - startIndex + 1;

                if (newPathCost < sectionPathCost && Math.Abs(newPathSize - sectionPathSize) < 10) // Arbitrary value //TODO: Figure out what to do with this method
                {
                    return true;
                }
                else
                {
                    return false;
                }


            } 
            else
            {
                // One or both nodes are not within the pathway
                // Do not replace with new path
                return false;
            }
        }

        /**
         * returns the final node in the path list
         */
         public DataNode getFinalNode()
        {
            return this.pathway[this.pathway.Count - 1];
        }


        /*
         * returns the integer count of the number of nodes in a pathway
         */
         public int getNodeCount()
        {
            return this.pathway.Count;
        }


    }
}
