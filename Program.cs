using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Synthese_Image
{
    class Program
    {
        static void Main(string[] args)
        {
            Sphere s1 = new Sphere(new Vector3(500, 180, 210), 200, new Material(MaterialType.Diffuse, new Vector3(1, 1, 1)));
            Sphere s2 = new Sphere(new Vector3(200, 180, 74), 30, new Material(MaterialType.Diffuse, new Vector3(0.05f, 1, 0.05f)));
            Sphere s4 = new Sphere(new Vector3(30, 260, 30), 30, new Material(MaterialType.Diffuse, new Vector3(0.05f, 0.05f, 1)));
            Sphere s3 = new Sphere(new Vector3(200, 180, 550), 200, new Material(MaterialType.Diffuse, new Vector3(1, 0.05f, 0.05f)));

            Sphere[] sphL = { s1, s2, s3, s4 };

            Camera cam = new Camera(new Vector3(0, 0, 0), 640, 360, new Vector3(320, 180, -1000));

            Light lgt = new Light(new Vector3(0, 180, 65), new Vector3(1000000, 1000000, 1000000));

            Scene scn = new Scene(sphL, cam, lgt);

            scn.DrawScene(scn);

            return;
        }
    }
}
