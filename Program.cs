using System;
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

            float[,] noiseValues;// = new float[1000,1000];

            int width = 240;
            int height = 240;
            int seed = 1337;
            int octaves = 3;
            float scale = 0.01f;
            int lacunarity = 2;
            float persistance = 1;

            //noiseValues = NoiseMapLayering.getNoiseData(width, height, seed, octaves, scale, lacunarity, persistance) ;

                noiseValues = NoiseMapLayering.getNoiseData(width, height, seed, octaves, scale, lacunarity, persistance);
            

        }
    }

}

