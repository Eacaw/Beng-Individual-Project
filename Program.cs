using System;
using System.Collections.Generic;
using BEng_Individual_Project.src;
using BEng_Individual_Project.GA_Methods;
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
            int seed = 1337; // RNG seed
            int octaves = 8; // Level of details
            float scale = 0.005f; // Bigger scale = Less Terrain details
            int lacunarity = 2; // Value must be > 1
            float persistance = 1; // Value must be 0-1

            // GA Defining Variables
            int populationSize = 5000;

            // Generate the initial noise map using Simplex Noise
            noiseValues = NoiseMapLayering.getNoiseData(width, height, seed, octaves, scale, lacunarity, persistance);

            // Construct the graph and link all the nodes within
            terrainGraph graph = new terrainGraph(height, width, noiseValues);

            // Create the initial Population
            List<Agent> population = new List<Agent>();

            // Output image of graph without paths
            //graph.saveImageOfGraph("../../../OutputImages/TerrainImage.bmp");

            // Bring the noise values up so that a black pathway can easily be seen
            graph.increaseGrayscaleMapping();
            
            int nonZeroAgents = 0;
            int agentsReachedTargetCount = 0;

            //src.Path shortestPath = AStarMethod.runAStar(graph.terrainNodes[1, 1], graph.terrainNodes[498, 498],graph);

            //shortestPath.paintPathway(0);

            string line = "";


            // Generate Initial Population and perform intial blind search step
            for (int i = 0; i < populationSize; i++)
            {
                float pathLength = 0;
                int targetCheck = 0;
                Agent newAgent = new Agent(graph.terrainNodes[0, 0], graph.terrainNodes[height -2, width -2], graph.getEdgeNode());
                // Generate a new agent until reasonable path length is achieved
                while (pathLength < 250)
                {
                    //newAgent = new Agent(graph.terrainNodes[0, 0], graph.terrainNodes[205, 498], graph.getEdgeNode());
                    targetCheck = newAgent.performBlindSearch();
                    pathLength = newAgent.pathCost;
                    if (pathLength < 150)
                    {
                        Console.WriteLine("Repeat");
                    }
                }

                population.Add(newAgent);

                nonZeroAgents++;

                if (targetCheck == 255)
                {
                    agentsReachedTargetCount++;
                }

                // Paint the agent's paths
                population[i].agentPath.paintPathway(targetCheck);

                //Progress output
                string backup = new string('\b', line.Length);
                Console.Write(backup);
                line = string.Format("{0} Agents", nonZeroAgents);
                Console.Write(line);
            }

            // Find the minimum and maximum path length for mapping
            float maxPath = 0;
            float minPath = float.MaxValue;
            for (int i = 0; i < population.Count; i++)
            {
                if (population[i].pathCost > maxPath)
                {
                    maxPath = population[i].pathCost;
                }
                if (population[i].pathCost < minPath)
                {
                    minPath = population[i].pathCost;
                }
            }

            // Calculate the fitness for each agent
            for (int i = 0; i < population.Count; i++)
            {
                population[i].fitnessScore = Fitness.calculateWeightedFitness(population[i], 1, 0, maxPath, minPath);
            }

            // Order the population by fitness (Hi-Lo)
            var agentWatch = System.Diagnostics.Stopwatch.StartNew();
            population = population.OrderBy(o => o.fitnessScore).ToList();
            agentWatch.Stop();
            var elapsedMS = agentWatch.ElapsedMilliseconds;

            Console.Write("\n");
            Console.WriteLine("sort time: " + elapsedMS);
            //for (int i = 0; i < population.Count; i++)
            //{
            //    Console.WriteLine("Agent: " + i + "\t fitness: " + population[i].fitnessScore + "\t path: " + population[i].pathCost);
            //}
            Console.WriteLine("Agents reached target: " + agentsReachedTargetCount);




            //// Generate output image including paths
            graph.saveImageOfGraph("../../../OutputImages/PathwayTesting.bmp");
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

            // GA Defining Variables
            int populationSize = 10000;

            //for (int i = 1; i < 9; i++) { // i = octaves
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
            //}
        }



        public static void testBlindSearch(terrainGraph graph)
        {
            int width = 500;
            int height = width;


            int totalAgentTime = 0;
            int nonZeroAgents = 0;
            int agentsReachedTargetCount = 0;
            int agentCount = 0;
            int prevAgentCount = 0;
            float totalPath = 0;

            //Run the simulation 100 times
            while (nonZeroAgents < 1000)
            {
                // Start Stopwatch
                var agentWatch = System.Diagnostics.Stopwatch.StartNew();

                // Generate new Agent for testing
                Agent testAgent = new Agent(graph.terrainNodes[0, 0], graph.terrainNodes[width - 1, height - 1], graph.getEdgeNode());

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

                    totalPath += testAgent.agentPath.getPathCost();

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



                }
                agentCount++;
            }
            // Output average time per agent
            Console.Write("\n");
            totalPath /= nonZeroAgents;

            Console.WriteLine("Average Path Length: " + totalPath);
            Console.WriteLine("Average per agent: " + totalAgentTime / nonZeroAgents);
            Console.WriteLine("Agents that reached Target: " + agentsReachedTargetCount);
            Console.WriteLine("Total Computing Time: " + totalAgentTime / 1000 + " seconds");
        }
    }

}

