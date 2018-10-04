using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Synthese_Image
{
    class Light
    {
        public Vector3 origin;
        public Vector3 power;

        public Light(Vector3 o, Vector3 p)
        {
            origin = o;
            power = p;
        }
    }
}
