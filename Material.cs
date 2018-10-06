using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Synthese_Image
{
    enum MaterialType { Diffuse, Mirror };

    class Material
    {
        public MaterialType type;
        public Vector3 albedo;

        public Material(MaterialType t, Vector3 a)
        {
            type = t;
            albedo = a;
        }
    }
}
