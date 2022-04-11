using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{


    public class GUIBezierSplineAnimPath : GUIAnimPath
    {
        public Vector3[] points;
        public float[] distances;
        public float totalDistance;

        public GUIBezierSplineAnimPath(params Vector3[] points)
        {
            this.points = points;
            this.distances = new float[points.Length];
            this.totalDistance = 0.0f;
            distances[0] = 0.0f;
            for (int i = 1; i < points.Length; i++ )
            {
                //distances[i] = Vector3.Distance(points[i], points[i + 1]);
                //totalDistance += distances[i];
                totalDistance += Vector3.Distance(points[i - 1], points[i]);
                distances[i] = totalDistance;
                
            }
        }

        Vector3 CatmulRom(float tinput, int i)
        {
            //if overshooting just return last point
            if(i >= points.Length - 1)
            {
                return points[points.Length - 1];
            }

            Vector3 p0;
            if(i < 1)
            {
                //fake a point -1 by mirroring point1 accross point 0
                p0 = points[0] - (points[1] - points[0]);
            }
            else
            {
                p0 = points[i - 1];
            }

                
            Vector3 p1 = points[i];
            Vector3 p2;
            if (i + 1 >= points.Length)
            {
                p2 = points[i] - (points[i - 1] - points[i]);
            }
            else
            {
                p2 = points[i + 1];
            }
            Vector3 p3;
            if(i + 2 >= points.Length)
            {
                if (i + 1 >= points.Length)
                {
                    p3 = points[i] - (points[i - 1] - points[i]) * 2;
                }
                else
                {
                    p3 = points[i + 1] - (points[i] - points[i + 1]);
                }
            }
            else
            {
                p3 = points[i + 2];
            }
            

            float t0 = 0.0f;
            float t1 = GetT(t0, p0, p1);
            float t2 = GetT(t1, p1, p2);
            float t3 = GetT(t2, p2, p3);

            float tnormed = ((tinput * totalDistance) - distances[i]) / (distances[i + 1] - distances[i]);
            float t = (t2 - t1) * tnormed + t1;


            Debug.LogError("i=" + i + "  tinput " + tinput + "  tnormed " + tnormed + "  t " + t + "  " + Vector3.Lerp(p1, p2, tnormed));

            Vector3 A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
            Vector3 A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
            Vector3 A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

            Vector3 B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
            Vector3 B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;

            Vector3 C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;

            //return Vector3.Lerp(p1, p2, tnormed);
            return C;
        }

        float GetT(float t, Vector2 p0, Vector2 p1)
        {
            float a = Mathf.Pow((p1.x - p0.x), 2.0f) + Mathf.Pow((p1.y - p0.y), 2.0f);
            float b = Mathf.Pow(a, 0.5f);
            float c = Mathf.Pow(b, 0.5f);

            return (c + t);
        }

        public override Vector3 Evaluate(float t)
        {
            float currentDistance = t * totalDistance;
            //Debug.LogError(currentDistance + "/" + totalDistance);
            int currentIndex = 0;
            for (int i = 1; i < points.Length; i++ )
            {
                Debug.LogError(i  + "   " + distances[i]);
                if (distances[i] <= currentDistance)
                {
                    currentIndex = i;
                    
                }
                else
                {
                    break;
                }
            }

            //Debug.LogError(currentIndex + "   " + points[currentIndex] + "   " + points[currentIndex + 1]);
            //
            return CatmulRom(t, currentIndex);
        }
    }

}