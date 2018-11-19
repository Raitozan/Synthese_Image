using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Synthese_Image
{
	class AABBox
	{
		public Vector3 pmin;
		public Vector3 pmax;

		public AABBox(Sphere s)
		{
			pmin = new Vector3(s.center.X - s.radius, s.center.Y - s.radius, s.center.Z - s.radius);
			pmax = new Vector3(s.center.X + s.radius, s.center.Y + s.radius, s.center.Z + s.radius);
		}

		public AABBox(AABBox b1, AABBox b2)
		{
			pmin = b1.pmin;
			if (pmin.X < b2.pmin.X)
				pmin.X = b2.pmin.X;
			if (pmin.Y < b2.pmin.Y)
				pmin.Y = b2.pmin.Y;
			if (pmin.Z < b2.pmin.Z)
				pmin.Z = b2.pmin.Z;
			pmax = b1.pmax;
			if (pmax.X > b2.pmax.X)
				pmax.X = b2.pmax.X;
			if (pmax.Y > b2.pmax.Y)
				pmax.Y = b2.pmax.Y;
			if (pmax.Z > b2.pmax.Z)
				pmax.Z = b2.pmax.Z;
		}
	}
}
