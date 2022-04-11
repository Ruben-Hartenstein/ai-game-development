using UnityEngine;
using UnityEngine.UI;

namespace RelegatiaCCG.rccg.frontend.animations
{

    public class DissolveAnimator : CurveAnimator
    {
        public Image image;

        private float homeState;

        private float oldState;

        private float newState;

        private float diffState;

        private bool initialized;

        private void initialize()
        {
            if(!initialized)
            {
                Material material = MonoBehaviour.Instantiate(image.material);
                this.gameObject.GetComponent<Image>().material = material;
                this.homeState = getThreshold();
                this.oldState = getThreshold();
                initialized = true;
            }

            
        }

        public void Awake()
        {
            initialize();
        }

        
        private void setThreshold(float threshold)
        {
            initialize();
            image.material.SetFloat("_Threshold", threshold);
        }

        private float getThreshold()
        {
            return image.material.GetFloat("_Threshold");
        }

        internal void setVisibleImmediately()
        {
            setThreshold(0.0f);
            this.oldState = 0.0f;
            this.newState = 0.0f;
        }



      

        internal void setInvisibleImmediately()
        {
            setThreshold(1.01f);
            this.oldState = 1.01f;
            this.newState = 1.01f;
        }

        internal DissolveAnimator moveToX(float x)
        {
            newState = x;
            return this;
        }

        internal DissolveAnimator dissolve()
        {
            setThreshold(0.0f);
            newState = 1.01f;
            return this;
        }

        internal DissolveAnimator appear()
        {
            setThreshold(1.01f);
            newState = 0.0f;
            return this;
        }

        internal DissolveAnimator moveXRelative(float x)
        {
            newState += x;
            return this;
        }

        internal DissolveAnimator moveToHome()
        {
            this.newState = homeState;
            return this;
        }

        void Start()
        {
        }

        protected override void startAnimationCurveExtension()
        {
            oldState = getThreshold();

            diffState = newState - oldState;
        }

        protected override void updateCurveExtension(float deltaTime, float curveValue)
        {
            setThreshold(oldState + diffState * curveValue);
        }

        internal void reset()
        {
            initialize();
            terminateAnimation();
            setThreshold(homeState);
        }

        internal void setCurrentAsHome()
        {
            this.homeState = getThreshold();
        }

        internal void moveInstant()
        {
            initialize();
            terminateAnimation();
            setThreshold(newState);
        }
    }
}