using rccg.frontend.common;
using System.Collections;
using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{

    public abstract class CurveAnimator : GUIAnimator
    {

        protected CurveAnimation legacyCurveAnimation
        {
            set
            {
                _legacyAnimation = value;
            }
            get
            {
                if (_legacyAnimation == null)
                {
                    _legacyAnimation = newAnimation();
                }
                return (CurveAnimation)_legacyAnimation;
            }

        }

        protected CurveAnimation runningCurveAnimation
        {
            set
            {
                runningAnimation = value;
            }
            get
            {
                if (runningAnimation == null)
                {
                    return null;
                }
                return (CurveAnimation)runningAnimation;
            }

        }


        protected override GUIAnimation newAnimation()
        {
            CurveAnimation animation = new CurveAnimation();
            animation.animator = this;
            return animation;
        }


        public CurveAnimator setCurve(string id)
        {
            legacyCurveAnimation.curve = GUICurves.get(id);
            legacyCurveAnimation.endCurveTime = 1;
            return this;
        }

        public CurveAnimator setCurve(AnimationCurve curve)
        {
            legacyCurveAnimation.curve = curve;
            legacyCurveAnimation.endCurveTime = 1;
            return this;
        }

        //do this AFTER setting curve
        public CurveAnimator pingpong(int times)
        {
            legacyCurveAnimation.curve = new AnimationCurve(legacyCurveAnimation.curve.keys);
            legacyCurveAnimation.curve.preWrapMode = WrapMode.PingPong;
            legacyCurveAnimation.curve.postWrapMode = WrapMode.PingPong;
            legacyCurveAnimation.endCurveTime = times * 2;
            return this;
        }

        public CurveAnimator returnToNull(float fixedSpeed)
        {
            float currValue = legacyCurveAnimation.curve.Evaluate(legacyCurveAnimation.elapsedCurveTime);
            legacyCurveAnimation.curve = GUICurves.linear(legacyCurveAnimation.curve.Evaluate(legacyCurveAnimation.elapsedCurveTime), 0);
            legacyCurveAnimation.elapsedCurveTime = 0;
            legacyCurveAnimation.endCurveTime = 1;
            this.setFixedSpeed(fixedSpeed * currValue);
            return this;
        }

        public CurveAnimator loop(int times)
        {
            legacyCurveAnimation.curve = new AnimationCurve(legacyCurveAnimation.curve.keys);
            legacyCurveAnimation.curve.preWrapMode = WrapMode.Loop;
            legacyCurveAnimation.curve.postWrapMode = WrapMode.Loop;
            legacyCurveAnimation.endCurveTime = times;
            return this;
        }

        public CurveAnimator setStartTime(float seconds)
        {
            legacyCurveAnimation.startTime = seconds;
            return this;
        }

        public CurveAnimator setStartTimeByCurveValue(float curveValue)
        {
            float stCandidate = 0.0f;
            //search the curveForAFittingValue
            for (int i = 1; i < 999; i++)
            {
                stCandidate += 0.02f;
                float cValue = legacyCurveAnimation.curve.Evaluate(stCandidate / legacyCurveAnimation.endTime);

                if (cValue >= curveValue)
                {
                    break;
                }
            }

            legacyCurveAnimation.startTime = stCandidate;
            return this;
        }



        public CurveAnimator setFixedSpeed(float seconds)
        {
            legacyCurveAnimation.useFixedSpeed = true;
            legacyCurveAnimation.currentFixedSpeed = seconds;
            return this;
        }

        public CurveAnimator setVariableSpeed(float unitsPerSecond)
        {
            legacyCurveAnimation.useFixedSpeed = false;
            legacyCurveAnimation.currentSpeed = unitsPerSecond;
            return this;
        }

        public float getStartTime()
        {
            return runningCurveAnimation.startTime;
        }

        public float getElapsedTime()
        {
            return runningCurveAnimation.elapsedTime;
        }
        public float getCurveValue()
        {
            return runningCurveAnimation.curve.Evaluate(runningCurveAnimation.elapsedCurveTime);
        }

        public virtual void Update()
        {
            if (animationRunning)
            {
                float deltaTime = getAdjustedDeltaTime();

                runningCurveAnimation.elapsedTime += deltaTime;

                runningCurveAnimation.elapsedCurveTime = runningCurveAnimation.elapsedTime / runningCurveAnimation.endTime;
                float curveValue = runningCurveAnimation.curve.Evaluate(runningCurveAnimation.elapsedCurveTime);

                updateCurveExtension(deltaTime, curveValue);

                //Debug.LogError("curve.postWrapMode " + curve.postWrapMode);
                //Debug.LogError("elapsedCurveTime " + elapsedCurveTime);
                //Debug.LogError("endCurveTime " + endCurveTime);
                //Debug.LogError("curveValue " + curveValue);


                if (runningCurveAnimation.elapsedCurveTime > runningCurveAnimation.endCurveTime)
                {
                    finishCurveExtension();
                    finishedAnimation();
                }


            }

        }

        protected virtual float calcDistExtension()
        {
            return 1.0f;
        }

        protected virtual void startAnimationCurveExtension()
        {

        }

        protected virtual void updateCurveExtension(float deltaTime, float curveValue)
        {

        }

        protected virtual void finishCurveExtension()
        {

        }

        protected virtual void terminateCurveExtension()
        {

        }


        public override IEnumerator startAnimationInternal()
        {
            startAnimationCurveExtension();

            if (this == null || this.gameObject == null || !this.gameObject.activeSelf)
            {
                Debug.LogWarning("Animation called on nonactive image - finishing automatically." + (this != null ? "" + this.gameObject : ""));
                finishedAnimation();
            }

            if(runningCurveAnimation.curve == null)
            {
                Debug.LogError("Animation " + gameObject.name + " has no curve. Substituting linear.");
                runningCurveAnimation.curve = GUICurves.get(GUICurves.Linear);
            }

            runningCurveAnimation.elapsedTime = runningCurveAnimation.startTime;

            runningCurveAnimation.elapsedCurveTime = runningCurveAnimation.startTime / runningCurveAnimation.endTime;
            

            if (runningCurveAnimation.useFixedSpeed)
            {
                runningCurveAnimation.endTime = runningCurveAnimation.currentFixedSpeed;
            }
            else
            {
                runningCurveAnimation.distance = calcDistExtension();
                runningCurveAnimation.endTime = runningCurveAnimation.distance / runningCurveAnimation.currentSpeed;

                //If there is no distance use fixed speed instead
                if (runningCurveAnimation.endTime == 0.0f)
                {
                    runningCurveAnimation.endTime = runningCurveAnimation.currentFixedSpeed;
                }
            }

            return null;

        }

      
        internal void terminateAnimation()
        {
            terminateCurveExtension();

            if (animationRunning)
            {
                finishedAnimation();
            }
        }


     
    }
}