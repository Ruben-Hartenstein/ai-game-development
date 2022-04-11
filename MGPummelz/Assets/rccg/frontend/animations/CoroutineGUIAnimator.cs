using System.Collections;




namespace RelegatiaCCG.rccg.frontend.animations
{



    public class CoroutineGUIAnimator : GUIAnimator
    {
        private IEnumerator coroutine;

        public CoroutineGUIAnimator()
        {
            
        }

        public CoroutineGUIAnimator withCoroutine(IEnumerator coroutine)
        {
            this.coroutine = coroutine;
            return this;
        }


        public override IEnumerator startAnimationInternal()
        {
            StartCoroutine(coroutine);
            finishedAnimation();
            yield return null;
        }

    }

}