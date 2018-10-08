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
		public Sphere[] spheres;
		public Camera camera;
		public Light light;

		public Scene(string n, Sphere[] s, Camera c, Light l)
		{
			name = n;
			spheres = s;
			camera = c;
			light = l;
		}

		public void DrawScene()
		{
			Vector3[,] pixMat = new Vector3[camera.width, camera.height];

			for (int y = 0; y < camera.height; y++)
			{
				for (int x = 0; x < camera.width; x++)
				{
					Vector3 start = new Vector3(camera.center.X + x, camera.center.Y + y, camera.center.Z);
					Vector3 direction = Vector3.Subtract(start, camera.focus);
					direction = Vector3.Normalize(direction);
					Ray r = new Ray(start, direction);

					pixMat[x, y] = Radiance(r, 0);
				}
			}
			ImagePPM img = new ImagePPM(name, camera.width, camera.height).FromMatrix(pixMat);
			img.ToPPM();
		}

		public Vector3 Radiance(Ray ray, int stop)
		{
			Vector3 color = new Vector3(0, 0, 0);

			if (stop != 100)
			{
				ResIntersect resInter = Intersects(ray);
				if (resInter.t != -1)
				{
					Vector3 interPoint = Vector3.Add(ray.point, Vector3.Multiply(ray.direction, resInter.t));
					Vector3 normal = Vector3.Subtract(interPoint, resInter.sph.center);
					normal = Vector3.Normalize(normal);
					interPoint = Vector3.Add(interPoint, Vector3.Multiply(normal, 0.1f));

					switch (resInter.sph.material.type)
					{
						case MaterialType.Difuse:
							Vector3 directionToLight = Vector3.Subtract(light.origin, interPoint);
							ray = new Ray(interPoint, directionToLight);
							ResIntersect resInterL = Intersects(ray);
							if (!(resInterL.t != -1 && resInterL.t <= 1.0f && resInterL.sph.material.type != MaterialType.Light))
								color = ReceiveLight(light, interPoint, resInter.sph);
							else
								color = Vector3.Multiply(ReceiveLight(light, interPoint, resInter.sph), 0.001f);
							break;
						case MaterialType.Mirror:
							Vector3 newDir = Vector3.Add(Vector3.Multiply(2 * -Vector3.Dot(ray.direction, normal), normal), ray.direction);
							Ray reflection = new Ray(interPoint, newDir);
							color = Vector3.Multiply(resInter.sph.material.albedo, Radiance(reflection, ++stop));
							break;
						case MaterialType.Light:
							color = resInter.sph.material.albedo;
							break;
					}
				}
			}

			return color;
		}

		public struct ResIntersect
		{
			public float t;
			public Sphere sph;
		}
		public ResIntersect Intersects(Ray r)
		{
			ResIntersect res;
			res.t = -1;
			res.sph = null;
			foreach (Sphere s in spheres)
			{
				float inter = Intersect(r, s);
				if (inter != -1)
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

		public float Intersect(Ray r, Sphere s)
		{
			float A = Vector3.Dot(r.direction, r.direction);
			float B = 2 * (Vector3.Dot(r.point, r.direction) - Vector3.Dot(s.center, r.direction));
			float C = Vector3.Dot(Vector3.Subtract(s.center, r.point), Vector3.Subtract(s.center, r.point)) - (s.radius * s.radius);
			float D = B * B - 4 * A * C;
			if (D < 0)
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

		public Vector3 ReceiveLight(Light light, Vector3 interPoint, Sphere sphere)
		{
			Vector3 l = Vector3.Subtract(light.origin, interPoint);
			float dist = l.Length();
			l = Vector3.Normalize(l);
			Vector3 n = Vector3.Subtract(interPoint, sphere.center);
			n = Vector3.Normalize(n);

			Vector3 powerReceived = Vector3.Multiply(light.power, 1 / (dist * dist));
			Vector3 lightEmmited = Vector3.Divide(Vector3.Multiply(sphere.material.albedo, Clamp(Vector3.Dot(n,l), 0.0f, 1.0f)), (float)Math.PI);

			return Vector3.Multiply(powerReceived, lightEmmited);
		}

		public float Clamp(float v, float min, float max)
		{
			return Math.Max(Math.Min(v, max), min);
		}
	}
}
