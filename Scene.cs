using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Synthese_Image
{
	class Scene
	{
		public string name;
		public AABBTree spheresTree;
		public Sphere[] bigSpheres;
		public Sphere[] lights;
		public Camera camera;
		public Random rdm;

		public Scene(string n, AABBTree t, Sphere[] bs, Sphere[] l, Camera c)
		{
			rdm = new Random();
			name = n;
			spheresTree = t;
			bigSpheres = bs;
			lights = l;
			camera = c;
		}

		//======================================================================================================================
		//==            DRAW SCENE - créer un image ppm de la scene actuelle, lance les rayons etc.                           ==
		//======================================================================================================================
		public void DrawScene(int rayCastNb)
		{
			Vector3[,] pixMat = new Vector3[camera.width, camera.height];

			for (int y = 0; y < camera.height; y++)
			{
				for (int x = 0; x < camera.width; x++)
				{
					Vector3 color = new Vector3(0, 0, 0);

					for (int i = 0; i < rayCastNb; i++)
					{
						Vector3 start = new Vector3(camera.center.X + x + (float)(rdm.NextDouble()-0.5), camera.center.Y + y + (float)(rdm.NextDouble()-0.5), camera.center.Z);
						Vector3 direction = Vector3.Subtract(start, camera.focus);
						direction = Vector3.Normalize(direction);
						Ray r = new Ray(start, direction);
						color = Vector3.Add(color, Radiance(r, 0));
					}

					pixMat[x, y] = Vector3.Divide(color, rayCastNb);
				}
			}
			ImagePPM img = new ImagePPM(name, camera.width, camera.height).FromMatrix(pixMat);
			img.ToPPM();
		}

		//======================================================================================================================
		//==            RADIANCE - lance un rayon dans la scene et renvoi la couleur obtenue                                  ==
		//======================================================================================================================
		public Vector3 Radiance(Ray ray, int rebound)
		{
			Vector3 color = new Vector3(0, 0, 0);

			if (rebound != 5)
			{
				ResIntersect resInter = Intersects(ray);
				if (resInter.t != -1)
				{
					Vector3 interPoint = Vector3.Add(ray.point, Vector3.Multiply(ray.direction, resInter.t));
					Vector3 normal = Vector3.Subtract(interPoint, resInter.sph.center);
					normal = Vector3.Normalize(normal);
					interPoint = Vector3.Add(interPoint, Vector3.Multiply(normal, 0.1f));
					Vector3 newDir;
					Ray reflection;
					switch (resInter.sph.material.type)
					{
						case MaterialType.Difuse:
							Vector3 directLight = directLighting(interPoint, normal, resInter);
							newDir = transformToNewONBase(randomDirectionOnHemisphere(), NewONBase(normal));
							reflection = new Ray(interPoint, newDir);
							color = Vector3.Add(directLight, Vector3.Multiply(Radiance(reflection, ++rebound), lightEmmited(newDir, normal, resInter.sph)));
							break;
						case MaterialType.Mirror:
							newDir = Vector3.Add(Vector3.Multiply(2 * -Vector3.Dot(ray.direction, normal), normal), ray.direction);
							reflection = new Ray(interPoint, newDir);
							color = Vector3.Multiply(resInter.sph.material.albedo, Radiance(reflection, ++rebound));
							break;
						case MaterialType.Light:
							color = Vector3.Divide(resInter.sph.material.albedo, 1000000);
							break;
					}
				}
			}

			return color;
		}

		//======================================================================================================================
		//==            INTERSECTS - test l'intersection du rayon r avec toute la scene, renvoi la plus proche                ==
		//======================================================================================================================
		public struct ResIntersect
		{
			public float t;
			public Sphere sph;
		}
		public ResIntersect Intersects(Ray r)
		{
			ResIntersect res = intersectTree(spheresTree, r);
			foreach (Sphere s in bigSpheres)
			{
				ResIntersect res2 = Intersect(r, s);
				if (res2.t != -1)
				{
					if (res.t == -1 || res2.t < res.t)
					{
						res.t = res2.t;
						res.sph = res2.sph;
					}
				}
			}
			return res;
		}

		//======================================================================================================================
		//==            INTERSECT - l'intersection rayon/sphere                                                               ==
		//======================================================================================================================
		public ResIntersect Intersect(Ray r, Sphere s)
		{
			ResIntersect res;
			res.t = -1;
			res.sph = null;

			float A = Vector3.Dot(r.direction, r.direction);
			float B = 2 * (Vector3.Dot(r.point, r.direction) - Vector3.Dot(s.center, r.direction));
			float C = Vector3.Dot(Vector3.Subtract(s.center, r.point), Vector3.Subtract(s.center, r.point)) - (s.radius * s.radius);
			float D = B * B - 4 * A * C;
			if(D >= 0)
			{
				float i1 = ((-B) + (float)Math.Sqrt(D)) / (2 * A);
				float i2 = ((-B) - (float)Math.Sqrt(D)) / (2 * A);
				if (i2 > 0)
				{
					res.t = i2;
					res.sph = s;
				}
				else if (i1 > 0)
				{
					res.t = i1;
					res.sph = s;
				}
			}

			return res;
		}

		//======================================================================================================================
		//==            POWER RECEIVED - renvoi la puissance de lumière reçue selon la distance                               ==
		//======================================================================================================================
		public Vector3 powerReceived(Vector3 directionToLight, Sphere light)
		{
			float dist = directionToLight.Length();

			return Vector3.Multiply(light.material.albedo, 1 / (dist * dist));
		}

		//======================================================================================================================
		//==            LIGHT EMMITED - renvoi la puissance de lumière reçue selon l'angle du rayon par rapport à la surface  ==
		//======================================================================================================================
		public Vector3 lightEmmited(Vector3 rayDir, Vector3 sphNormal, Sphere sphere)
		{
			return Vector3.Divide(Vector3.Multiply(sphere.material.albedo, Clamp(Vector3.Dot(sphNormal, rayDir), 0.0f, 1.0f)), (float)Math.PI);
		}

		//======================================================================================================================
		//==            CLAMP - clamping de v entre min et max                                                                ==
		//======================================================================================================================
		public float Clamp(float v, float min, float max)
		{
			return Math.Max(Math.Min(v, max), min);
		}

		//======================================================================================================================
		//==            DIRECT LIGHTING - renvoi la lumière directe reçue au point d'intersection du rayon                    ==
		//======================================================================================================================
		public Vector3 directLighting(Vector3 interPoint, Vector3 normal, ResIntersect resInter)
		{
			Vector3 directLight = new Vector3(0.0f, 0.0f, 0.0f);
			foreach (Sphere l in lights)
			{
				Vector3 lightNormal = Vector3.Normalize(Vector3.Subtract(interPoint, l.center));
				Vector3 rdmDir = transformToNewONBase(randomDirectionOnHemisphere(), NewONBase(lightNormal));
				Vector3 rdmPoint = Vector3.Add(l.center, Vector3.Multiply(rdmDir, l.radius + 0.1f));
				Vector3 directionToLight = Vector3.Subtract(rdmPoint, interPoint);
				Ray ray = new Ray(interPoint, directionToLight);
				ResIntersect resInterL = Intersects(ray);
				if (!(resInterL.t != -1 && resInterL.t <= 1.0f && resInterL.sph.material.type != MaterialType.Light))
					directLight = Vector3.Add(directLight, Vector3.Multiply(powerReceived(directionToLight, l), lightEmmited(Vector3.Normalize(directionToLight), normal, resInter.sph)));
			}
			return directLight;
		}

		//======================================================================================================================
		//==            RANDOM DIRECTION ON HEMISPHERE - renvoi une direction aléatoire sur un hémisphere                     ==
		//======================================================================================================================
		public Vector3 randomDirectionOnHemisphere()
		{
			Vector3 rdmDir;
			double r1 = rdm.NextDouble();
			double r2 = rdm.NextDouble();

			float x = (float)(Math.Cos(2 * Math.PI * r1) * Math.Sqrt(1 - r2));
			float y = (float)(Math.Sin(2 * Math.PI * r1) * Math.Sqrt(1 - r2));
			float z = (float)(Math.Sqrt(r2));
			rdmDir = new Vector3(x, y, z);

			return rdmDir;
		}

		//======================================================================================================================
		//==            NEW ORTHONORMAL BASE - renvoi une nouvelle base grâce à une normale                                   ==
		//======================================================================================================================
		public struct OrthonormalBase
		{
			public Vector3 x;
			public Vector3 y;
			public Vector3 z;
		}
		public OrthonormalBase NewONBase(Vector3 zNormal)
		{
			OrthonormalBase newBase;

			Vector3 randomDir = new Vector3((float)(rdm.NextDouble() - 0.5f), (float)(rdm.NextDouble() - 0.5f), (float)(rdm.NextDouble() - 0.5f));
			randomDir = Vector3.Normalize(randomDir);

			newBase.x = Vector3.Normalize(Vector3.Cross(zNormal, randomDir));
			newBase.y = Vector3.Cross(zNormal, newBase.x);
			newBase.z = zNormal;

			return newBase;
		}

		//======================================================================================================================
		//==            TRANSFORM TO NEW ORTHONORMAL BASE - "rotationne" une direction pour la mettre selon une nouvelle base ==
		//======================================================================================================================
		public Vector3 transformToNewONBase(Vector3 dir, OrthonormalBase newBase)
		{
			return Vector3.Add(Vector3.Add(Vector3.Multiply(dir.X, newBase.x), Vector3.Multiply(dir.Y, newBase.y)), Vector3.Multiply(dir.Z, newBase.z));
		}

		//======================================================================================================================
		//==            AXE ALIGNED BOUNDIND BOX INTERSECT - l'intersection rayon/boite englobante                            ==
		//======================================================================================================================
		public bool AABBoxIntersect(AABBox box, Ray r)
		{
			float txmin = (box.pmin.X - r.point.X) / r.direction.X;
			float txmax = (box.pmax.X - r.point.X) / r.direction.X;

			if(txmin > txmax)
			{
				float tmp = txmin;
				txmin = txmax;
				txmax = tmp;
			}

			float tymin = (box.pmin.Y - r.point.Y) / r.direction.Y;
			float tymax = (box.pmax.Y - r.point.Y) / r.direction.Y;

			if(tymin > tymax)
			{
				float tmp = tymin;
				tymin = tymax;
				tymax = tmp;
			}

			float tzmin = (box.pmin.Z - r.point.Z) / r.direction.Z;
			float tzmax = (box.pmax.Z - r.point.Z) / r.direction.Z;

			if (tzmin > tzmax)
			{
				float tmp = tzmin;
				tzmin = tzmax;
				tzmax = tmp;
			}

			if ((txmin > tymax) || (tymin > txmax))
				return false;

			if ((txmin > tzmax) || (tzmin > txmax))
				return false;

			return true;
		}

		//======================================================================================================================
		//==            INTERSECT TREE - test l'intersection du rayon r avec un arbre de boîtes englobantes                   ==
		//======================================================================================================================
		public ResIntersect intersectTree(AABBTree tree, Ray r)
		{
			ResIntersect res;
			res.t = -1;
			res.sph = null;

			if(tree.leaf)
			{
				res = Intersect(r, tree.sph);
			}
			else
			{
				if(AABBoxIntersect(tree.box, r))
				{
					ResIntersect res1 = intersectTree(tree.tree1, r);
					ResIntersect res2 = intersectTree(tree.tree2, r);

					if (res1.t != -1)
						if (res2.t != -1)
							if (res1.t < res2.t)
								res = res1;
							else
								res = res2;
						else
							res = res1;
					else if (res2.t != -1)
						res = res2;
				}
			}

			return res;
		}
	}
}
