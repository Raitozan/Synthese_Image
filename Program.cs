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
			//===============SCENE=1=============================================================================================================================

			Sphere light = new Sphere(new Vector3(640, 50, 500), 10, new Material(MaterialType.Light, new Vector3(1, 1, 1)));

            Sphere leftWall = new Sphere(new Vector3((float)-1e5, 360, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(1, 0.1f, 0.1f)));
            Sphere rightWall = new Sphere(new Vector3((float)1e5+1280, 360, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(0.1f, 1, 0.1f)));
            Sphere topWall = new Sphere(new Vector3(640, (float)-1e5, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(0.1f, 1, 1)));
            Sphere bottomWall = new Sphere(new Vector3(640, (float)1e5 + 720, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(1, 1, 0.1f)));
            Sphere backWall = new Sphere(new Vector3(640, 360, (float)1e5 + 1000), (float)1e5, new Material(MaterialType.Difuse, new Vector3(0.1f, 0.1f, 1)));
            Sphere frontWall = new Sphere(new Vector3(640, 360, (float)-1e5), (float)1e5, new Material(MaterialType.Difuse, new Vector3(1, 0.1f, 1)));

            Sphere s1 = new Sphere(new Vector3(640, 620, 500), 100, new Material(MaterialType.Mirror, new Vector3(1, 1, 1)));
            Sphere s2 = new Sphere(new Vector3(1040, 250, 600), 100, new Material(MaterialType.Mirror, new Vector3(1, 1, 1)));
            Sphere s3 = new Sphere(new Vector3(830, 300, 560), 40, new Material(MaterialType.Difuse, new Vector3(1, 0.05f, 0.05f)));
            Sphere s4 = new Sphere(new Vector3(940, 450, 600), 100, new Material(MaterialType.Difuse, new Vector3(0.05f, 1, 0.05f)));
            Sphere s5 = new Sphere(new Vector3(200, 200, 800), 70, new Material(MaterialType.Difuse, new Vector3(0.05f, 0.05f, 1)));

			//===============SCENE=2=============================================================================================================================

			Sphere leftWall2 = new Sphere(new Vector3((float)-1e5, 360, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			Sphere rightWall2 = new Sphere(new Vector3((float)1e5 + 1280, 360, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			Sphere topWall2 = new Sphere(new Vector3(640, (float)-1e5, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			Sphere bottomWall2 = new Sphere(new Vector3(640, (float)1e5 + 720, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			Sphere backWall2 = new Sphere(new Vector3(640, 360, (float)1e5 + 1000), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			Sphere frontWall2 = new Sphere(new Vector3(640, 360, (float)-1e5), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));

			Sphere s6 = new Sphere(new Vector3(640, 620, 500), 100, new Material(MaterialType.Difuse, new Vector3(1, 1, 1)));

            //===================================================================================================================================================

            Sphere[] sphL = {light, leftWall, rightWall, topWall, bottomWall, backWall, frontWall, s1, s2, s3, s4, s5};
            Sphere[] sphL2 = { light, leftWall2, rightWall2, topWall2, bottomWall2, backWall2, frontWall2, s6};

            Camera cam = new Camera(new Vector3(0, 0, 0), 1280, 720, new Vector3(640, 360, -1000));

            Light lgt = new Light(new Vector3(640, 50, 500), new Vector3(1000000, 1000000, 1000000));

			Scene scn = new Scene("Scene", sphL, cam, lgt);
            Scene scn2 = new Scene("MirrorCeption", sphL2, cam, lgt);

            scn.DrawScene();
            scn2.DrawScene();
        }
    }
}
