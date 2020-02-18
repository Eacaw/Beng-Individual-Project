using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BEng_Individual_Project.src;
using BEng_Individual_Project.lib;
using System.Threading;

namespace BEng_Individual_Project
{
    class Program
    {

        private static int complete = 0;

        static void Main(string[] args)
        {

            float[,] noiseValues;

            int width = 500; // Value must be divisable by two into an integer
            int height = width; // No idea why I get an out of bounds if the map isn't square. WTF! 
            int seed = 1337;
            int octaves = 8;
            float scale = 0.005f;
            int lacunarity = 2;
            float persistance = 1;

            noiseValues = NoiseMapLayering.getNoiseData(width, height, seed, octaves, scale, lacunarity, persistance);

            terrainGraph graph = new terrainGraph(height, width, noiseValues);

            bool targetFound = false;

            // Output image of graph without paths
            graph.saveImageOfGraph("../../../OutputImages/TerrainImage");

            graph.increaseGrayscaleMapping();

            int totalAgentTime = 0;
            int nonZeroAgents = 0;
            int agentsReachedTargetCount = 0;

            // Run the simulation 100 times
            // Gets the results for the average time for each agent
            for (int i = 0; i < 1000; i++)
            {
                // Start Stopwatch
                var agentWatch = System.Diagnostics.Stopwatch.StartNew();

                // Generate new Agent for testing
                Agent testAgent = new Agent(graph.terrainNodes[0, 0], graph.terrainNodes[width / 2, height / 2], graph.getEdgeNode());

                // Perform Search
                int output = testAgent.performBlindSearch();
                if (output == 255)
                {
                    agentsReachedTargetCount++;
                }

                // Stop stopwatch
                agentWatch.Stop();

                // Report on the cost of the path
                Console.WriteLine("Agent " + i + ", Path Cost: " + testAgent.agentPath.getPathCost());

                // Paint pathway to graph
                testAgent.agentPath.paintPathway(output);

                // Add the agent's time to total for averaging
                // Only add agents that didn't instantly die to total
                var elapsedMS = agentWatch.ElapsedMilliseconds;
                if ((int)elapsedMS > 10)
                {
                    totalAgentTime += (int)elapsedMS;
                    nonZeroAgents++;
                }

                //Output time taken
                Console.WriteLine("Time taken for agent " + i + ": " + (int)elapsedMS);
                Console.WriteLine("------");
            }
            // Output average time per agent
            Console.WriteLine("Average per agent: " + totalAgentTime / nonZeroAgents);
            Console.WriteLine("Agents that reached Target: " + agentsReachedTargetCount);
            Console.WriteLine("Total Computing Time: " + totalAgentTime / 60 + " seconds");

            // Generate output image including paths
            graph.saveImageOfGraph("../../../OutputImages/PathwayTesting");
        }
    }

}

