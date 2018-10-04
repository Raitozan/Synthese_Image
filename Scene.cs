using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthese_Image
{
    class Scene
    {
        public Sphere[] spheres;
        public Camera camera;
        public Light light;

        public Scene(Sphere[] s, Camera c, Light l)
        {
            spheres = s;
            camera = c;
            light = l;
        }
    }
}
