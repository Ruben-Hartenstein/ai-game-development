using UnityEngine;
using UnityEngine.UI;

namespace RelegatiaCCG.rccg.frontend.animations
{

    public class ImageAnimator : CurveAnimator
    {
       

        public GUIAnimPath path;

       

        internal ImageAnimatorState homeState;

        internal ImageAnimatorState oldState;

        internal ImageAnimatorState newState;

        internal ImageAnimatorState diffState;

        internal bool dontChangePosition = false;

        //private bool faceDown;

        private Image image;

        public ImageAnimator()
        {
        }

        internal bool initialized;

        protected void initialize()
        {
            if(!initialized)
            {
                homeState = new ImageAnimatorState(this.transform);
                oldState = homeState.deepCopy();
                newState = homeState.deepCopy();
                image = gameObject.GetComponent<Image>();
                initialized = true;
            }

            
        }

        public virtual void Awake()
        {
            //curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
            initialize();

        }

        internal ImageAnimator fadeIn()
        {
            newState.alpha = 1.0f;
            return this;
        }

        internal ImageAnimator fadeOut()
        {
            newState.alpha = 0.0f;
            return this;
        }

      

        public ImageAnimator setPath(GUIAnimPath path)
        {
            this.path = path;
            return this;
        }

        public virtual SwitchModeImageAnimator getFollowUpAnimator()
        {
            SwitchModeImageAnimator swAnimator = this.gameObject.AddComponent<SwitchModeImageAnimator>() as SwitchModeImageAnimator;
            swAnimator.setImageAnimator(this);
            return swAnimator;
        }

        internal void setVisibleImmediately()
        {
            this.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
            this.oldState.alpha = 1.0f;
            this.newState.alpha = 1.0f;
        }

        internal void setScaleImmediately(float scale)
        {
            Vector3 scaleVector = new Vector3(scale, scale, scale);
            oldState.scale = scaleVector;
            newState.scale = scaleVector;
            this.gameObject.transform.localScale = scaleVector;
        }

        internal ImageAnimator zoomToScale(float scale)
        {
            newState.scale = new Vector3(scale, scale, scale);
            return this;
        }

        internal ImageAnimator zoomToScale(Vector3 scale)
        {
            newState.scale = scale;
            return this;
        }

        internal void setInvisibleImmediately()
        {
            //Debug.LogError(this.gameObject.GetComponent<CanvasGroup>());
            //Debug.LogError(oldState);
            //Debug.LogError(newState);
            initialize();
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
            this.oldState.alpha = 0.0f;
            this.newState.alpha = 0.0f;
        }

    

        internal ImageAnimator moveToX(float x)
        {
            newState.pos.x = x;
            return this;
        }

        

        internal ImageAnimator moveXRelative(float x)
        {
            //Debug.LogError("relative x= " + x);
            newState.pos.x += x;
            return this;
        }

        internal ImageAnimator moveYRelative(float y)
        {
            newState.pos.y += y;
            
            return this;
        }


        internal ImageAnimator rotateYRelative(float y)
        {
            newState.rotationY += y;
            return this;
        }

        internal ImageAnimator moveToHome()
        {
            this.newState = homeState.deepCopy();
            return this;
        }

        public ImageAnimator moveToTransform(GameObject nextParent, GameObject newPlaceholder)
        {
            this.newState.parent = nextParent.transform;
            this.newState.pos = newPlaceholder.transform.localPosition;
            return this;
        }

        public ImageAnimator moveToPosition(Vector3 newPos)
        {
            this.newState.pos = newPos;
            return this;
        }

        public ImageAnimator moveToPosition(Vector3 newPos, Vector3 newScale, float newAlpha)
        {
            this.newState.pos = newPos;
            this.newState.scale = newScale;
            this.newState.alpha = newAlpha;
            return this;
        }

        public ImageAnimator moveToGlobalPosition(Vector3 newPos, Vector3 newScale, float newAlpha)
        {


            this.newState.pos = this.transform.parent.InverseTransformPoint(newPos);
            this.newState.scale = newScale;
            this.newState.alpha = newAlpha;
            return this;
        }

        public ImageAnimator moveToGlobalPosition(Vector3 newPos)
        {


            this.newState.pos = this.transform.parent.InverseTransformPoint(newPos);
            return this;
        }

     

     

