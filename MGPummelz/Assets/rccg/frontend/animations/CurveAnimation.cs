using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{


    public class CurveAnimation : GUIAnimation
    {
        public CurveAnimator curveAnimator {
            get { return (CurveAnimator)animator; }
            set { animator = value; }
        }

        const float SLOW_SPEED = 400.0f;
        const float MEDIUM_SPEED = 600.0f;
        const float HIGH_SPEED = 600.0f;

        internal float elapsedTime;
        internal float startTime = 0;
        internal float endTime = 1.0f;

        public AnimationCurve curve;

        internal float elapsedCurveTime;
        internal float endCurveTime = 1;


        internal float distance;

        internal bool useFixedSpeed = false;

        internal float currentSpeed = SLOW_SPEED;
        internal float currentFixedSpeed = 1.0f;


        public override void startAnimationInternal()
        {

        }

        public CurveAnimation withVariableSpeed(float unitsPerSecond)
        {
            useFixedSpeed = false;
            currentSpeed = unitsPerSecond;
            return this;
        }

        public CurveAnimation withFixedSpeed(float seconds)
        {
            useFixedSpeed = true;
            currentFixedSpeed = seconds;
            return this;
        }





    }
    

       

   


}