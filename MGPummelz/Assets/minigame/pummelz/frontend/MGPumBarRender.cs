using RelegatiaCCG.rccg.frontend.animations;
using System.Collections;


using UnityEngine;
using UnityEngine.UI;

public class MGPumBarRender : GUIAnimator
{
    public Image bar;

    public Sprite[] bars;

    public int barSize;

    internal float currentBarPercentage;
    internal float targetBarPercentage;
    internal float totalBarPercentage;

    private bool useFixedSpeed = true;

    private float currentSpeed = 4.0f;
    private float currentFixedSpeed = 1.0f;

    private float elapsedTime;

    void Start()
    {

    }

    private void updateBar()
    {
        if (currentBarPercentage >= 0.5f)
        {
            bar.sprite = bars[0];
        }
        else if (currentBarPercentage >= 0.25f)
        {
            bar.sprite = bars[1];
        }
        else
        {
            bar.sprite = bars[2];
        }
        bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentBarPercentage * barSize);
    }

    public void setBar(float percentage)
    {
        currentBarPercentage = percentage;
        targetBarPercentage = percentage;
        updateBar();
    }

    public MGPumBarRender moveBarTo(float percentage)
    {
        targetBarPercentage = percentage;
        //Debug.LogError("Moving bar from " + currentBarPercentage + " to " + targetBarPercentage);
        return this;
    }

    void Update()
    {
        if (animationRunning)
        {
            float deltaTime = getAdjustedDeltaTime();

            elapsedTime += deltaTime;

            float barIncrease;

            if (useFixedSpeed)
            {
                barIncrease = ((float)totalBarPercentage * (elapsedTime / currentFixedSpeed));
            }
            else
            {
                barIncrease = (currentSpeed * elapsedTime);

            }


            if (currentBarPercentage < targetBarPercentage)
            {
                currentBarPercentage += barIncrease;
                if (currentBarPercentage > targetBarPercentage)
                {
                    currentBarPercentage = targetBarPercentage;
                }
                updateBar();
                if (currentBarPercentage == targetBarPercentage)
                {
                    finishedAnimation();
                }
            }
            else if (currentBarPercentage > targetBarPercentage)
            {
                currentBarPercentage -= barIncrease;
                if (currentBarPercentage < targetBarPercentage)
                {
                    currentBarPercentage = targetBarPercentage;
                }
                updateBar();
                if (currentBarPercentage == targetBarPercentage)
                {
                    finishedAnimation();
                }
            }


        }

    }

    public override IEnumerator startAnimationInternal()
    {
        this.totalBarPercentage = Mathf.Abs(targetBarPercentage - currentBarPercentage);

        return null;
    }

    internal void terminateAnimation()
    {
        if (animationRunning)
        {
            finishedAnimation();
        }
    }
}



