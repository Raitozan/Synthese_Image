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

			Sphere light = new Sphere(new Vector3(640, 360, 500), 10, new Material(MaterialType.Light, new Vector3(1, 1, 1)));

			Sphere leftWall = new Sphere(new Vector3((float)-1e5, 360, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(1, 0.1f, 0.1f)));
			Sphere rightWall = new Sphere(new Vector3((float)1e5 + 1280, 360, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(0.1f, 1, 0.1f)));
			Sphere topWall = new Sphere(new Vector3(640, (float)-1e5, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(0.1f, 1, 1)));
			Sphere bottomWall = new Sphere(new Vector3(640, (float)1e5 + 720, 500), (float)1e5, new Material(MaterialType.Difuse, new Vector3(1, 1, 0.1f)));
			Sphere backWall = new Sphere(new Vector3(640, 360, (float)1e5 + 1000), (float)1e5, new Material(MaterialType.Difuse, new Vector3(0.1f, 0.1f, 1)));
			Sphere frontWall = new Sphere(new Vector3(640, 360, (float)-1e5), (float)1e5, new Material(MaterialType.Difuse, new Vector3(1, 0.1f, 1)));

			Sphere sLeft = new Sphere(new Vector3(100, 360, 100), 100, new Material(MaterialType.Difuse, new Vector3(0.1f, 1, 0.1f)));

			Sphere sLeftDown = new Sphere(new Vector3(370, 490, 300), 50, new Material(MaterialType.Mirror, new Vector3(1, 1, 0.1f)));
			Sphere sLeftUp = new Sphere(new Vector3(370, 230, 300), 50, new Material(MaterialType.Mirror, new Vector3(1, 0.1f, 0.1f)));

			Sphere sDown = new Sphere(new Vector3(640, 620, 500), 100, new Material(MaterialType.Difuse, new Vector3(0.1f, 1, 1)));
			Sphere sUp = new Sphere(new Vector3(640, 100, 500), 100, new Material(MaterialType.Difuse, new Vector3(1, 1, 0.1f)));

			Sphere sRightDown = new Sphere(new Vector3(910, 490, 700), 50, new Material(MaterialType.Mirror, new Vector3(0.1f, 1, 0.1f)));
			Sphere sRightUp = new Sphere(new Vector3(910, 230, 700), 50, new Material(MaterialType.Mirror, new Vector3(0.1f, 1, 1)));

			Sphere sRight = new Sphere(new Vector3(1180, 360, 900), 100, new Material(MaterialType.Difuse, new Vector3(1, 0.1f, 0.1f)));

			//===============SCENE=2=============================================================================================================================

			Sphere light2 = new Sphere(new Vector3(640, 50, 500), 10, new Material(MaterialType.Light, new Vector3(1, 1, 1)));

			Sphere leftWall2 = new Sphere(new Vector3((float)-1e5, 360, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			Sphere rightWall2 = new Sphere(new Vector3((float)1e5 + 1280, 360, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			Sphere topWall2 = new Sphere(new Vector3(640, (float)-1e5, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			Sphere bottomWall2 = new Sphere(new Vector3(640, (float)1e5 + 720, 500), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			Sphere backWall2 = new Sphere(new Vector3(640, 360, (float)1e5 + 1000), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));
			Sphere frontWall2 = new Sphere(new Vector3(640, 360, (float)-1e5), (float)1e5, new Material(MaterialType.Mirror, new Vector3(0.9f, 0.9f, 0.9f)));

			Sphere sph = new Sphere(new Vector3(640, 620, 500), 100, new Material(MaterialType.Difuse, new Vector3(1, 1, 1)));

			//===================================================================================================================================================

			Sphere[] sphL = { light, leftWall, rightWall, topWall, bottomWall, backWall, frontWall, sDown, sUp, sLeft, sRight, sLeftDown, sLeftUp, sRightDown, sRightUp};
			Sphere[] sphL2 = { light2, leftWall2, rightWall2, topWall2, bottomWall2, backWall2, frontWall2, sph};

			Camera cam = new Camera(new Vector3(0, 0, 0), 1280, 720, new Vector3(640, 360, -1000));

			Light lgt = new Light(new Vector3(640, 360, 500), new Vector3(10000000, 10000000, 10000000));
			Light lgt2 = new Light(new Vector3(640, 50, 500), new Vector3(1000000, 1000000, 1000000));

			Scene scn = new Scene("Scene", sphL, cam, lgt);
			Scene scn2 = new Scene("MirrorCeption", sphL2, cam, lgt2);

			scn.DrawScene();
			//scn2.DrawScene();
		}
	}
}
