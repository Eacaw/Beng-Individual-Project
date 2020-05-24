using System;
using System.Collections.Generic;
using System.Text;
using BEng_Individual_Project.lib;
using System.Drawing;
using System.Numerics;
using BEng_Individual_Project.src.Utilities;
using System.Drawing.Imaging;

namespace BEng_Individual_Project.src
{
    public class terrainGraph
    {
        public DataNode[,] terrainNodes;
        public float[,] noiseMap;
        public int height, width;

        public float maxRisk;

        private DataNode edgeNode { get; set; }

        /**
         * Terrain Graph Constructor
         */
        public terrainGraph(int dimensions)
        {
            // Initial Noise Values
            float[,] noiseValues;

            // Noise map generation Parameters
            int width = dimensions; // Value must be divisable by two into an integer
            int height = width; // Map must be square to avoid out of bounds error
            int seed = 1337; // RNG seed
            int octaves = 8; // Level of details
            float scale = 0.005f; // Higher scale = Less Terrain details
            int lacunarity = 2; // Value must be > 1
            float persistance = 1; // Value must be 0-1

            // GA Defining Variables
            int populationSize = 100;
            this.maxRisk = 0;

            // Generate the initial noise map using Simplex Noise
            noiseValues = NoiseMapLayering.getNoiseData(width, height, seed, octaves, scale, lacunarity, persistance);

            this.height = height;
            this.width = width;
            this.noiseMap = noiseValues;
            this.terrainNodes = new DataNode[width, height];
            this.edgeNode = new DataNode(-1, -1, -1);

            this.populateTerrainGraph();
            this.connectNodes();

        }

        public DataNode getEdgeNode()
        {
            return this.edgeNode;
        }

