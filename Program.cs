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
			Sphere leftWall = new Sphere(new Vector3((float)-1e5, 360, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(0.1f, 1, 0.1f)));
			Sphere rightWall = new Sphere(new Vector3((float)1e5 + 1280, 360, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(1, 0.1f, 0.1f)));
			Sphere topWall = new Sphere(new Vector3(640, (float)-1e5, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(0.1f, 1, 1)));
			Sphere bottomWall = new Sphere(new Vector3(640, (float)1e5 + 720, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(1, 1, 0.1f)));
			Sphere backWall = new Sphere(new Vector3(640, 360, (float)1e5 + 1000), (float)1e5, new Material(MaterialType.Difuse, new Vector3(0.1f, 0.1f, 1)));
			Sphere frontWall = new Sphere(new Vector3(640, 360, (float)-1e5), (float)1e5, new Material(MaterialType.Difuse, new Vector3(0.1f, 0.1f, 1)));

			//Sphere leftWall = new Sphere(new Vector3((float)-1e5, 360, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			//Sphere rightWall = new Sphere(new Vector3((float)1e5 + 1280, 360, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			//Sphere topWall = new Sphere(new Vector3(640, (float)-1e5, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			//Sphere bottomWall = new Sphere(new Vector3(640, (float)1e5 + 720, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			//Sphere backWall = new Sphere(new Vector3(640, 360, (float)1e5 + 1000), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			//Sphere frontWall = new Sphere(new Vector3(640, 360, (float)-1e5), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));

			Sphere sLeft = new Sphere(new Vector3(100, 360, 100), 100, new Material(MaterialType.Difuse, new Vector3(1, 0.1f, 0.1f)));
			Sphere sLeftDown = new Sphere(new Vector3(370, 490, 300), 50, new Material(MaterialType.Mirror, new Vector3(0.1f, 0.1f, 1)));
			Sphere sLeftUp = new Sphere(new Vector3(370, 230, 300), 50, new Material(MaterialType.Mirror, new Vector3(1, 0.1f, 0.1f)));
			Sphere sDown = new Sphere(new Vector3(640, 620, 500), 100, new Material(MaterialType.Difuse, new Vector3(0.1f, 0.1f, 1)));
			Sphere sUp = new Sphere(new Vector3(640, 100, 500), 100, new Material(MaterialType.Difuse, new Vector3(1, 1, 0.1f)));
			Sphere sRightDown = new Sphere(new Vector3(910, 490, 700), 50, new Material(MaterialType.Mirror, new Vector3(0.1f, 1, 0.1f)));
			Sphere sRightUp = new Sphere(new Vector3(910, 230, 700), 50, new Material(MaterialType.Mirror, new Vector3(1, 1, 0.1f)));
			Sphere sRight = new Sphere(new Vector3(1180, 360, 900), 100, new Material(MaterialType.Difuse, new Vector3(0.1f, 1, 0.1f)));

			Sphere light = new Sphere(new Vector3(640, 360, 500), 10, new Material(MaterialType.Light,  new Vector3(1000000, 1000000, 1000000)));
			Sphere light2 = new Sphere(new Vector3(100, 100, 100), 30, new Material(MaterialType.Light, new Vector3(1000000,   10000,   10000)));

			Sphere[] spheres = { light, light2, sDown, sUp, sLeft, sRight, sLeftDown, sLeftUp, sRightDown, sRightUp };
			List<Sphere> sphL = new List<Sphere>();
			sphL.AddRange(spheres);
			Random rdm = new Random();
			for (int i = 0; i < 50; i++)
			{
				sphL.Add(new Sphere(new Vector3((float)(rdm.NextDouble() * 1280), (float)(rdm.NextDouble() * 720), (float)(rdm.NextDouble() * 1000)), (float)((rdm.NextDouble()*15)+5), new Material(MaterialType.Difuse, new Vector3((float)(rdm.NextDouble()), (float)(rdm.NextDouble()), (float)(rdm.NextDouble())))));
			}
			AABBTree spheresTree = new AABBTree(sphL);

			Sphere[] bigSpheres = { leftWall, rightWall, topWall, bottomWall, backWall, frontWall};

			Sphere[] lights = { light, light2 };

			Camera cam = new Camera(new Vector3(0, 0, 0), 1280, 720, new Vector3(640, 360, -1000));

			Scene scn = new Scene("Scene", spheresTree, bigSpheres, lights, cam);

			scn.DrawScene(5);
		}
	}
}
