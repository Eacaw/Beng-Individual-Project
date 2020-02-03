using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BEng_Individual_Project
{
    class Program
    {

        private static int count = 0;

        static void Main(string[] args)
        {
            SimplexNoise.Noise.Seed = 8035;
            int length = 1000, width = 1000;
            float scale = 0.01f;
            float[,] noiseValues;
            float[,] noiseValues2;
            float[,] noiseValues3;

            noiseValues = generateNewNoiseMap(length, width, scale);
            noiseValues2 = generateNewNoiseMap(length, width, scale*2);
            noiseValues3 = generateNewNoiseMap(length, width, scale*4);

            }

        
        

        

        

    }

}

