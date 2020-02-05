using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using BEng_Individual_Project.lib;

namespace BEng_Individual_Project.src
{
    public static class NoiseMapLayering
    {

        private static float minimum = float.MaxValue, maximum = 0;

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

                //findMinMax(noiseValues, height, width);

                //TODO: Remove this save image line, it's here for debugging! 
                //SaveBitmapImageFile.SaveBitmap("../../../OutputImage " + octaveCount, width, height, generateImageData(noiseValues, height, width));

            }
            // Find minimum and maximum values for normalising
            findMinMax(noiseValues, height, width);
            
            //TODO: Remove this save image line, it's here for debugging! 
            StringBuilder fileName = new StringBuilder("../../../");
            fileName.Append(lacunarity.ToString().Replace(".","p"));
            fileName.Append(",");
            fileName.Append(scale.ToString().Replace(".", "p"));
            Console.WriteLine(fileName.ToString());
            Console.WriteLine("min: " + minimum + " max: " + maximum);
            SaveBitmapImageFile.SaveBitmap(fileName.ToString(), width, height, generateImageData(noiseValues, height, width)) ;
            minimum = float.MaxValue;
            maximum = 0;
            return noiseValues;

        }


        public static float[,] generateNewNoiseMap(int height, int width, float scale)
        {
            float[,] noiseValue = SimplexNoise.Noise.Calc2D(width, height, scale);
            return noiseValue;
        }

        /**
         * Find the minimum and maximum values used for normalising the values
         */
        private static void findMinMax(float[,] noiseMap, int height, int width)
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

        /**
         * Generates byte array for grayscale image data
         */
        public static byte[] generateImageData(float[,] noiseValues, int height, int width)
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
            return ((value - minimum) / (maximum - minimum));
        }


    }
}
