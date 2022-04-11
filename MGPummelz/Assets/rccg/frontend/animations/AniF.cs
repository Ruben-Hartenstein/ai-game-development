using rccg.frontend;
using System;
using System.Collections;
using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{

    public class AniF : MonoBehaviour
    {
        public static AniF ani()
        {
            //return CommonController.getAniF();
            throw new NotImplementedException();
        }

        public void startAni(GUIAnimator animator)
        {
            StartCoroutine(startDelayed(animator));
        }

        public void startAni(GUIAnimation animation)
        {
            StartCoroutine(startDelayed(animation));
        }

        public void startAni(GUIAnimation animation, Action<int> callback, int myID)
        {
            StartCoroutine(startDelayed(animation, callback, myID));
        }

        private IEnumerator startDelayed(GUIAnimator animator)
        {
            yield return null;
            yield return animator.startAnimation();

        }

        public GUIAnimator startAni(GUIAnimator animator, Action<int> callback, int myID)
        {
            StartCoroutine(startDelayed(animator, callback, myID));
            return animator;
        }

        private IEnumerator startDelayed(GUIAnimator animator, Action<int> callback, int myID)
        {
            yield return null;
            yield return animator.startAnimation(callback, myID);

        }

        private IEnumerator startDelayed(GUIAnimation animation)
        {
            yield return null;
            yield return animation.startAnimation();

        }

        private IEnumerator startDelayed(GUIAnimation animation, Action<int> callback, int myID)
        {
            yield return null;
            yield return animation.startAnimation(callback, myID);

        }


        public WaitAnimator wait(float waitSeconds)
        {
            WaitAnimator waitA = this.gameObject.AddComponent<WaitAnimator>();
            waitA.waitSeconds = waitSeconds;
            waitA.destroyMeAtFinish = true;
            return waitA;
        }


        public SequentialAnimator seq(params GUIAnimator[] animators)
        {
            SequentialAnimator seqA = this.gameObject.AddComponent<SequentialAnimator>();
            seqA.setAnimators(animators);
            return seqA;
        }

        public ParallelAnimator par(params GUIAnimator[] animators)
        {
            ParallelAnimator parA = this.gameObject.AddComponent<ParallelAnimator>();
            parA.setAnimators(animators);
            return parA;
        }

        public ParallelDelayedAnimator parDelayed(float delayInSeconds, bool waitFinished, params GUIAnimator[] animators)
        {
            ParallelDelayedAnimator parA = this.gameObject.AddComponent<ParallelDelayedAnimator>();
            parA.delay = delayInSeconds;
            parA.waitFinished = waitFinished;
            parA.setAnimators(animators);
            return parA;
        }

        public ParallelDelayedAnimator parDelayed(float delayInSeconds, bool waitFinished)
        {
            ParallelDelayedAnimator parA = this.gameObject.AddComponent<ParallelDelayedAnimator>();
            parA.delay = delayInSeconds;
            parA.waitFinished = waitFinished;
            return parA;
        }

        //public ParticleSubAnimator particle(AniFParticles particle)
        //{

        //    UnityEngine.Object prefab = GUIResourceLoader.getResourceLoaderInstance().loadParticlePrefab(particle);

        //    ParticleSubAnimator particleAni = (Instantiate(prefab) as GameObject).GetComponent<ParticleSubAnimator>();
        //    particleAni.destroyGameObjectAfterwards();

        //    return particleAni;
        //}

        //public ParticleSubAnimator particle(AniFParticles particle, Transform parent)
        //{

        //    UnityEngine.Object prefab = GUIResourceLoader.getResourceLoaderInstance().loadParticlePrefab(particle);

        //    ParticleSubAnimator particleAni = (Instantiate(prefab, parent) as GameObject).GetComponent<ParticleSubAnimator>();
        //    particleAni.destroyGameObjectAfterwards();

        //    return particleAni;
        //}

        internal CoroutineGUIAnimator coroutine(IEnumerator coroutine)
        {
            CoroutineGUIAnimator cga = this.gameObject.AddComponent<CoroutineGUIAnimator>().withCoroutine(coroutine);
            return cga;
        }
    }
       
}