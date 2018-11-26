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

		public AABBTree(List<Sphere> spheres)
		{
			if(spheres.Count == 1)
			{
				leaf = true;
				sph = spheres[0];
			}
			else
			{
				leaf = false;
				spheres = spheres.OrderBy(v => v.center.X).ToList();
				
				box = new AABBox(spheres[0]);
				for (int i = 1; i < spheres.Count; i++)
				{
					box = new AABBox(box, new AABBox(spheres[i]));
				}

				List<Sphere> spheresLeft = new List<Sphere>();
				List<Sphere> spheresRight = new List<Sphere>();
				for (int i = 0; i < spheres.Count; i++)
				{
					if (i < spheres.Count / 2)
						spheresLeft.Add(spheres[i]);
					else
						spheresRight.Add(spheres[i]);
				}

				tree1 = new AABBTree(spheresLeft);
				tree2 = new AABBTree(spheresRight);
			}
		}
	}
}
