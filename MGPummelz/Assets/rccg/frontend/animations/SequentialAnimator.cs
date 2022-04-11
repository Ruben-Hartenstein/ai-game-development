using System.Collections;
using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{

    public class SequentialAnimator : GUIMultiAnimator
    {
        private int currentSubAnimatorID;
        
        public SequentialAnimator()
        {

        }

        internal void subAnimatorCallback(int callbackID)
        {
            //Debug.Log("sequential callback " + callbackID + " for:" + multiAnimators[currentSubAnimatorID].GetType());

            if (stopped)
            {
                finishedAnimation();
                Destroy(this);
                return;
            }

            if (callbackID >= animatorsFinished.Length)
            {
                Debug.LogError("Sequential callback out of range " + callbackID + "/" + (animatorsFinished.Length - 1) + " in seqAnim " + id);
            }

            this.animatorsFinished[callbackID] = true;

            if (callbackID != currentSubAnimatorID)
            {
                Debug.LogError("Out of sequence callback " + callbackID + " expected:" + currentSubAnimatorID);
            }
            currentSubAnimatorID++;
            if(currentSubAnimatorID < multiAnimators.Count)
            {
                GUIAnimator animator = multiAnimators[currentSubAnimatorID];
                if(animator != null)
                {
                    GUIAnimator currAnim = multiAnimators[currentSubAnimatorID];
                    //Debug.LogError("currAnim " + currAnim + " with id " + currentSubAnimatorID + " started from " + this.id);
                    AniF.ani().startAni(currAnim, this.subAnimatorCallback, currentSubAnimatorID);
                }
                else
                {
                    //if we have no Animator, we do the callback directly
                    //TODO: this is not nice
                    subAnimatorCallback(currentSubAnimatorID);
                }

                
            }
            else
            {
                finishedAnimation();
                Destroy(this);
            }
        }

        public override IEnumerator startAnimationInternal()
        {
            if(multiAnimators.Count == 0)
            {
                //Debug.LogError("Empty sequential animator callback");
                finishedAnimation();
                return null;
            }

            //Debug.LogError("starting sequential animation");

            this.animatorsFinished = new bool[multiAnimators.Count];

            this.currentSubAnimatorID = 0;
            if(multiAnimators[0] != null)
            {
                //Debug.LogError("subAnimators[0] " + subAnimators[0]);
                AniF.ani().startAni(multiAnimators[0], this.subAnimatorCallback, 0);
            }
            else
            {
                Debug.LogError("null animator callback");
                //TODO: this is not nice
                subAnimatorCallback(0);
            }
            
            return null;
        }

        
    }
       
}