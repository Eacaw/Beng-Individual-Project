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

            GALoopTest();

        }


        private static void GALoopTest()
        {
            // GA Defining Variables
            int populationSize = 1000;
            int topScore = populationSize - 1;
            int elitismPercentage = 25;
            int mutationPercentage = 25;

            int terrainDimensions = 250; // Terrain generated will be square
            int targetNodeCoords = terrainDimensions - 2; // Target always two nodes in from bottom left corner

            // Construct the graph and link all the nodes within
            terrainGraph graph = new terrainGraph(terrainDimensions);

            DataNode graphStartNode = graph.terrainNodes[0, 0]; // Starting node is top left corner
            DataNode graphTargetNode = graph.terrainNodes[targetNodeCoords, targetNodeCoords];

            // Max distance used for mapping the values in the fitness function
            float maxDistance = numericalUtilities.getDistanceBetweenNodes(graphStartNode, graphTargetNode);

            // Add pre-defined obstacles and hostile nodes
            graph = constructTestTerrain(graph);

            // Calculate total risk value from hostile nodes
            graph.calculateMaximumRisk();

            // Generate the initial Population
            List<Agent> population = generateInitialPopulation(graph, populationSize, maxDistance);

            // Paint the pathway of the initial population
            for (int il = 0; il < population.Count; il++)
            {
                population[il].agentPath.paintPathway(0);
            }

            graph.saveImageOfGraph("../../../OutputImages/GATesting/GATest-initialPopulation.bmp");

            int i = 0;
            while (i < 10001) // Max generations cut off at 10k
            {
                // Clear the mating pool
                matingPool breedingPool = new matingPool();

                // Selection Method
                breedingPool.elitismSelection(population, elitismPercentage); // Elitism only used as other methods too inefficient

                // Clear the current population list
                population = new List<Agent>();

                // Produce the next generation for the population
                for (int j = 0; j < (int)(populationSize); j++) // requires the pool to be integer divisible by 0.1
                {
                    // Selection
                    matingPartners parentSelection = Selection.tournamentSelectionPool(breedingPool, 3);

                    //Crossover
                    Agent childAgent = Crossover.performCrossover(parentSelection);

                    // Mutation
                    childAgent = Mutation.mutatePathWithoutLimit(childAgent, mutationPercentage);

                    // Calculate the child's values for fitness calculations
                    childAgent.pathCost = childAgent.agentPath.getPathCost();
                    childAgent.riskValue = childAgent.agentPath.getPathRisk();
                    childAgent.agentPath.removeFinalTwoNodes();
                    childAgent.findDistanceToTarget();

                    // Add new child to population
                    population.Add(childAgent);

                }

                population = sortPopulationByFitness(population, maxDistance, graph.maxRisk);

                Console.WriteLine("Generation: " + i + "\t fit: " + population[topScore].fitnessScore +
                                    "\t path: " + population[topScore].pathCost + "\t Dist: " +
                                        population[topScore].distanceFromTarget + "\t Nodes: " + population[topScore].agentPath.getNodeCount() +
                                        "\t Risk: " + population[topScore].riskValue);


                // Every 100 Generations paint the top 5% of the population to show variety
                if (i % 100 == 0)
                {
                    for (int paint = topScore; paint > (int)(populationSize / 20); paint--)
                    {
                        population[paint].agentPath.paintPathway(0);
                        graph.saveHeatmapImageOfGraph("../../../OutputImages/GATesting/Heatmap" + i + ".bmp");
                    }
                }
                // Otherwise just output the top scoring agent from the generation
                else
                {
                    population[topScore].agentPath.paintPathway(0);
                }

                    graph.saveImageOfGraph("../../../OutputImages/GATesting/Gen" + i + ".bmp");

                // Reset the paint values in the graph to avoid repeating painting paths
                graph.resetNodeHeightValues();

                
                i++;

            }
        }


        private static List<Agent> generateInitialPopulation(terrainGraph graph, int populationSize, float maxDistance)
        {
            // Create the initial Population
            List<Agent> population = new List<Agent>();

            int targetCoords = graph.height - 2;

            DataNode graphStartNode = graph.terrainNodes[0, 0];
            DataNode graphTargetNode = graph.terrainNodes[targetCoords, targetCoords];

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
                //while (pathCostCheck < 1)
                //{
                newAgent = new Agent(graphStartNode, graphTargetNode, graph.getEdgeNode());
                newAgent.agentPath = BlindSearch.performBlindSearch(newAgent, 0);
                newAgent.pathCost = newAgent.agentPath.getPathCost();
                newAgent.riskValue = newAgent.agentPath.getPathRisk();
                pathCostCheck = newAgent.pathCost;
                //}

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
                terrainGraph graph = new terrainGraph(100);

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
                testAgent.agentPath = src.GAMethods.BlindSearch.performBlindSearch(testAgent, 0);

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

        private static terrainGraph constructTestTerrain(terrainGraph graph)
        {
            Hostile centerHostile = new Hostile(75, 75, 20, 0, 15);
            Hostile centerHostile2 = new Hostile(75, 175, 20, 0, 15);
            Hostile centerHostile3 = new Hostile(175, 175, 20, 0, 15);
            Hostile centerHostile4 = new Hostile(175, 75, 20, 0, 15);
            Hostile centerHostile5 = new Hostile(125, 75, 20, 0, 15);
            Hostile centerHostile6 = new Hostile(125, 175, 20, 0, 15);
            Hostile centerHostile7 = new Hostile(75, 125, 20, 0, 15);
            Hostile centerHostile8 = new Hostile(175, 125, 20, 0, 15);
            //Hostile centerHostile9 = new Hostile(50, 150, 10, 0, 10);
            graph.addHostileToGraph(centerHostile);
            graph.addHostileToGraph(centerHostile2);
            graph.addHostileToGraph(centerHostile3);
            graph.addHostileToGraph(centerHostile4);
            graph.addHostileToGraph(centerHostile5);
            graph.addHostileToGraph(centerHostile6);
            graph.addHostileToGraph(centerHostile7);
            graph.addHostileToGraph(centerHostile8);
            //graph.addHostileToGraph(centerHostile9);

            graph = Obstacle.addObstacletoGraphAtCoords(graph, 25, 25, 50, 100, 1);
            graph = Obstacle.addObstacletoGraphAtCoords(graph, 50, 25, 100, 50, 1);
            graph = Obstacle.addObstacletoGraphAtCoords(graph, 150, 25, 200, 50, 1);
            graph = Obstacle.addObstacletoGraphAtCoords(graph, 200, 25, 225, 100, 1);
            graph = Obstacle.addObstacletoGraphAtCoords(graph, 25, 150, 50, 225, 1);
            graph = Obstacle.addObstacletoGraphAtCoords(graph, 50, 200, 100, 225, 1);
            graph = Obstacle.addObstacletoGraphAtCoords(graph, 150, 200, 200, 225, 1);
            graph = Obstacle.addObstacletoGraphAtCoords(graph, 200, 150, 225, 225, 1);
            graph = Obstacle.addObstacletoGraphAtCoords(graph, 100, 100, 150, 150, 1);

            return graph;
        }


    }

}

