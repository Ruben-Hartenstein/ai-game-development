using UnityEngine;

namespace RelegatiaCCG.rccg.frontend.animations
{
    public class ParticleSubAnimator : GUISubAnimator
    {
        private ParticleSystem _particleSystem;

        private ParticleSystem getParticleSystem()
        {
            if (this._particleSystem == null)
            {
                _particleSystem = this.gameObject.GetComponent<ParticleSystem>();
            }
            return _particleSystem;
        }

        internal bool stayWithAnimatorPosition = false;

        void Awake()
        {
            getParticleSystem();
        }

        public override void startSubAnimationInternal()
        {
            //Debug.LogError("Playing parrticle " + this.gameObject.name);
            getParticleSystem().Play();
        }

        public override void endSubAnimationInternal()
        {
            //Debug.LogError("Stopping parrticle " + this.gameObject.name);
            getParticleSystem().Stop();
        }

        protected override void destroyGO()
        {
            Destroy(this.gameObject, getParticleSystem().main.startLifetime.constantMax);
        }

        void Update()
        {
            if(subAnimationRunning)
            {
                if(stayWithAnimatorPosition)
                {
                    //Debug.LogError(parentAnimator.gameObject + "Updating stayWithAnimatorPosition " + parentAnimator.transform.position);
                    this.transform.position = parentAnimator.transform.position;
                }
            }
        }

        public ParticleSubAnimator followAnimator()
        {
            this.stayWithAnimatorPosition = true;
            return this;
        }


        public ParticleSubAnimator withSprite(int index, Sprite sprite, int scale)
        {


            getParticleSystem().textureSheetAnimation.SetSprite(index, sprite);
            ParticleSystem.MainModule mm = getParticleSystem().main;
            //the 100 stems from 100 pixels per unit, which is default for sprites
            //this could probably be done better with a better understanding of unitys unit system
            mm.startSize = sprite.bounds.size.y * 100 * scale;
            return this;
        }

        public ParticleSubAnimator withSprite(Sprite sprite, int scale)
        {
            return withSprite(0, sprite, scale);
        }
        public ParticleSubAnimator withSprites(Sprite[] sprites, int scale)
        {
            for(int i = 0; i < sprites.Length; i++)
            {
                withSprite(i, sprites[i], scale);
            }
            return this;
        }

        public ParticleSubAnimator withAdditionalSprite(Sprite sprite, int scale)
        {
            

            getParticleSystem().textureSheetAnimation.AddSprite(sprite);
            ParticleSystem.MainModule mm = getParticleSystem().main;
            //the 100 stems from 100 pixels per unit, which is default for sprites
            //this could probably be done better with a better understanding of unitys unit system
            mm.startSize = sprite.bounds.size.y * 100 * scale;
            return this;
        }

        internal ParticleSubAnimator withGradient(Gradient g)
        {
            ParticleSystem.ColorOverLifetimeModule colm = getParticleSystem().colorOverLifetime;
            colm.enabled = true;
            colm.color = g;
            return this;
        }
    }

}