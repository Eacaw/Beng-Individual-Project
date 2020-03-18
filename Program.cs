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

            for(int i = 0; i < 20; i++)
            {
                graph = Obstacle.addObstacletoGraph(graph, 25, 25);
            }

            //for(int i = 100; i < 200; i++)
            //{
            //    for ( int j = 100; j < 200; j++)
            //    {
            //        graph.terrainNodes[i, j].heightValue = -2;
            //    }
            //}

            //for (int i = 300; i < 400; i++)
            //{
            //    for (int j = 300; j < 400; j++)
            //    {
            //        graph.terrainNodes[i, j].heightValue = -2;
            //    }
            //}

            DataNode graphStartNode = graph.terrainNodes[0, 0];
            DataNode graphTargetNode = graph.terrainNodes[498, 498];

            generateInitialPopulation(graph, 100);


            //Agent testAgentOne = new Agent(graphStartNode, graphTargetNode, graph.getEdgeNode());
            //Agent testAgentTwo = new Agent(graphStartNode, graphTargetNode, graph.getEdgeNode());

            //testAgentOne.agentPath = src.GAMethods.BlindSearch.performBlindSearch(testAgentOne);
            //testAgentTwo.agentPath = src.GAMethods.BlindSearch.performBlindSearch(testAgentTwo);

            //testAgentOne.agentPath.paintPathway(0);
            //testAgentTwo.agentPath.paintPathway(0);
            //float remainingDistance = float.MaxValue;
            //Path nodeTestPath = new Path();
            //int count = 0;
            ////while (remainingDistance != 0)
            ////{
            //    nodeTestPath = src.GAMethods.BlindSearch.performBlindSearch(graph.terrainNodes[450, 50], graph.terrainNodes[50, 450]);
            //    remainingDistance = numericalUtilities.getDistanceBetweenNodes(nodeTestPath.getFinalNode(), graph.terrainNodes[50, 450]);
            //    count++;
            ////}
            //nodeTestPath.paintPathway(500);
            //if (remainingDistance == 0)
            //{
            //    Console.WriteLine("Target Reached");
            //}
            //else
            //{
            //    Console.WriteLine("Distance: " + remainingDistance);
            //}

            //Console.WriteLine("Blind Search Iterations Taken: " + count);

            //Console.WriteLine("Agent One: Node Count: " + testAgentOne.agentPath.getNodeCount());
            //Console.WriteLine("Agent Two: Node Count: " + testAgentTwo.agentPath.getNodeCount());

            //matingPartners testMatingSame = new matingPartners(testAgentOne, testAgentOne);
            //matingPartners testMatingDifferent = new matingPartners(testAgentOne, testAgentTwo);

            //Console.WriteLine("Same - mutualNodes: " + Crossover.countMatchingNodes(testMatingSame).Count);
            //Console.WriteLine("Diff - mutualNodes: " + Crossover.countMatchingNodes(testMatingDifferent).Count);

            graph.saveImageOfGraph("../../../OutputImages/ObstacleTesting3.bmp");

        }



        //private static void generateInitialPopulationTest()
        //{
            

        //    // Create the initial Population
        //    List<Agent> population = new List<Agent>();

        //    // Output image of graph without paths
        //    //graph.saveImageOfGraph("../../../OutputImages/TerrainImage.bmp");

        //    // Bring the noise values up so that a black pathway can easily be seen
        //    graph.increaseGrayscaleMapping();

        //    int nonZeroAgents = 0;
        //    int agentsReachedTargetCount = 0;

        //    //src.Path shortestPath = AStarMethod.runAStar(graph.terrainNodes[1, 1], graph.terrainNodes[498, 498],graph);

        //    //shortestPath.paintPathway(0);

        //    string line = "";


        //    // Generate Initial Population and perform intial blind search step
        //    for (int i = 0; i < populationSize; i++)
        //    {
        //        float pathLength = 0;
        //        int targetCheck = 0;
        //        Agent newAgent = new Agent(graphStartNode, graphTargetNode, graph.getEdgeNode());
        //        // Generate a new agent until reasonable path length is achieved
        //        while (pathLength < 1)
        //        {
        //            newAgent = new Agent(graphStartNode, graphTargetNode, graph.getEdgeNode());
        //            targetCheck = newAgent.performBlindSearch();
        //            pathLength = newAgent.pathCost;
        //        }

        //        if (targetCheck == 255)
        //        {
        //            agentsReachedTargetCount++;
        //            newAgent.hitTarget = true;
        //        }

        //        population.Add(newAgent);

        //        nonZeroAgents++;

        //        // Paint the agent's paths
        //        population[i].agentPath.paintPathway(targetCheck);

        //        //Progress output
        //        string backup = new string('\b', line.Length);
        //        Console.Write(backup);
        //        line = string.Format("{0} Agents", nonZeroAgents);
        //        Console.Write(line);
        //    }

        //    // Find the minimum and maximum path length for mapping
        //    float maxPath = 0;
        //    float minPath = float.MaxValue;
        //    for (int i = 0; i < population.Count; i++)
        //    {
        //        if (population[i].pathCost > maxPath)
        //        {
        //            maxPath = population[i].pathCost;
        //        }
        //        if (population[i].pathCost < minPath)
        //        {
        //            minPath = population[i].pathCost;
        //        }
        //    }

        //    //TODO: Create References to starting and target nodes at the top level
        //    float maxDistance = numericalUtilities.getDistanceBetweenNodes(graph.terrainNodes[0, 0], graph.terrainNodes[498, 498]);
        //    Console.WriteLine("Max Dist: " + maxDistance);

        //    // Calculate the fitness for each agent
        //    for (int i = 0; i < population.Count; i++)
        //    {
        //        population[i].fitnessScore = Fitness.calculateWeightedFitness(population[i], 1, 0, maxPath, minPath, maxDistance);
        //    }

        //    // Order the population by fitness (Hi-Lo)
        //    var agentWatch = System.Diagnostics.Stopwatch.StartNew();
        //    population = population.OrderBy(o => o.fitnessScore).ToList();
        //    agentWatch.Stop();
        //    var elapsedMS = agentWatch.ElapsedMilliseconds;

        //    Console.Write("\n");
        //    Console.WriteLine("sort time: " + elapsedMS);
        //    for (int i = 0; i < population.Count; i++)
        //    {
        //        string hitTarget = "Nope";
        //        if (population[i].hitTarget)
        //        {
        //            hitTarget = "Yep";
        //        }
        //        Console.WriteLine("Agent: " + i + "\t fit: " + population[i].fitnessScore + "\t path: " + population[i].pathCost + "\t Dist: " + population[i].distanceFromTarget + "\t Hit: " + hitTarget);
        //    }
        //    Console.WriteLine("Agents reached target: " + agentsReachedTargetCount);




        //    //// Generate output image including paths
        //    graph.saveImageOfGraph("../../../OutputImages/PathwayTesting.bmp");
        //}


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

