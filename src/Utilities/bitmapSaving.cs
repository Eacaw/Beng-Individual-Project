using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using BEng_Individual_Project.src;
using BEng_Individual_Project.lib;

namespace BEng_Individual_Project.src.Utilities
{
    public static class bitmapSaving
    {


        /**
         * Combine all methods to output a image file
         */
         public static void generateOutputImage(string filename, int width, int height, float[,] noiseValues)
        {
            float[] minMaxValues = numericalUtilities.findMinMax(noiseValues, height, width);
            byte[] outputData = generateImageData(noiseValues, height, width, minMaxValues);
            SaveBitmap(filename, width, height, outputData);
        }

        /**
        * Output a bitmap image of the terrain data upon request
        * Code "UNSAFE", replacement code to be found
        */
        public static void SaveBitmap(string fileName, int width, int height, byte[] imageData)
        {

            byte[] data = new byte[width * height * 4];

            int o = 0;

            for (int i = 0; i < width * height; i++)
            {
                byte value = imageData[i];


                data[o++] = value;
                data[o++] = value;
                data[o++] = value;
                data[o++] = 0;
            }

            unsafe
            {
                fixed (byte* ptr = data)
                {

                    using (Bitmap image = new Bitmap(width, height, width * 4, PixelFormat.Format32bppRgb, new IntPtr(ptr)))
                    {
                        image.Save(System.IO.Path.ChangeExtension(fileName, ".bmp"),ImageFormat.Bmp);
                    }
                }
            }
        }



        /**
         * Generates byte array for grayscale image data
         */
        public static byte[] generateImageData(float[,] noiseValues, int height, int width, float[] minMaxValues)
        {
            byte[] imageDataBytes = new byte[noiseValues.Length];
            int write = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    imageDataBytes[write++] = (byte)(normaliseFloat(noiseValues[i, j], minMaxValues) * 255);
                }
            }

            return imageDataBytes;
        }

        /**
         * Normalises the floating point values
         * between 0 and 1.
         */
        private static float normaliseFloat(float value, float[] minMaxValues)
        {
            float minimum = minMaxValues[0];
            float maximum = minMaxValues[1];
            return ((value - minimum) / (maximum - minimum));
        }

        

    }
}





