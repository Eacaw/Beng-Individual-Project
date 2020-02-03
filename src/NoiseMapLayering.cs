using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace BEng_Individual_Project.src
{
    public static class NoiseMapLayering
    {

        private static float minimum = float.MaxValue, maximum = 0;

        /**
         * Method to return the noise data to the caller
         * Will produce layered noise maps to specified level of detail.
         */
        public static float[,] getNoiseData(int width, int height, int seed, int octaves, float scale, int lacunarity, float perisitance)
        {
            float[,] noiseValues;
            

            SimplexNoise.Noise.Seed = seed;
            //Repeat this for the total number of octaves requested
            for (int octaveCount = 0; octaveCount < octaves; octaveCount++)
            {
                // Update the scale factor to change the frequency of the noise for this layer
                float scale = scale * Math.Pow(lacunarity, octaveCount);
                // Generate a noise map with set scale factor
                noiseValues = generateNewNoiseMap(width, height, scale);
                // Find minimum and maximum values for normalising
                findMinMax(noiseValues, width, height);
            }


        }


        private static float[,] generateNewNoiseMap(int length, int width, float scale)
        {
            float[,] noiseValue = SimplexNoise.Noise.Calc2D(length, width, scale);
            return noiseValue;
        }

        private static void findMinMax(float[,] noiseMap, int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (noiseMap[i, j] > maximum)
                    {
                        maximum = noiseMap[i, j];
                    }

                    if (noiseMap[i, j] < minimum)
                    {
                        minimum = noiseMap[i, j];
                    }
                }
            }
        }

        /**
         * Method used to combine two noise maps for layering
         */
        private static float[,] sumNoiseMaps(float[,] noiseMap1, float[,] noiseMap2, int height, int width, int divisor)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    noiseMap1[i, j] += noiseMap2[i, j] / divisor;
                }
            }

            return noiseMap1;
        }

        /**
         * Generates byte array for grayscale image data
         */
        private static byte[] genetateImageData(float[,] noiseValues, int height, int width)
        {
            byte[] imageDataBytes = new byte[noiseValues.Length];
            int write = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    imageDataBytes[write++] = (byte)(normaliseFloat(noiseValues[i, j]) * 255);
                }
            }

            return imageDataBytes;
        }

        /**
         * Normalises the floating point values
         * between 0 and 1.
         */
        private static float normaliseFloat(float value)
        {
            return (value - minimum) / (maximum - minimum);
        }


    }
}
