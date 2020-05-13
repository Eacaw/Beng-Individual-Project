using System;
using System.Collections.Generic;
using BEng_Individual_Project.src;
using BEng_Individual_Project.src.Utilities;
using System.Linq;
using System.Text;
using BEng_Individual_Project.src.GAMethods;
using BEng_Individual_Project.GA_Methods;

namespace BEng_Individual_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            // GA Defining Variables
            int populationSize = 1000;
            int topScore = populationSize - 1;

            int mutationPercentage = 2;

            // Construct the graph and link all the nodes within
            terrainGraph graph = new terrainGraph(0,0);

            //graph.increaseGrayscaleMapping();

            DataNode graphStartNode = graph.terrainNodes[0, 0];
            DataNode graphTargetNode = graph.terrainNodes[498, 498];

            // Max distance used for mapping the values in the fitness function
            float maxDistance = numericalUtilities.getDistanceBetweenNodes(graphStartNode, graphTargetNode);

            for(int x = 1; x < 8; x++)
            {
                for(int y = 1; y < 8; y++)
                {
                    int[] hostilePos = new int[2];
                    hostilePos[0] = 60 * x;
                    hostilePos[1] = 60 * y;
                    Hostile centerHostile = new Hostile(hostilePos[0], hostilePos[1], 200, 0, 25);
                    graph.addHostileToGraph(centerHostile, hostilePos);
                }
            }

            graph.calculateMaximumRisk();

            for (int testNo = 0; testNo < 10; testNo++)
            {
                Console.WriteLine("------------------Test: " + testNo + "------------------");

                // Generate the initial Population
                List<Agent> population = generateInitialPopulation(graph, populationSize, maxDistance);

                for (int il = 0; il < population.Count; il++)
                {
                    population[il].agentPath.paintPathway(0);
                }

                graph.saveImageOfGraph("../../../OutputImages/GATesting/GATest-initialPopulation.bmp");

                matingPool breedingPool;
                // Do 100 generations test
                //for (int i = 0; i < 100; i++)
                int i = 0;
                while (population[topScore].riskValue > 0)
                {
                    // Clear the mating pool
                    breedingPool = new matingPool();

                    // Selection Method
                    breedingPool.elitismSelection(population, 50);
                    //breedingPool.linearRouletteSelectionFitness(population);
                    //breedingPool.linearRouletteSelectionRank(population);

                    // Clear the current population list
                    population = new List<Agent>();

                    graph.resetNodeHeightValues();

                    // Produce the next generation for the population
                    for (int j = 0; j < populationSize; j++)
                    {
                        matingPartners parentSelection = Selection.randomSelectionFromPool(breedingPool);

                        Agent childAgent = Crossover.performCrossover(parentSelection);

                        childAgent = Mutation.mutatePathWithoutLimit(childAgent, mutationPercentage);

                        childAgent.pathCost = childAgent.agentPath.getPathCost();

                        childAgent.riskValue = childAgent.agentPath.getPathRisk();

                        childAgent.agentPath.removeFinalTwoNodes();

                        childAgent.findDistanceToTarget();

                        population.Add(childAgent);

                    }

                    population = sortPopulationByFitness(population, maxDistance, graph.maxRisk);



                    population[topScore].pathCost = population[topScore].agentPath.getPathCost();

                    Console.WriteLine("Generation: " + i + "\t fit: " + population[topScore].fitnessScore +
                                        "\t path: " + population[topScore].pathCost + "\t Dist: " +
                                            population[topScore].distanceFromTarget + "\t Nodes: " + population[topScore].agentPath.getNodeCount() +
                                            "\t Risk: " + population[topScore].riskValue);

                    population[topScore].agentPath.paintPathway(0);

                    graph.saveImageOfGraph("../../../OutputImages/GATesting/New/Gen" + i + ".bmp");

                    graph.resetNodeHeightValues();

                    i++;

                }

                graph.saveImageOfGraph("../../../OutputImages/GATesting/Test" + testNo +".bmp");

            }


        }



        private static List<Agent> generateInitialPopulation(terrainGraph graph, int populationSize, float maxDistance)
        {
            // Create the initial Population
            List<Agent> population = new List<Agent>();

            DataNode graphStartNode = graph.terrainNodes[0, 0];
            DataNode graphTargetNode = graph.terrainNodes[498, 498];

            // Bring the noise values up so that a black pathway can easily be seen
            //graph.increaseGrayscaleMapping();

            int nonZeroAgents = 0;
            int agentsReachedTargetCount = 0;

            string line = "";


            // Generate Initial Population and perform intial blind search step
            for (int i = 0; i < populationSize; i++)
            {
                float pathCostCheck = 0;
                int targetCheck = 0;
                Agent newAgent = new Agent(graphStartNode, graphTargetNode, graph.getEdgeNode());
                // Generate a new agent until reasonable path length is achieved
                while (pathCostCheck < 1)
                {
                    newAgent = new Agent(graphStartNode, graphTargetNode, graph.getEdgeNode());
                    newAgent.agentPath = BlindSearch.performBlindSearch(newAgent);
                    newAgent.pathCost = newAgent.agentPath.getPathCost();
                    newAgent.riskValue = newAgent.agentPath.getPathRisk();
                    pathCostCheck = newAgent.pathCost;
                }

                newAgent.findDistanceToTarget();

                if (newAgent.distanceFromTarget == 0)
                {
                    agentsReachedTargetCount++;
                    newAgent.hitTarget = true;
                }

                population.Add(newAgent);

                nonZeroAgents++;

                // Paint the agent's paths
                //if (newAgent.hitTarget)
                //{
                //    newAgent.agentPath.paintPathway(255);
                //} else
                //{
                //    newAgent.agentPath.paintPathway(0);
                //}

                //Progress output
                string backup = new string('\b', line.Length);
                Console.Write(backup);
                line = string.Format("{0} Agents", nonZeroAgents);
                Console.Write(line);
            }


            population = sortPopulationByFitness(population, maxDistance, graph.maxRisk);

            // Output the top 10 agents details
            Console.Write("\n");
            for (int i = population.Count - 10; i < population.Count; i++)
            {
                string hitTarget = "Nope";
                if (population[i].hitTarget)
                {
                    hitTarget = "Yep";
                }
                Console.WriteLine("Agent: " + i + "\t fit: " + population[i].fitnessScore + "\t path: " + population[i].pathCost + "\t Dist: " + population[i].distanceFromTarget + "\t Hit: " + hitTarget + "\t Risk: " + population[i].riskValue);
            }
            Console.WriteLine("Agents reached target: " + agentsReachedTargetCount);

            //// Generate output image including paths
            //graph.saveImageOfGraph("../../../OutputImages/InitialPopulationTesting.bmp");

            return population;
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
                    terrainGraph graph = new terrainGraph(0,0);

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

        public static List<Agent> testBlindSearch(terrainGraph graph, int populationSize)
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
            //for (int i = 0; i < population.Count; i++)
            //{
            //    population[i].findDistanceToTarget();

            //    if (population[i].distanceFromTarget > 0)
            //    {
            //        population[i].agentPath.paintPathway(0);
                    
            //    } else
            //    {
            //        population[i].agentPath.paintPathway(600);
            //        agentsReachedTargetCount += 1;
            //    }
            //}

            Console.Write("\n");
            totalPath /= nonZeroAgents;

            Console.WriteLine("Average Path Length: " + totalPath);
            Console.WriteLine("Average per agent: " + totalAgentTime / nonZeroAgents);
            Console.WriteLine("Agents that reached Target: " + agentsReachedTargetCount);
            Console.WriteLine("Total Computing Time: " + totalAgentTime / 1000 + " seconds");

            return population;
        }

        private static List<Agent> sortPopulationByFitness(List<Agent> population, float maxDistance, float maxRisk)
        {

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
                population[i].fitnessScore = Fitness.calculateWeightedFitness(population[i], 1, 2, maxPath, minPath, maxDistance, maxRisk);
            }

            // Order the population by fitness (Hi-Lo)
            population = population.OrderBy(o => o.fitnessScore).ToList();

            return population;
        }

        private static terrainGraph addObstaclesToGraph(int obstacleCount, terrainGraph graph)
        {
            obstacleCount++;
            //Add obstacles to the terrain
            for (int i = 0; i < obstacleCount; i++)
            {
                graph = Obstacle.addObstacletoGraph(graph, 25, 25);
            }
            return graph;
        }
    }

}