        void Start()
        {
            //trailEmitter().activateTrail();
        }

        internal void moveInstant(Transform parent, Vector3 position, Vector3 scale, bool visible)
        {
            CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                if (visible)
                {
                    gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
                }
                else
                {
                    gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
                }
            }
            if(!dontChangePosition)
            {
                this.transform.localPosition = position;
            }
            this.transform.localScale = scale;
            this.transform.parent = parent;
            this.oldState = new ImageAnimatorState(this.transform);
        }

        protected override void updateCurveExtension(float deltaTime, float curveValue)
        {
            if(diffState == null)
            {
                diffState = new ImageAnimatorState(oldState, newState);
            }
            //Debug.LogError("updateExtension " + gameObject.name);
            //Debug.LogError(" diffScale:" + diffState.scale + " curveVl:" + curveValue);
            transform.localScale = oldState.scale + diffState.scale * curveValue;
            //transform.localPosition = oldState.pos + diffState.pos * curveValue;
            if(path != null)
            {
                if (!dontChangePosition)
                {
                    transform.localPosition = path.Evaluate(curveValue);
                }
            }
            //Debug.LogError("path.Evaluate(curveValue);" + path.Evaluate(curveValue));


            if (diffState.alpha != 0.0f)
            {
                CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.alpha = oldState.alpha + diffState.alpha * curveValue;
                    //Debug.LogError("cg.alpha " + cg.alpha);
                }

            }


            if (diffState.rotationY != 0.0f)
            {
                float currentRotationY = oldState.rotationY + diffState.rotationY * curveValue;
                transform.localRotation = Quaternion.Euler(0, currentRotationY, 0);
            }
        }

        protected override void finishCurveExtension()
        {
            applyState(newState);
            oldState = newState.deepCopy();

            path = null;
        }

        protected override float calcDistExtension()
        {
            return diffState.pos.magnitude;
        }

        protected override void startAnimationCurveExtension()
        {
            if(this == null)
            {
                return;
            }

            //Debug.LogError("starting imageanim " + homeState.pos.x + "  " + newState.pos.x);
            if (path == null)
            {
                path = new GUILinearAnimPath(oldState.pos, newState.pos);
            }

            oldState = new ImageAnimatorState(this.transform);

            //Debug.LogError("Setting diffstate for " + gameObject.name);
            diffState = new ImageAnimatorState(oldState, newState);
        }

           internal void applyState(ImageAnimatorState state)
        {
            this.transform.localScale = state.scale;

            if (!dontChangePosition)
            {
                this.transform.localPosition = state.pos;
            }
            
            this.transform.SetParent(state.parent);
            //Debug.LogError("state.rotationY " + state.rotationY);
            this.transform.localRotation = Quaternion.Euler(0, state.rotationY, 0);
            CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
            if(cg != null)
            {
                cg.alpha = state.alpha;
            }
            oldState = state.deepCopy();
            newState = state.deepCopy();

        }

      
        internal void reset()
        {
            initialize();
            terminateAnimation();
            this.applyState(homeState);
        }

        internal void setCurrentAsHome()
        {
            this.homeState = new ImageAnimatorState(this.transform);
        }

        internal void setCurrentRotationAsHomeOldNew()
        {
            this.homeState.rotationY = transform.eulerAngles.y;
            this.oldState.rotationY = transform.eulerAngles.y;
            this.newState.rotationY = transform.eulerAngles.y;
        }

        internal void setCurrentAsHomeOldNew()
        {
            this.homeState = new ImageAnimatorState(this.transform);
            this.oldState = this.homeState.deepCopy();
            this.newState = this.homeState.deepCopy();
        }

        internal void setCurrentAsOldNew()
        {
            this.oldState = new ImageAnimatorState(this.transform);
            this.newState = this.oldState.deepCopy();
        }

        internal void moveInstant()
        {
            initialize();
            terminateAnimation();
            applyState(newState);
        }

        public void updateParent(Transform parent)
        {
            this.transform.SetParent(parent);
            oldState.parent = parent;
            oldState.pos = this.transform.localPosition;
            newState.parent = parent;
            newState.pos = this.transform.localPosition;
        }

        public void updateParentAtInit(Transform parent)
        {
            this.transform.parent = parent;
            homeState.parent = parent;
            oldState.parent = parent;
            newState.parent = parent;
        }
    }
}