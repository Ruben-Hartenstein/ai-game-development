using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{


    public class GUIQuadBezierAnimPath : GUIAnimPath
    {
        public Vector3 midPoint;

        public GUIQuadBezierAnimPath(Vector3 start, Vector3 middle, Vector3 end) 
        {
            startPoint = start;
            midPoint = middle;
            endPoint = end;
        }

        public override Vector3 Evaluate(float t)
        {
            Vector3 result = Mathf.Pow(1.0f - t, 2) * startPoint + 2.0f * t * (1.0f - t) * midPoint + Mathf.Pow(t, 2) * endPoint;
            return result;
        }
    }

}