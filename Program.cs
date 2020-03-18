using System;
using System.Collections.Generic;
using BEng_Individual_Project.src;
using BEng_Individual_Project.GA_Methods;
using BEng_Individual_Project.src.Utilities;
using System.Linq;
using System.Text;

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
            int seed = 13337; // RNG seed
            int octaves = 8; // Level of details
            float scale = 0.005f; // Bigger scale = Less Terrain details
            int lacunarity = 2; // Value must be > 1
            float persistance = 1; // Value must be 0-1

            // GA Defining Variables
            int populationSize = 1000;

            // Generate the initial noise map using Simplex Noise
            noiseValues = NoiseMapLayering.getNoiseData(width, height, seed, octaves, scale, lacunarity, persistance);

            // Construct the graph and link all the nodes within
            terrainGraph graph = new terrainGraph(height, width, noiseValues);

            graph.increaseGrayscaleMapping();

            for(int i = 0; i < 50; i++)
            {
                graph = Obstacle.addObstacletoGraph(graph, 25, 25);
            }

            DataNode graphStartNode = graph.terrainNodes[0, 0];
            DataNode graphTargetNode = graph.terrainNodes[498, 498];

            generateInitialPopulation(graph, 100);

            graph.saveImageOfGraph("../../../OutputImages/ObstacleTesting3.bmp");

        }

        public static void outputVariableTests()
        {

            // Initial Noise Values
            float[,] noiseValues;

            // Noise map generation Parameters
            int width = 500; // Value must be divisable by two into an integer
            int height = width; // No idea why I get an out of bounds if the map isn't square. WTF! 
            int seed = 1337; // RNG seed
            int octaves = 8; // Level of details
            float scale = 0.001f; // Bigger scale = Less Terrain details
            int lacunarity = 2; // Value must be > 1
            float persistance = 1; // Value must be 0-1

            for (int i = 1; i < 9; i++) { // i = octaves
                for (int j = 1; j < 17; j++) // J = lacunarity
                {
                    scale = 0.001f * j;
                    // Generate the initial noise map using Simplex Noise
                    noiseValues = NoiseMapLayering.getNoiseData(width, height, seed, 8, scale, 2, persistance);

                    // Construct the graph and link all the nodes within
                    terrainGraph graph = new terrainGraph(height, width, noiseValues);

                    StringBuilder filename = new StringBuilder("../../../OutputImages/TerrainGenTesting/O8 ");
                    //filename.Append(j.ToString());
                    filename.Append(" S");
                    filename.Append(scale.ToString());
                    filename.Append(".bmp");
                    graph.saveImageOfGraph(filename.ToString());
                    Console.WriteLine("Test: " + (j));
                }
            }
        }

        public static List<Agent> generateInitialPopulation(terrainGraph graph, int populationSize)
        {
            int width = 500;
            int height = width;


            int totalAgentTime = 0;
            int nonZeroAgents = 0;
            int agentsReachedTargetCount = 0;
            int agentCount = 0;
            int prevAgentCount = 0;
            float totalPath = 0;

            DataNode graphStartNode = graph.terrainNodes[0, 0];
            DataNode graphTargetNode = graph.terrainNodes[498, 498];

            List<Agent> population = new List<Agent>();

            string line = "";

            //Run the simulation 100 times
            while (nonZeroAgents < populationSize)
            {
                // Start Stopwatch
                // measureing time taken to perform blind search
                var agentWatch = System.Diagnostics.Stopwatch.StartNew();

                // Generate new Agent for testing
                Agent testAgent = new Agent(graph.terrainNodes[0, 0], graph.terrainNodes[width - 2, height - 2], graph.getEdgeNode());
                // Generate Path for new agent
                testAgent.agentPath = src.GAMethods.BlindSearch.performBlindSearch(testAgent);

                // Stop stopwatch
                agentWatch.Stop();

                // Report on the cost of the path
                if (testAgent.agentPath.getPathCost() > 100)
                {
                    population.Add(testAgent);
                    nonZeroAgents++;

                    totalPath += testAgent.agentPath.getPathCost();

                    // Add the agent's time to total for averaging
                    // Only add agents that didn't instantly die to total
                    var elapsedMS = agentWatch.ElapsedMilliseconds;
                    if ((int)elapsedMS > 10)
                    {
                        totalAgentTime += (int)elapsedMS;

                    }

                }

                //Progress output
                string backup = new string('\b', line.Length);
                Console.Write(backup);
                line = string.Format("{0} Agents", nonZeroAgents);
                Console.Write(line);
            }

            // Calculate the distance from the target for each agent
            // Then paint the pathway for each of the agents
            for (int i = 0; i < population.Count; i++)
            {
                population[i].findDistanceToTarget();

                if (population[i].distanceFromTarget > 0)
                {
                    population[i].agentPath.paintPathway(0);
                    
                } else
                {
                    population[i].agentPath.paintPathway(600);
                    agentsReachedTargetCount += 1;
                }
            }

            Console.Write("\n");
            totalPath /= nonZeroAgents;

            Console.WriteLine("Average Path Length: " + totalPath);
            Console.WriteLine("Average per agent: " + totalAgentTime / nonZeroAgents);
            Console.WriteLine("Agents that reached Target: " + agentsReachedTargetCount);
            Console.WriteLine("Total Computing Time: " + totalAgentTime / 1000 + " seconds");

            return population;
        }
    }

}

