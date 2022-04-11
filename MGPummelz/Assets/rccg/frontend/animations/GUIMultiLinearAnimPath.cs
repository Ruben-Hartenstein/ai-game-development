using System;
using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{


    public class GUIMultiLinearAnimPath : GUIAnimPath
    {
        Vector3[] points;


        public GUIMultiLinearAnimPath(params Vector3[] points)
        {
            this.startPoint = points[0];
            this.endPoint = points[points.Length - 1];
            this.points = points;
        }

        public override Vector3 Evaluate(float pathTime)
        {
            float adjustedPathTime = Math.Min(1.0f, Math.Max(0.0f, pathTime));

            int currentPoint = (int)((float)(points.Length - 1) * adjustedPathTime);
            if(currentPoint == points.Length - 1)
            {
                return endPoint;
            }
            else
            {
                Vector3 diff = points[currentPoint + 1] - points[currentPoint];

                //Debug.LogError("currentPoint" + currentPoint);
                //Debug.LogError("adjustedPathTime / points.Length" + adjustedPathTime / points.Length);
                //Debug.LogError("Progress in current segment" + ((adjustedPathTime - currentPoint * (1.0f / (points.Length - 1))) * (points.Length - 1)));
                
                return points[currentPoint] + ((adjustedPathTime - currentPoint * (1.0f / (points.Length - 1)) ) * (points.Length - 1)) * diff;
            }

        }
    }

}