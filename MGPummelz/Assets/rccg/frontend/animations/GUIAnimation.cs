using rccg.frontend;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{


    public class GUIAnimation
    {

        public enum WaitMode
        {
            Interrupt,
            Queue,
            Skip
        }


        public WaitMode waitMode = WaitMode.Interrupt;

        public GUIAnimation skipIfBusy()
        {
            waitMode = WaitMode.Skip;
            return this;
        }


        public GUIAnimator animator;

        internal int callbackID;
        internal Action<int> finishedCallback;
        private List<GUISubAnimator> subAnimators;

        internal float timeout = -1;
        internal bool timedOut = false;

        public bool animationStarted = false;
        internal bool animationRunning { get { return animationStarted && !animationFinished; } }
        public bool animationFinished = true;

        public bool destroyAnimatorAtFinish = false;
        public bool destroyGameObjectAtFinish = false;

        public bool skipping;

        public IEnumerator startAnimation(Action<int> finishedCallback, int myID)
        {
            this.finishedCallback = finishedCallback;
            this.callbackID = myID;

            if (animator.animationRunning && waitMode == WaitMode.Skip)
            {
                finishedAnimation();
                yield return null;
            }
            else if (animator.animationRunning && waitMode == WaitMode.Queue)
            {
                yield return null;
            }
            else
            {
                if(animator.animationRunning)
                {
                    Debug.LogError("Interrupting running animation");
                }
                skipping = false;

                this.animationStarted = true;
                this.animationFinished = false;



                if (animator == null)
                {
                    Debug.LogWarning("Animator is null.");
                    yield return null;
                }
                else
                {
                    if (animationRunning)
                    {
                        //Debug.LogWarning("Starting animation " + animator.GetType() + " " + animator.gameObject + " even though it is already running!");
                    }

                    if (timeout > 0)
                    {

                    }

                    if (subAnimators != null)
                    {
                        foreach (GUISubAnimator subAni in subAnimators)
                        {
                            subAni.startSubAnimation();
                        }
                    }

                    yield return animator.startFromAnimation(this);
                }
            }


            

           
        }

        public virtual void startAnimationInternal()
        {

        }

        public IEnumerator startAnimation()
        {
            //NOTE: If this method is not called, you need to start it as a coroutine!
            //NOTE: if you get a null error here, remember that the AniF MonoBehaviour must be spawned at least a frame BEFORE starting the animation
            //Debug.LogError("starting animation without parameters");
            yield return startAnimation(GUIAnimator.emptyCallback, 0);
        }

        internal void finishedAnimation()
        {
            skipping = false;

            this.animationStarted = false;
            this.animationFinished = true;

            if (subAnimators != null)
            {
                foreach (GUISubAnimator subAni in subAnimators)
                {
                    subAni.endSubAnimation();
                }
                this.subAnimators = null;
            }

            //Debug.Log("Finishing animation " + this.GetType() + " with callback " + this.finishedCallback);
            if (!timedOut)
            {
                if (this.finishedCallback != null)
                {
                    //Debug.Log("Calling callback with id " + id);
                    this.finishedCallback(callbackID);
                }

            }

            if (destroyGameObjectAtFinish)
            {
                animator.destroyGO();
            }
            else if (destroyAnimatorAtFinish)
            {
                animator.destroyMe();
            }
        }


        private IEnumerator timeoutCoroutine()
        {
            yield return new WaitForSeconds(this.timeout);
            if (!animationFinished)
            {
                this.animationStarted = false;
                this.animationFinished = true;
                this.timedOut = true;
                Debug.LogWarning("Animation " + animator.gameObject.name + " with id " + callbackID + " timed out.");

                if (this.finishedCallback != null)
                {
                    this.finishedCallback(callbackID);
                }



            }

        }



        public GUIAnimation withSubAnimator(GUISubAnimator subAni)
        {
            if (this.subAnimators == null)
            {
                this.subAnimators = new List<GUISubAnimator>();
            }
            if (subAni == null)
            {
                Debug.LogError("Adding null subanimator");
            }
            this.subAnimators.Add(subAni);
            subAni.registeredAt(this.animator);
            return this;
        }

        //public GUIAnimation withParticleTrail(AniFParticles particle)
        //{

        //    UnityEngine.Object prefab = GUIResourceLoader.getResourceLoaderInstance().loadParticlePrefab(particle);

        //    Debug.LogError(prefab);
        //    ParticleSubAnimator particleAni = animator.instantiatePrefab(prefab).GetComponent<ParticleSubAnimator>();
        //    particleAni.followAnimator();
        //    particleAni.destroyGameObjectAfterwards();
        //    this.withSubAnimator(particleAni);

        //    return this;
        //}

        public GUIAnimation withParticle(ParticleSubAnimator particleAni)
        {
            return withParticle(particleAni, animator.transform.parent);
        }

        public GUIAnimation withParticle(ParticleSubAnimator particleAni, Transform parent)
        {
            particleAni.transform.SetParent(parent, false);
            this.withSubAnimator(particleAni);

            return this;
        }

  
        public void skip()
        {
            if (animationRunning)
            {
                this.skipping = true;
            }
        }


    }
    

       

   


}