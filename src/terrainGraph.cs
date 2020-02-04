using System;
using System.Collections.Generic;
using System.Text;

namespace BEng_Individual_Project.src
{
    class terrainGraph
    {
        private DataNode[,] terrainNodes;
        private float[,] noiseMap;
        private int height, width;

        private float minimum = float.MaxValue, maximum = 0;

        /**
         * Terrain Graph Constructor
         */
        public terrainGraph(int height, int width, float[,] noiseMap)
        {
            this.height = height;
            this.width = width;
            this.noiseMap = noiseMap;
            this.terrainNodes = new DataNode[width, height];
        }

        /**
         * Call the noise generator to construct the graph and input all
         * data into the nodes. 
         */
        private void populateTerrainGraph()
        {
            if (noiseMap.Length == this.width * this.height)
            {
                for (int i = 0; i < this.height; i++)
                {
                    for (int j = 0; j < this.width; j++)
                    {
                        this.terrainNodes[i, j] = new DataNode(this.noiseMap[i, j]);
                    }
                }
            } else
            {
                Console.WriteLine("Size of Noisemap does not match size of graph");
            }
        }

        /**
         * Connect the graph's nodes together through node's neighbour Array
         * DO NOT CALL BEFORE POPULATING GRAPH!
         */
         private void connectNodes()
        {

        }

        /**
         * Output a text visualisation of the graph
         * All Values in graph have been normalised between 0-99
         */
        public void printGraphToConsole()
        {
            populateTerrainGraph();
            findMinMax(noiseMap, this.height, this.width);
            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    Console.Write(Math.Floor(mapValue(terrainNodes[i,j].heightValue, minimum, maximum, 0, 99))+ "\t");
                }
                Console.Write("\n");
            }
        }

        /**
         * Linearly map values into a new given range
         */
        private float mapValue(float value, float initialMin, float initialMax, float newMin, float newMax)
        {
            return (newMin + ((value - initialMin) * (newMax - newMin)) / (initialMax - initialMin));
        }


        /**
         * TODO: DEBUG CODE - REMOVE AND REWRITE METHOD IN NOISE LAYERING
         */
        private void findMinMax(float[,] noiseMap, int height, int width)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (noiseMap[i, j] > maximum)
                    {
                        maximum = noiseMap[i, j];
                    }

                    if (noiseMap[i, j] < minimum)
                    {
                        minimum = noiseMap[i, j];
                    }
                }
            }
        }

    }
}
