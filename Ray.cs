using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Synthese_Image
{
	class Ray
	{
		public Vector3 point;
		public Vector3 direction;

		public Ray(Vector3 p, Vector3 d)
		{
			point = p;
			direction = d;
		}
	}
}
