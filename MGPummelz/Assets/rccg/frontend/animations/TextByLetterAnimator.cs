using RelegatiaCCG.rccg.frontend.animations;
using System.Collections;
using TMPro;
using UnityEngine;

namespace rccg.frontend.animations
{
    public class TextByLetterAnimator : GUIAnimator
    {
        TextMeshProUGUI text;

        private GameObject sfxObject;
        
        private int revealedCharacters;
        private int totalCharacters;

        private float elapsedTime;

        private bool useFixedSpeed;

        private float currentSpeed = 1.0f;
        private float currentFixedSpeed = 5.0f;



        public void Awake()
        {
            initialize();
            this.reset();
        }

        private bool initialized;

        private void initialize()
        {
            if(!initialized)
            {
                this.text = gameObject.GetComponent<TextMeshProUGUI>();
                initialized = true;
            }
            
        }

        public TextByLetterAnimator reset()
        {
            if(this.text == null)
            {
                this.text = gameObject.GetComponent<TextMeshProUGUI>();
            }
            terminateAnimation();
            text.maxVisibleCharacters = 0;
            return this;
        }

        public TextByLetterAnimator reset(string newText)
        {
            initialize();
            text.text = newText;
            return reset();
        }

        public void disable()
        {
            initialize();
            text.maxVisibleCharacters = 99999;
            terminateAnimation();
        }

        public void setSpeedFromConfig()
        {
         
            this.setVariableSpeed(40);
         
        }

        public TextByLetterAnimator setVariableSpeed(float lettersPerSecond)
        {
            useFixedSpeed = false;
            currentSpeed = lettersPerSecond;
            return this;
        }

        public TextByLetterAnimator setFixedSpeed(float totalTime)
        {
            useFixedSpeed = true;
            currentFixedSpeed = totalTime;
            return this;
        }

        public override IEnumerator startAnimationInternal()
        {

            this.elapsedTime = 0;
            this.totalCharacters = text.textInfo.characterCount;

            return null;
        }

        public void internalCallback()
        {
            this.finishedAnimation();
        }

    

        public void Update()
        {
            if(animationRunning)
            {
                float deltaTime = getAdjustedDeltaTime();

                elapsedTime += deltaTime;

                int newRevealedCharacters;
                
                if (useFixedSpeed)
                {
                    newRevealedCharacters = (int)((float)totalCharacters * (elapsedTime / currentFixedSpeed));
                }
                else
                {
                    newRevealedCharacters = (int)(currentSpeed * elapsedTime);
                    
                }

                //only rebuild text mesh if something actually changes
                if(newRevealedCharacters != revealedCharacters)
                {
                    revealedCharacters = newRevealedCharacters;
                    if (revealedCharacters <= totalCharacters)
                    {
                        text.maxVisibleCharacters = revealedCharacters;
                        //Debug.Log(text.maxVisibleCharacters);
                    }
                    else
                    {
                        text.maxVisibleCharacters = 99999;
                        if(sfxObject != null)
                        {
                            Destroy(sfxObject);
                         
                        }
                        finishedAnimation();
                    }
                }

                
            }
        }

        internal void terminateAnimation()
        {
            text.maxVisibleCharacters = 99999;
            if (sfxObject != null)
            {
                Destroy(sfxObject);
   
            }
            if (animationRunning)
            {
                finishedAnimation();
            }
        }

    }

}

