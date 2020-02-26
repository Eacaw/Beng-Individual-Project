using System;
using System.Collections.Generic;
using System.Text;

namespace BEng_Individual_Project.src.Utilities
{
    static class numericalUtilities
    {

        // Finds the minimum and maximum values contained withing a noise map
        public static float[] findMinMax(float[,] noiseMap, int height, int width)
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


        /**
         * Linearly map values into a new given range
         */
        public static float mapValue(float value, float initialMin, float initialMax, float newMin, float newMax)
        {
            return (newMin + ((value - initialMin) * (newMax - newMin)) / (initialMax - initialMin));
        }


        /*
         * Returns an integer value for the preferred neighbour between two nodes
         * to aid the blind search in finding it's best neighbour to move to.
         */
        public static int getPreferredNeighbour(DataNode start, DataNode end)
        {
            int startX = start.getGraphLocation()[0];
            int startY = start.getGraphLocation()[1];
            int endX = end.getGraphLocation()[0];
            int endY = end.getGraphLocation()[1];

            double angle = Math.Atan2(endY - startY, endX - startX);
            angle = (180 / Math.PI) * angle + 180;

            angle = Math.Ceiling(angle);

            angle = mapValue((float)angle, 0, 360, -1, 7);

            return (int)Math.Floor(angle);

        }


        /**
         * Get euclidean distance between two nodes
         */
        public static float getDistanceBetweenNodes(DataNode start, DataNode end)
        {
            int startX = start.getGraphLocation()[0];
            int startY = start.getGraphLocation()[1];
            int endX = end.getGraphLocation()[0];
            int endY = end.getGraphLocation()[1];

            double aSquared = Math.Pow((startX - endX), 2);
            double bSquared = Math.Pow((startY - endY), 2);

            return (float)Math.Sqrt(aSquared + bSquared);
        }


    }
}
