using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthese_Image
{
	class AABBTree
	{
		public bool leaf;

		//if leaf
		public AABBox box;
		public AABBTree tree1;
		public AABBTree tree2;

		//else
		public Sphere sph;

		public AABBTree(List<Sphere> spheres, AABBox b)
		{
			if (spheres.Count == 1)
			{
				leaf = true;
				sph = spheres[0];
			}
			else
			{
				leaf = false;
				box = b;

				AABBox actualBox;

				List<AABBox> Xforward = new List<AABBox>();
				spheres = spheres.OrderBy(v => v.center.X).ToList();
				actualBox = new AABBox(spheres[0]);
				Xforward.Add(actualBox);
				for (int i = 1; i < spheres.Count-1; i++)
				{
					actualBox = new AABBox(actualBox, new AABBox(spheres[i]));
					Xforward.Add(actualBox);
				}

				List<AABBox> Xbackward = new List<AABBox>();
				spheres.Reverse();
				actualBox = new AABBox(spheres[0]);
				Xbackward.Add(actualBox);
				for (int i = 1; i < spheres.Count - 1; i++)
				{
					actualBox = new AABBox(actualBox, new AABBox(spheres[i]));
					Xbackward.Add(actualBox);
				}

				List<AABBox> Yforward = new List<AABBox>();
				spheres = spheres.OrderBy(v => v.center.Y).ToList();
				actualBox = new AABBox(spheres[0]);
				Yforward.Add(actualBox);
				for (int i = 1; i < spheres.Count - 1; i++)
				{
					actualBox = new AABBox(actualBox, new AABBox(spheres[i]));
					Yforward.Add(actualBox);
				}

				List<AABBox> Ybackward = new List<AABBox>();
				spheres.Reverse();
				actualBox = new AABBox(spheres[0]);
				Ybackward.Add(actualBox);
				for (int i = 1; i < spheres.Count - 1; i++)
				{
					actualBox = new AABBox(actualBox, new AABBox(spheres[i]));
					Ybackward.Add(actualBox);
				}

				List<AABBox> Zforward = new List<AABBox>();
				spheres = spheres.OrderBy(v => v.center.Z).ToList();
				actualBox = new AABBox(spheres[0]);
				Zforward.Add(actualBox);
				for (int i = 1; i < spheres.Count - 1; i++)
				{
					actualBox = new AABBox(actualBox, new AABBox(spheres[i]));
					Zforward.Add(actualBox);
				}

				List<AABBox> Zbackward = new List<AABBox>();
				spheres.Reverse();
				actualBox = new AABBox(spheres[0]);
				Zbackward.Add(actualBox);
				for (int i = 1; i < spheres.Count - 1; i++)
				{
					actualBox = new AABBox(actualBox, new AABBox(spheres[i]));
					Zbackward.Add(actualBox);
				}
				
				List<AABBox> Forward = new List<AABBox>();
				Forward.AddRange(Xforward);
				Forward.AddRange(Yforward);
				Forward.AddRange(Zforward);

				List<AABBox> Backward = new List<AABBox>();
				Xbackward.Reverse();
				Backward.AddRange(Xbackward);
				Ybackward.Reverse();
				Backward.AddRange(Ybackward);
				Zbackward.Reverse();
				Backward.AddRange(Zbackward);

				int indBestBox = 0;
				float bestCost = float.MaxValue;
				int nbForward = 1;
				int nbBackward = spheres.Count - 1;
				for (int i = 0; i < Forward.Count; i++)
				{
					float cost = Forward[i].getSurface() * nbForward + Backward[i].getSurface() * nbBackward;
					if(cost < bestCost)
					{
						bestCost = cost;
						indBestBox = i;
					}

					nbForward++;
					if (nbForward == spheres.Count)
						nbForward = 1;
					nbBackward--;
					if (nbBackward == 0)
						nbBackward = spheres.Count - 1;

				}

				int axis = (int)((indBestBox + 1) / (spheres.Count - 1));
				switch (axis)
				{
					case 0:
						spheres = spheres.OrderBy(v => v.center.X).ToList();
						break;
					case 1:
						spheres = spheres.OrderBy(v => v.center.Y).ToList();
						break;
					case 2:
						spheres = spheres.OrderBy(v => v.center.Z).ToList();
						break;
				}
				int pivot = (indBestBox + 1) % (spheres.Count - 1);
				if (pivot == 0)
					pivot = spheres.Count - 1;
				List<Sphere> spheresLeft = new List<Sphere>();
				List<Sphere> spheresRight = new List<Sphere>();
				for (int i = 0; i < spheres.Count; i++)
				{
					if (i < pivot)
						spheresLeft.Add(spheres[i]);
					else
						spheresRight.Add(spheres[i]);
				}

				tree1 = new AABBTree(spheresLeft, Forward[indBestBox]);
				tree2 = new AABBTree(spheresRight, Backward[indBestBox]);
			}
		}
	}
}
