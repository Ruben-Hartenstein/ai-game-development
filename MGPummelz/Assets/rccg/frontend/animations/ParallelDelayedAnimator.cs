using System.Collections;
using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{

    public class ParallelDelayedAnimator : GUIMultiAnimator
    {
        public float delay;

        public bool waitFinished;

        public GUIAnimator currentAnimator;

        public bool animationStarted;
        public bool animationFinished;

        internal void subAnimatorCallback(int id)
        {
            //Debug.LogError("Parallel callback " + id);
            animatorsFinished[id] = true;

            if(waitFinished)
            {
                bool finished = true;
                for (int i = 0; i < animatorsFinished.Length; i++)
                {
                    if (!animatorsFinished[i])
                    {
                        finished = false;
                        break;
                    }
                }


                if (finished)
                {
                    //Debug.LogError("Parallel delayed finished " + id);
                    animationFinished = true;
                    animationStarted = false;
                    this.finishedAnimation();
                    Destroy(this);
                }
            }
            
        }


        private IEnumerator delayedStartup()
        {
            //Debug.LogError("Parallel delayed started " + id);
            for (int i = 0; i < multiAnimators.Count; i++)
            {
                if(!stopped)
                {
                    AniF.ani().startAni(multiAnimators[i], this.subAnimatorCallback, i);
                    currentAnimator = multiAnimators[i];
                }
                
                yield return new WaitForSeconds(delay);
            }
            if(!waitFinished)
            {
                finishedAnimation();
            }
        }

        public override IEnumerator startAnimationInternal()
        {
            if (multiAnimators == null || multiAnimators.Count == 0)
            {
                finishedAnimation();
                return null;
            }
            
            this.animatorsFinished = new bool[multiAnimators.Count];

            StartCoroutine(delayedStartup());

            animationStarted = true;
            animationFinished = false;

            return null;
        }
    }
       
}