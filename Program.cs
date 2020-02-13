using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BEng_Individual_Project.src;
using BEng_Individual_Project.src.A_Star;
using System.Threading;

namespace BEng_Individual_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initial Noise Values
            float[,] noiseValues;

            // Noise map generation Parameters
            int width = 500; // Value must be divisable by two into an integer
            int height = width; // No idea why I get an out of bounds if the map isn't square. WTF! 
            int seed = 1337; // RNG seed
            int octaves = 8; // Level of details
            float scale = 0.005f; // Bigger scale = Less Terrain details
            int lacunarity = 2; // Value must be > 1
            float persistance = 1; // Value must be 0-1

            // Generate the initial noise map using Simplex Noise
            noiseValues = NoiseMapLayering.getNoiseData(width, height, seed, octaves, scale, lacunarity, persistance);

            // Construct the graph and link all the nodes within
            terrainGraph graph = new terrainGraph(height, width, noiseValues);

            // Output image of graph without paths
            graph.saveImageOfGraph("../../../OutputImages/TerrainImage.bmp");

            // Bring the noise values up so that a black pathway can easily be seen
            graph.increaseGrayscaleMapping();

            int totalAgentTime = 0;
            int nonZeroAgents = 0;
            int agentsReachedTargetCount = 0;
            int agentCount = 0;
            int prevAgentCount = 0;

            //src.Path shortestPath = AStarMethod.runAStar(graph.terrainNodes[1, 1], graph.terrainNodes[49, 49]);

            //shortestPath.paintPathway(0);

            //Run the simulation 100 times
            while (nonZeroAgents < 1000)
            {
                // Start Stopwatch
                var agentWatch = System.Diagnostics.Stopwatch.StartNew();

                // Generate new Agent for testing
                Agent testAgent = new Agent(graph.terrainNodes[0, 0], graph.terrainNodes[width -2, height - 2], graph.getEdgeNode());
                
                // Perform Search
                int output = testAgent.performBlindSearch();
                if (output == 255)
                {
                    agentsReachedTargetCount++;
                }

                // Stop stopwatch
                agentWatch.Stop();

                // Report on the cost of the path
                if (testAgent.agentPath.getPathCost() > 500)
                {
                    //Console.WriteLine("Agent " + agentCount);
                    //Console.WriteLine("Path Cost: " + testAgent.agentPath.getPathCost());
                    nonZeroAgents++;

                    // Paint pathway to graph
                    testAgent.agentPath.paintPathway(output);

                    // Add the agent's time to total for averaging
                    // Only add agents that didn't instantly die to total
                    var elapsedMS = agentWatch.ElapsedMilliseconds;
                    if ((int)elapsedMS > 10)
                    {
                        totalAgentTime += (int)elapsedMS;

                    }

                    //Output time taken
                    
                    //Console.WriteLine("Time taken: " + (int)elapsedMS);
                    Console.WriteLine("Agent: " + agentCount + " \t | NZAs : " + nonZeroAgents + " \t | Time: " + elapsedMS + " \t | ZAs: " + (agentCount - prevAgentCount - 1));
                    prevAgentCount = agentCount;
                    //Console.WriteLine("------");
                }
                agentCount++;
            }
            // Output average time per agent
            Console.WriteLine("Average per agent: " + totalAgentTime / nonZeroAgents);
            Console.WriteLine("Agents that reached Target: " + agentsReachedTargetCount);
            Console.WriteLine("Total Computing Time: " + totalAgentTime / 1000 + " seconds");

            //// Generate output image including paths
            graph.saveImageOfGraph("../../../OutputImages/PathwayTesting.bmp");
        }
    }

}

