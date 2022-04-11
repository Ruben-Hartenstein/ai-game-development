using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{


    public abstract class GUISubAnimator : MonoBehaviour
    {
        internal GUIAnimator parentAnimator;

        internal bool subAnimationRunning;

        protected bool destroyMeAtFinish = false;
        protected bool destroyGameObjectAtFinish = false;

        public void registeredAt(GUIAnimator myAnimator)
        {
            parentAnimator = myAnimator;
        }

        public void startSubAnimation()
        {
            subAnimationRunning = true;
            startSubAnimationInternal();
        }

        public abstract void startSubAnimationInternal();

        public void endSubAnimation()
        {
            subAnimationRunning = false;
            endSubAnimationInternal();

            if (destroyGameObjectAtFinish)
            {
                destroyGO();
            }
            else if (destroyMeAtFinish)
            {
                Destroy(this);
            }
        }

        protected virtual void destroyGO()
        {
            Destroy(this.gameObject);
        }

        public abstract void endSubAnimationInternal();

        public GUISubAnimator destroyGameObjectAfterwards()
        {
            destroyGameObjectAtFinish = true;
            return this;
        }

    }

}