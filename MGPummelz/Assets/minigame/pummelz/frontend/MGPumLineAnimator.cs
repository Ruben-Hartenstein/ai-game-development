using RelegatiaCCG.rccg.frontend.animations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace mg.pummelz
{
    public class MGPumLineAnimator : CurveAnimator
    {

        public LineRenderer line;

        internal Vector3 start;
        internal Vector3 end;

        internal MGPumLineAnimator showLine(Transform start, Transform end)
        {
            this.start = start.position;
            this.start.z = -2;
            this.end = end.position;
            this.end.z = -2;
            return this;
        }


        protected override void startAnimationCurveExtension()
        {
            this.line.gameObject.SetActive(true);
        }

        protected override void finishCurveExtension()
        {
            this.line.gameObject.SetActive(false);
        }

        protected override void terminateCurveExtension()
        {
            this.line.gameObject.SetActive(false);
        }

        protected override void updateCurveExtension(float deltaTime, float curveValue)
        {
            Vector3[] linePositions = new Vector3[2];

            if (curveValue <= 0.5f)
            {
                linePositions[0] = (start);
                linePositions[1] = (start + (end - start) * (curveValue * 2.0f));
            }
            else
            {
                linePositions[0] = (start + (end - start) * ((curveValue - 0.5f) * 2.0f));
                linePositions[1] = (end);
            }
            line.positionCount = 2;
            line.SetPositions(linePositions);

        }

    }

}
