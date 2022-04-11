using System.Collections;




namespace RelegatiaCCG.rccg.frontend.animations
{

    public class SwitchModeImageAnimator : ImageAnimator
    {
        private ImageAnimator imageAnimator;

        

        public SwitchModeImageAnimator()
        {
            
        }
        
        public void setImageAnimator(ImageAnimator ia)
        {
            this.imageAnimator = ia;
            this.homeState = ia.newState.deepCopy();
            oldState = homeState.deepCopy();
            newState = homeState.deepCopy();
            this.initialized = true;
        }

        public override SwitchModeImageAnimator getFollowUpAnimator()
        {
            SwitchModeImageAnimator swAnimator = this.gameObject.AddComponent<SwitchModeImageAnimator>() as SwitchModeImageAnimator;
            swAnimator.imageAnimator = this.imageAnimator;
            swAnimator.homeState = this.newState.deepCopy();
            swAnimator.oldState = this.newState.deepCopy();
            swAnimator.newState = this.newState.deepCopy();
            swAnimator.initialized = true;
            return swAnimator;
        }

        public void imageAnimFinished(int myID)
        {
            //Debug.LogError("finished switch mode with id " + id);
            finishedAnimation();
            Destroy(this);
        }

        public override IEnumerator startAnimationInternal()
        {
            //Debug.LogError("starting switchmode for " + gameObject.name);
            imageAnimator.newState = this.newState;
            //if(runningCurveAnimation.useFixedSpeed)
            //{
            //    imageAnimator.setFixedSpeed(runningCurveAnimation.currentFixedSpeed);
            //}
            //else
            //{
            //    imageAnimator.setVariableSpeed(this.currentSpeed);
            //}
            //if(this.curve != null)
            //{
            //    imageAnimator.setCurve(this.curve);
            //}

            startAnimation();
            AniF.ani().startAni(runningAnimation);

            yield return null;
        }

        //nothing to do here
        public override void Update()
        {
            
        }

        //nothing to do here
        protected override void updateCurveExtension(float deltaTime, float curveValue)
        {

        }
    }
}