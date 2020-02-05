using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BEng_Individual_Project.src;

namespace BEng_Individual_Project
{
    class Program
    {

        static void Main(string[] args)
        {

            float[,] noiseValues;

            noiseValues = NoiseMapLayering.getNoiseData(1000, 1000, 1444, 3, 0.01f, 2, 0.5f);

        }
    }

}

