using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class CheckPoint
    {
        public Vector3 p1;
        public Vector3 p2;

        public CheckPoint(float p1x, float p1y, float p1z, float p2x, float p2y, float p2z)
        {
            p1 = new Vector3(p1x, p1y, p1z);
            p2 = new Vector3(p2x, p2y, p2z);
        }
    }
}
