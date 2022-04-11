using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{


    public abstract class GUIAnimator : MonoBehaviour
    {
        public class QueueItem
        {
            public QueueItem(GUIAnimation animation, Action<int> finishedCallback, int callbackID)
            {
                this.animation = animation;
                this.callbackID = callbackID;
                this.finishedCallback = finishedCallback;
            }
            public GUIAnimation animation;
            public int callbackID;
            public Action<int> finishedCallback;
        }

        protected Queue<QueueItem> queuedAnimations;

        protected GUIAnimation _legacyAnimation;

        protected GUIAnimation legacyAnimation
        {
            set
            {
                _legacyAnimation = value;
            }
            get
            {
                if(_legacyAnimation == null)
                {
                    _legacyAnimation = newAnimation();
                }
                return _legacyAnimation;
            }

        }

        internal bool animationRunning { get { return runningAnimation != null && runningAnimation.animationRunning; } }
        internal bool animationStarted { get { return runningAnimation != null && runningAnimation.animationStarted; } }
        internal bool animationFinished { get { return runningAnimation == null || runningAnimation.animationFinished; } }
        internal bool destroyMeAtFinish { set { legacyAnimation.destroyAnimatorAtFinish = value; } }
        internal bool destroyGameObjectAtFinish { set { legacyAnimation.destroyGameObjectAtFinish = value; } }

        internal float timeout { set { legacyAnimation.timeout = value; } }

        internal int id { get { if (runningAnimation != null) { return runningAnimation.callbackID; } else { return legacyAnimation.callbackID; } } }

        public virtual T getNewAnimation<T>() where T : GUIAnimation
        {
            return (T)newAnimation();
        }

        protected virtual GUIAnimation newAnimation()
        {
            GUIAnimation animation = new GUIAnimation();
            animation.animator = this;
            return animation;
        }

        internal GUIAnimation runningAnimation;

        public IEnumerator startAnimation()
        {
            //NOTE: If this method is not called, you need to start it as a coroutine!
            //NOTE: if you get a null error here, remember that the AniF MonoBehaviour must be spawned at least a frame BEFORE starting the animation
            //Debug.LogError("starting animation without parameters");
            yield return startAnimation(emptyCallback, 0);
        }

        public void queueAnimation(GUIAnimation animation, Action<int> finishedCallback, int callbackID)
        {
            if(animationRunning)
            {
                if(queuedAnimations == null)
                {
                    queuedAnimations = new Queue<QueueItem>();
                }
                queuedAnimations.Enqueue(new QueueItem(animation, finishedCallback, callbackID));
            }
            else
            {
                //start directly if not running
                StartCoroutine(animation.startAnimation(finishedCallback, callbackID));
            }
        }


        public IEnumerator startAnimation(Action<int> finishedCallback, int myID)
        {
            yield return legacyAnimation.startAnimation(finishedCallback, myID);
        }

        internal IEnumerator startFromAnimation(GUIAnimation animation)
        {
            if(!animation.animationRunning)
            {
                Debug.LogWarning("Started a non-running animation. Do not call this directly. Instead call animation.");
            }

            runningAnimation = animation;
            yield return startAnimationInternal();
        }

        public abstract IEnumerator startAnimationInternal();
        
        
       

        protected void finishedAnimation()
        {
            if(runningAnimation != null)
            {
                runningAnimation.finishedAnimation();
            }
            if(queuedAnimations != null && queuedAnimations.Count > 0)
            {
                QueueItem qi = queuedAnimations.Dequeue();

                StartCoroutine(qi.animation.startAnimation(qi.finishedCallback, qi.callbackID));
            }
        }

        public static void emptyCallback(int id)
        {

        }

        public static void loggingCallback(int id)
        {
            Debug.Log("Callback logged:" + id);
        }

        public float getAdjustedDeltaTime()
        {
            float deltaTime = Time.deltaTime;


            if (runningAnimation != null && runningAnimation.skipping)
            {
                deltaTime *= 10;
            }
       
            
            return deltaTime;
        }

        

        public GUIAnimator withSubAnimator(GUISubAnimator subAni)
        {
            legacyAnimation.withSubAnimator(subAni);
            return this;
        }


        internal virtual void destroyGO()
        {
            Destroy(this.gameObject);
        }

        internal virtual void destroyMe()
        {
            Destroy(this);
        }

        public void skip()
        {
            runningAnimation.skip();
        }

        internal GameObject instantiatePrefab(UnityEngine.Object prefab)
        {
            return (Instantiate(prefab, this.transform.parent, false) as GameObject);
        }

    }

   


}