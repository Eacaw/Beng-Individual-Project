﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BEng_Individual_Project.src;
using BEng_Individual_Project.lib;

namespace BEng_Individual_Project
{
    class Program
    {

        static void Main(string[] args)
        {

            float[,] noiseValues;

            int width = 1000;
            int height = width; // No idea why I get an out of bounds if the map isn't square. WTF! 
            int seed = 1337;
            int octaves = 8;
            float scale = 0.005f;
            int lacunarity = 1;
            float persistance = 1;

            noiseValues = NoiseMapLayering.getNoiseData(width, height, seed, octaves, scale, lacunarity, persistance) ;

            terrainGraph graph = new terrainGraph(height, width, noiseValues);

            //for (int i = 0; i < 5; i++)
            //{
                Agent testAgent = new Agent(graph.terrainNodes[0, 0], graph.terrainNodes[50, 50], graph.getEdgeNode());
                testAgent.performBlindSearch();
                testAgent.agentPath.printPathway();
                Console.WriteLine("------");
            //}

            graph.increaseGrayscaleMapping();
            testAgent.agentPath.paintPathway();
            graph.saveImageOfGraph("../../../PathwayTesting");

            


        }
    }

}

