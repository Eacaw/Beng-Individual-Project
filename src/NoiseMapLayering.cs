using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using BEng_Individual_Project.lib;

namespace BEng_Individual_Project.src
{
    public static class NoiseMapLayering
    {
         /**
         * Method to return the noise data to the caller
         * Will produce layered noise maps to specified level of detail.
         */
        public static float[,] getNoiseData(int width, int height, int seed, int octaves, float scale, float lacunarity, float persistance)
        {
            float[,] noiseValues = new float[width,height];

            SimplexNoise.Noise.Seed = seed;
            //Repeat this for the total number of octaves requested
            for (int octaveCount = 0; octaveCount < octaves; octaveCount++)
            {
                // Update the scale factor to change the frequency of the noise for this layer
                float scaler = scale * (float)Math.Pow(lacunarity, octaveCount);

                // Generate a noise map with set scale factor
                float[,] noiseLayer = generateNewNoiseMap(width, height, scaler);

                // Sum the layers together with requested Persistance Value
                float divisor = (persistance * (float) Math.Pow(2, octaveCount));

                //Console.WriteLine("Divisor: " + divisor);

                // Sum the current values with the newly generated values
                noiseValues = sumNoiseMaps(noiseValues, noiseLayer, height, width, divisor);

            }
            return noiseValues;
        }


        /**
         * Generates a new noise map from the simplex
         * noise library based on passed parameters
         */
        public static float[,] generateNewNoiseMap(int height, int width, float scale)
        {
            float[,] noiseValue = SimplexNoise.Noise.Calc2D(width, height, scale);
            return noiseValue;
        }

        /**
         * Method used to combine two noise maps for layering
         */
        private static float[,] sumNoiseMaps(float[,] noiseMap1, float[,] noiseMap2, int height, int width, float divisor)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    noiseMap1[i, j] += (noiseMap2[i, j] / divisor);
                }
            }

            return noiseMap1;
        }

    }
}
