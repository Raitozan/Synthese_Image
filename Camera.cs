using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Synthese_Image
{
	class Camera
	{
		public Vector3 center;
		public int width, height;
		public Vector3 focus;

		public Camera(Vector3 c, int w, int h, Vector3 f)
		{
			center = c;
			width = w;
			height = h;
			focus = f;
		}
	}
}
