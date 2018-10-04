using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Synthese_Image
{
    class Sphere
    {
        public Vector3 center;
        public float radius;
        public Vector3 albedo;

        public Sphere(Vector3 c, float r, Vector3 a)
        {
            center = c;
            radius = r;
            albedo = a;
        }
    }
}
