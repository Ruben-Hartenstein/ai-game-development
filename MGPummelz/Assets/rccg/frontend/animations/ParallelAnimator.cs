using System.Collections;

namespace RelegatiaCCG.rccg.frontend.animations
{

    public class ParallelAnimator : GUIMultiAnimator
    {
        public bool waitForFinish = true;

        private int currentSubAnimatorID;

        internal void subAnimatorCallback(int id)
        {
            //Debug.LogError("Parallel callback " + id);
            animatorsFinished[id] = true;

            bool finished = true;
            for(int i = 0; i < animatorsFinished.Length; i++ )
            {
                if(!animatorsFinished[i])
                {
                    finished = false;
                    break;
                }
            }


            if(finished)
            {
                //Debug.LogError("Parallel finished " + id);
                if (waitForFinish)
                {
                    finishedAnimation();
                }
                Destroy(this);
            }
                
            
        }

        public override IEnumerator startAnimationInternal()
        {
            if(multiAnimators == null || multiAnimators.Count == 0)
            {
                finishedAnimation();
                return null;
            }

            //Debug.LogError("starting parallel animation " + subAnimators);

            this.animatorsFinished = new bool[multiAnimators.Count];

            for(int i = 0; i < multiAnimators.Count; i++)
            {
                currentSubAnimatorID = multiAnimators[i].id;
                AniF.ani().startAni(multiAnimators[i], this.subAnimatorCallback, i);
            }
            
            if(!waitForFinish)
            {
                finishedAnimation();
            }
            
            return null;
        }
    }
       
}