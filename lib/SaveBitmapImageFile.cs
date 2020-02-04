using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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

    }
}
