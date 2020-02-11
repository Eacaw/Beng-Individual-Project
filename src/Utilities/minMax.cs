using System;
using System.Collections.Generic;
using System.Text;

namespace BEng_Individual_Project.src.Utilities
{
    static class minMax
    {

        public  static float[] findMinMax(float[,] noiseMap, int height, int width)
        {

            float minimum = float.MaxValue, maximum = 0;
            float[] output = new float[2];

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

            output[0] = minimum;
            output[1] = maximum;

            return output;
        }

    }
}
