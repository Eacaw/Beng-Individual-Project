using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace BEng_Individual_Project.lib
{
    /**
     * Output a bitmap image of the terrain data upon request
     * Code "UNSAFE", replacement code to be found
     */
    public static class SaveBitmapImageFile
    {
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

                    using (Bitmap image = new Bitmap(width, height, width * 4,
                        PixelFormat.Format32bppRgb, new IntPtr(ptr)))
                    {
                        image.Save(Path.ChangeExtension(fileName, ".bmp"));
                    }
                }
            }
        }


        public static Bitmap CopyDataToBitmap(string filename, byte[] data, int height, int width)
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);

            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(
                                 new Rectangle(0, 0, bmp.Width, bmp.Height),
                                 ImageLockMode.WriteOnly, bmp.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);


            //Unlock the pixels
            bmp.UnlockBits(bmpData);

            bmp.Save(filename, ImageFormat.Jpeg);

            //Return the bitmap 
            return bmp;
        }

        public static void saveBitmapData(string filename, byte[] imageData, int height, int width)
        {
            // Create a new bitmap.
            Bitmap bmp = new Bitmap(width*4, height*4, PixelFormat.Format24bppRgb);

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData =
                bmp.LockBits(rect, ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = imageData;

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set every third value to 255. A 24bpp bitmap will look red.  
            for (int counter = 2; counter < rgbValues.Length; counter += 3)
                rgbValues[counter] = 255;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            bmp.Save(filename);
        }

    }
}
