using System.Collections;
using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{



    public class WaitAnimator : GUIAnimator
    {
        internal float waitSeconds;

        public WaitAnimator()
        {

        }

        private IEnumerator wait()
        {
           
            yield return new WaitForSeconds(this.waitSeconds);


            finishedAnimation();
        }

        public override IEnumerator startAnimationInternal()
        {
            StartCoroutine(wait());

            return null;
        }

      

    }

    

}