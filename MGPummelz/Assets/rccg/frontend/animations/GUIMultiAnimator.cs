using System.Collections.Generic;

namespace RelegatiaCCG.rccg.frontend.animations
{



    public abstract class GUIMultiAnimator : GUIAnimator
    {
        internal List<GUIAnimator> multiAnimators;
        protected bool[] animatorsFinished;

        public GUIMultiAnimator()
        {
            multiAnimators = new List<GUIAnimator>();
        }

        public void setAnimators(params GUIAnimator[] multiAnimators)
        {
            this.multiAnimators = new List<GUIAnimator>(multiAnimators);
        }

        internal void setAnimators(List<GUIAnimator> multiAnimators)
        {
            this.multiAnimators = multiAnimators;
        }

        public void addAnimator(GUIAnimator animator)
        {
            this.multiAnimators.Add(animator);
        }

        internal bool stopped = false;

        public void stopCascading()
        {
            stopped = true;
        }
    }

}