using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{


    public abstract class GUIAnimPath
    {
        public Vector3 startPoint;
        public Vector3 endPoint;

        public abstract Vector3 Evaluate(float pathTime);


        public static GUILinearAnimPath line(Vector3 start, Vector3 end)
        {
            return new GUILinearAnimPath(start, end);
        }

        public static GUIMultiLinearAnimPath shakeHorizontalRightToLeft(Transform t, float intensity)
        {
            return new GUIMultiLinearAnimPath(t.localPosition, t.localPosition + Vector3.right * intensity, t.localPosition, t.localPosition + Vector3.left * intensity, t.localPosition);
        }

        public static GUIMultiLinearAnimPath shakeHorizontalLeftToRight(Transform t, float intensity)
        {
            return new GUIMultiLinearAnimPath(t.localPosition, t.localPosition + Vector3.left * intensity, t.localPosition, t.localPosition + Vector3.right * intensity, t.localPosition);
        }

        public static GUIMultiLinearAnimPath shakeHorizontalCenterToRight(Transform t, float intensity)
        {
            return new GUIMultiLinearAnimPath(t.localPosition, t.localPosition + Vector3.right * intensity, t.localPosition);
        }

        public static GUIMultiLinearAnimPath shakeHorizontalCenterToLeft(Transform t, float intensity)
        {
            return new GUIMultiLinearAnimPath(t.localPosition, t.localPosition + Vector3.left * intensity, t.localPosition);
        }


        public static GUIQuadBezierAnimPath bezier(Vector3 start, Vector3 middle, Vector3 end)
        {
            return new GUIQuadBezierAnimPath(start, middle, end);
        }

        public static GUIQuadBezierAnimPath bezierLocalToGlobal(Transform parent, Vector3 localStart, Vector3 globalEnd)
        {
            Vector3 localEnd = parent.InverseTransformPoint(globalEnd);

            Vector3 localMiddle = new Vector3(localEnd.x, localStart.y, localStart.z);

            return new GUIQuadBezierAnimPath(localStart, localMiddle, localEnd);
        }

    }

}