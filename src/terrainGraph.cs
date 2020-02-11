using System;
using System.Collections.Generic;
using System.Text;
using BEng_Individual_Project.lib;
using System.Drawing;
using BEng_Individual_Project.src.Utilities;

namespace BEng_Individual_Project.src
{
    class terrainGraph
    {
        public DataNode[,] terrainNodes;
        private float[,] noiseMap;
        private int height, width;

        private DataNode edgeNode { get; set; }

        /**
         * Terrain Graph Constructor
         */
        public terrainGraph(int height, int width, float[,] noiseMap)
        {
            this.height = height;
            this.width = width;
            this.noiseMap = noiseMap;
            this.terrainNodes = new DataNode[width, height];
            this.edgeNode = new DataNode(-1,-1,-1);

            this.populateTerrainGraph();
            this.connectNodes();

        }

        public DataNode getEdgeNode()
        {
            return this.edgeNode;
        }

        /**
         * Call the noise generator to construct the graph and input all
         * data into the nodes. 
         */
        public void populateTerrainGraph()
        {
            if (noiseMap.Length == this.width * this.height)
            {
                for (int i = 0; i < this.height; i++) //TODO: Rename all loops to use [x,y] not [i,j]
                {
                    for (int j = 0; j < this.width; j++)
                    {
                        this.terrainNodes[i, j] = new DataNode(this.noiseMap[i, j],j,i);
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
         * TODO: Find a more efficient way to achieve this
         */
         public void connectNodes()
        {

            //TODO: Check to see if there's a better way to do this

            // Link all nodes except edge nodes
            for (int i = 0; i < this.height - 1; i++)
            {
                for (int j = 0; j < this.width - 1; j++)
                {
                    // North West
                    if (i != 0 && j != 0){
                        this.terrainNodes[i, j].addNeighbourNodeReference(0,this.terrainNodes[i - 1, j - 1]);
                    } else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(0, edgeNode);
                    }

                    // North
                    if (i != 0)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(1,this.terrainNodes[i - 1, j]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(1, edgeNode);
                    }

                    // North East
                    if (i != 0 && j != this.width -1)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(2,this.terrainNodes[i - 1, j + 1]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(2, edgeNode);
                    }

                    // East
                    if (j != this.width -1)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(3,this.terrainNodes[i, j + 1]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(3, edgeNode);
                    }

                    // South East
                    if (i != this.height -1 && j != this.width -1)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(4,this.terrainNodes[i + 1, j + 1]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(4, edgeNode);
                    }

                    // South
                    if (i != this.height -1)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(5,this.terrainNodes[i + 1, j]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(5, edgeNode);
                    }

                    // South West
                    if (i != this.height -1 && j != 0)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(6,this.terrainNodes[i + 1, j - 1]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(6, edgeNode);
                    }

                    // West
                    if (j != 0)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(7, this.terrainNodes[i, j - 1]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(7, edgeNode);
                    }
                }
            }

        }

        /**
         * Output a text visualisation of the graph
         * All Values in graph have been normalised between 0-99
         */
        public void printGraphToConsole()
        {
            float[] minMaxValues = minMax.findMinMax(nodesToFloatArray(), this.height, this.width);
            float minimum = minMaxValues[0], maximum = minMaxValues[1];

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
        public float mapValue(float value, float initialMin, float initialMax, float newMin, float newMax)
        {
            return (newMin + ((value - initialMin) * (newMax - newMin)) / (initialMax - initialMin));
        }

        /**
         * Method that maps all terrain height values to between 50 - 255
         * Used early in development to show the path clearly with a black line
         */
         public void increaseGrayscaleMapping()
        {
            float[] minMaxValues = minMax.findMinMax(this.noiseMap, this.height, this.width);
            float minimum = minMaxValues[0], maximum = minMaxValues[1];

            for (int height = 0; height < this.height; height++)
            {
                for (int width = 0; width < this.width; width++)
                {
                    this.terrainNodes[width, height].setHeightValue(mapValue(this.terrainNodes[width, height].heightValue, minimum, maximum, 50, 255));
                }
            }
        }

        /*
         * Method to convert the height data from the nodes into ]
         * a float array format for image saving
         */
         private float[,] nodesToFloatArray()
        {

            float[,] nodeToFloatData = new float[this.height, this.width];
            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    nodeToFloatData[i, j] = this.terrainNodes[i, j].heightValue;
                }
            }

            return nodeToFloatData;
        }

        /**
         * Generate Output image from Graph instace
         */
         public void saveImageOfGraph(string filename)
        {
            bitmapSaving.generateOutputImage(filename, this.width, this.height, nodesToFloatArray());
        }


        

    }
}
