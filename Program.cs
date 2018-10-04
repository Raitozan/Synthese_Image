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
            Sphere s1 = new Sphere(new Vector3(500, 180, 210), 200, new Vector3(1, 0.05f, 0.05f));
            Sphere s2 = new Sphere(new Vector3(200, 180, 74), 30, new Vector3(0.05f, 1, 0.05f));
            Sphere s3 = new Sphere(new Vector3(200, 180, 550), 200, new Vector3(0.05f, 0.05f, 1));
            Sphere s4 = new Sphere(new Vector3(30, 260, 30), 30, new Vector3(1, 1, 0.05f));

            Sphere[] sphL = { s1, s2, s3, s4 };

            Camera cam = new Camera(new Vector3(0, 0, 0), 640, 360, new Vector3(320, 180, -1000));

            Light lgt = new Light(new Vector3(0, 180, 65), new Vector3(1000000, 1000000, 1000000));

            Scene scn = new Scene(sphL, cam, lgt);

            Program.DrawScene(scn);

            return;
        }

        //====================================================================================================================================

        public static float Intersect(Ray r, Sphere s)
        {
            float A = Vector3.Dot(r.direction, r.direction);
            float B = 2 * (Vector3.Dot(r.point, r.direction) - Vector3.Dot(s.center, r.direction));
            float C = Vector3.Dot(Vector3.Subtract(s.center, r.point), Vector3.Subtract(s.center, r.point)) - (s.radius*s.radius);
            float D = B * B - 4 * A * C;
            if(D<0)
                return -1.0f;
            else
            {
                float i1 = ((-B) + (float)Math.Sqrt(D)) / (2 * A);
                float i2 = ((-B) - (float)Math.Sqrt(D)) / (2 * A);
                if (i2 > 0)
                    return i2;
                else if (i1 > 0)
                    return i1;
                else
                    return -1.0f;
            }
        }

        public struct ResIntersect
        {
            public float t;
            public Sphere sph;
        }
        public static ResIntersect Intersects(Ray r, Scene scene)
        {
            ResIntersect res;
            res.t = -1;
            res.sph = null;
            foreach(Sphere s in scene.spheres)
            {
                float inter = Intersect(r, s);
                if(inter != -1)
                {
                    if (res.t == -1 || inter < res.t)
                    {
                        res.t = inter;
                        res.sph = s;
                    }
                }
            }
            return res;
        }

        public static Vector3 ReceiveLight(Light light, Vector3 interPoint, Sphere sphere)
        {
            Vector3 l = Vector3.Subtract(light.origin, interPoint);
            float dist = l.Length();
            l = Vector3.Normalize(l);
            Vector3 n = Vector3.Subtract(interPoint, sphere.center);
            n = Vector3.Normalize(n);

            Vector3 powerReceived = Vector3.Multiply(light.power, 1 / (dist * dist));
            Vector3 lightEmmited = Vector3.Divide(Vector3.Multiply(sphere.albedo, Math.Max(Math.Min(Vector3.Dot(n, l), 1.0f), 0.0f)), (float)Math.PI);

            return Vector3.Multiply(powerReceived, lightEmmited);
        }

        public static void DrawScene(Scene scene)
        {
            Vector3[,] pixMat = new Vector3[scene.camera.width, scene.camera.height];

            for (int y = 0; y < scene.camera.height; y++)
            {
                for (int x = 0; x < scene.camera.width; x++)
                {
                    Vector3 start = new Vector3(scene.camera.center.X + x, scene.camera.center.Y + y, scene.camera.center.Z);
                    Vector3 direction = Vector3.Subtract(start, scene.camera.focus);
                    direction = Vector3.Normalize(direction);
                    Ray r = new Ray(start, direction);
                    ResIntersect resInter = Intersects(r, scene);
                    if(resInter.t != -1)
                    {
                        Vector3 interPoint = Vector3.Add(start, Vector3.Multiply(direction, 0.99999f * resInter.t));
                        Vector3 directionToLight = Vector3.Subtract(scene.light.origin, interPoint);
                        r = new Ray(interPoint, directionToLight);
                        ResIntersect resInterL = Intersects(r, scene);
                        if (resInterL.t != -1 && resInterL.t <= 1.0f)
                            pixMat[x, y] = new Vector3(0, 0, 0);
                        else
                            pixMat[x, y] = ReceiveLight(scene.light, interPoint, resInter.sph);
                    }
                    else
                        pixMat[x, y] = new Vector3(0, 0, 0);
                }
            }
            ImagePPM img = new ImagePPM("Scene", scene.camera.width, scene.camera.height).FromMatrix(pixMat);
            img.ToPPM();
        }
    }
}
