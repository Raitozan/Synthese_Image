using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Synthese_Image
{
    class ImagePPM
    {
        public string name;
        public int width, height;
        public Vector3[,] pixels;

        public ImagePPM(string n, int w, int h)
        {
            name = n;
            width = w;
            height = h;
            FillWithBlank();
        }

        public void FillWithBlank()
        {
            pixels = new Vector3[width,height];
            for(int y=0; y<height; y++)
            {
                for(int x=0; x<width; x++)
                {
                    pixels[x, y] = new Vector3(0, 0, 0);
                }
            }
        }

        public ImagePPM FromMatrix(Vector3[,] matrix)
        {
            pixels = matrix;
            return this;
        }

        public void ToPPM()
        {
            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Debug.Print(path);
            using (StreamWriter writer = new StreamWriter(@path + "\\" + name + ".ppm"))
            {
                writer.WriteLine("P3");
                writer.WriteLine(width.ToString() + " " + height.ToString());
                writer.WriteLine("255");
                for (int y = 0; y < height; y++)
                {
                    string line = "";
                    for (int x = 0; x < width; x++)
                    {
                        Vector3 color = Vector3.Clamp(new Vector3((float)Math.Pow(pixels[x, y].X, (1 / 2.2f)), (float)Math.Pow(pixels[x, y].Y, (1 / 2.2f)), (float)Math.Pow(pixels[x, y].Z, (1 / 2.2f))), new Vector3(0,0,0), new Vector3(1,1,1));
                        line += (color.X * 255).ToString() + " " + (color.Y * 255).ToString() + " " + (color.Z * 255).ToString() + " ";
                    }
                    writer.WriteLine(line);
                }
                System.Diagnostics.Process.Start(@path + "\\" + name + ".ppm");
            }
        }
    }
}