        public void resetNodeHeightValues()
        {
            for (int i = 0; i < this.height; i++) //TODO: Rename all loops to use [x,y] not [i,j]
            {
                for (int j = 0; j < this.width; j++)
                {
                    if (this.terrainNodes[i, j].heightValue >= 0)
                    {
                        this.terrainNodes[i, j].heightValue = this.noiseMap[i, j];
                        this.terrainNodes[i, j].paintValue = this.noiseMap[i, j];
                    }

                }
            }
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
                        this.terrainNodes[i, j] = new DataNode(this.noiseMap[i, j], j, i);
                    }
                }
            }
            else
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
                    if (i != 0 && j != 0)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(0, this.terrainNodes[i - 1, j - 1]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(0, edgeNode);
                    }

                    // North
                    if (i != 0)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(1, this.terrainNodes[i - 1, j]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(1, edgeNode);
                    }

                    // North East
                    if (i != 0 && j != this.width - 1)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(2, this.terrainNodes[i - 1, j + 1]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(2, edgeNode);
                    }

                    // East
                    if (j != this.width - 1)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(3, this.terrainNodes[i, j + 1]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(3, edgeNode);
                    }

                    // South East
                    if (i != this.height - 1 && j != this.width - 1)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(4, this.terrainNodes[i + 1, j + 1]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(4, edgeNode);
                    }

                    // South
                    if (i != this.height - 1)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(5, this.terrainNodes[i + 1, j]);
                    }
                    else
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(5, edgeNode);
                    }

                    // South West
                    if (i != this.height - 1 && j != 0)
                    {
                        this.terrainNodes[i, j].addNeighbourNodeReference(6, this.terrainNodes[i + 1, j - 1]);
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
            float[] minMaxValues = numericalUtilities.findMinMax(nodesToFloatArray(), this.height, this.width);
            float minimum = minMaxValues[0], maximum = minMaxValues[1];

            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    Console.Write(Math.Floor(numericalUtilities.mapValue(terrainNodes[i, j].heightValue, minimum, maximum, 0, 99)) + "\t");
                }
                Console.Write("\n");
            }
        }

        /**
         * Method that maps all terrain height values to between 50 - 255
         * Used early in development to show the path clearly with a black line
         */
        public void increaseGrayscaleMapping()
        {
            float[] minMaxValues = numericalUtilities.findMinMax(this.noiseMap, this.height, this.width);
            float minimum = minMaxValues[0], maximum = minMaxValues[1];

            for (int height = 0; height < this.height; height++)
            {
                for (int width = 0; width < this.width; width++)
                {
                    this.terrainNodes[width, height].setHeightValue(numericalUtilities.mapValue(this.terrainNodes[width, height].heightValue, minimum, maximum, 50, 255));
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

                    if (this.terrainNodes[i, j].hostileRiskValue > 0)
                    {
                        nodeToFloatData[i, j] = 500;
                    }

                    if (this.terrainNodes[i, j].heightValue != this.terrainNodes[i, j].paintValue)
                    {
                        nodeToFloatData[i, j] = this.terrainNodes[i, j].paintValue;
                    }


                    
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

        /**
         * Map an arbitrary value into RGB using the minimum and maximum possible values
         */
        private static Color nodeToRgb(float minimum, float maximum, float value, bool grayscale)
        {
            float normalizedValue = ((value - minimum) / (maximum - minimum));
            int r, g, b;

            if (grayscale)
            {
                r = (int)Math.Clamp(normalizedValue * 255.0, 0, 255);
                g = (int)Math.Clamp(normalizedValue * 255.0, 0, 255);
                b = (int)Math.Clamp(normalizedValue * 255.0, 0, 255);
            }
            else
            {
                const float redFactor = 3.0f;
                const float greenFactor = 1.0f;
                const float blueFactor = 0.5f;

                r = (int)Math.Clamp(((redFactor * normalizedValue) * 255.0), 0, 255);
                g = (int)Math.Clamp((greenFactor * (1 - normalizedValue)) * 255.0, 0, 255);
                b = 0;
            }

                return Color.FromArgb(r, g, b);
        }

        public void saveHeatmapImageOfGraph(string filename)
        {
            int minTraversed = int.MaxValue;
            int maxTraversed = 0;

            // Get the min and max traversal count of the graph, so can normalise the dataset
            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    if (this.terrainNodes[i, j].traversedCounter < minTraversed)
                    {
                        minTraversed = this.terrainNodes[i, j].traversedCounter;
                    }
                    if (this.terrainNodes[i, j].traversedCounter > maxTraversed)
                    {
                        maxTraversed = this.terrainNodes[i, j].traversedCounter;
                    }
                }
            }

            // Hijack this code from elsewhere to get terrain minmax, can do it in the above loop instead
            float[] minMaxValues = numericalUtilities.findMinMax(nodesToFloatArray(), this.height, this.width);
            float minHeight = minMaxValues[0];
            float maxHeight = minMaxValues[1];

            Bitmap bmp = new Bitmap(this.width, this.height);

            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    Color pixel;

                    bool isTerrain = this.terrainNodes[j, i].traversedCounter == 0;

                    if (isTerrain)
                    {
                        pixel = nodeToRgb(minHeight, maxHeight, this.terrainNodes[j,i].paintValue, true);
                    }
                    else
                    {
                        pixel = nodeToRgb(minTraversed, maxTraversed, (float)Math.Pow(this.terrainNodes[j,i].traversedCounter,3), false);
                    }

                    // Map the traversal count into RGB
                    bmp.SetPixel(i, j, pixel);
                }
            }

            bmp.Save(filename, ImageFormat.Bmp);
        }



        public void addHostileToGraph(Hostile hostileNode, int[] hostilePosition)
        {
            int hostileXPos = hostilePosition[0];
            int hostileYPos = hostilePosition[1];

            // Set the hostile position
            hostileNode.hostilePosition = hostilePosition;

            // Find the furthest point to calculate the risk from
            // to avoid iterating over the entire graph
            int riskStartX = (int) Math.Floor(hostileXPos - hostileNode.hostileRange);
            int riskStartY = (int) Math.Floor(hostileYPos - hostileNode.hostileRange);

            for (int i = riskStartX; i < (riskStartX + (2 * hostileNode.hostileRange)); i++)
            {
                for (int j = riskStartY; j < (riskStartY + (2 * hostileNode.hostileRange)); j++)
                { 
                    // Find the distance between the node and the hostile
                    float distanceToHostile = numericalUtilities.getDistanceBetweenNodes(this.terrainNodes[i, j], hostilePosition);

                    // Only apply the risk value if it is within range
                    if (distanceToHostile < hostileNode.hostileRange)
                    {
                        float hostileRiskValue = hostileNode.hostileSeverity / (distanceToHostile + 1);
                        this.terrainNodes[i, j].addHostileRiskValue(hostileRiskValue);
                    }
                }
            }
        }


        public void calculateMaximumRisk()
        {
            float totalRiskValue = 0;
            for(int i = 0; i < this.width; i++)
            {
                for(int j = 0; j < this.height; j++)
                {
                    totalRiskValue += this.terrainNodes[i, j].hostileRiskValue;
                }
            }

            this.maxRisk = totalRiskValue;
        }
    }
}
