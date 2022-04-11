using System;
using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{


    public class GUILinearAnimPath : GUIAnimPath
    {

        public GUILinearAnimPath(Vector3 start, Vector3 end)
        {
            this.startPoint = start;
            this.endPoint = end;
        }

        public override Vector3 Evaluate(float pathTime)
        {
            float adjustedPathTime = Math.Min(1.0f, Math.Max(0.0f, pathTime));

            Vector3 diff = endPoint - startPoint;

            return startPoint + adjustedPathTime * diff;

        }
    }

}