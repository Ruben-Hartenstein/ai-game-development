using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MGSpriteAnimator : MonoBehaviour
{

    public Sprite[] sprites;

    public int[] poses;

    public float[] delays;

    public Image image;

    internal Coroutine animationCoroutine;

    public Boolean autostart = true;
    public Boolean repeat = true;

    public void changePose(int i)
    {
        this.image.sprite = sprites[i];
    }

    protected IEnumerator animate()
    {
        if (poses.Length < delays.Length)
        {
            Debug.LogError("Too many delays in NPC animation");
        }
        else if (poses.Length > delays.Length)
        {
            Debug.LogError("Too few delays in NPC animation");
        }

        do
        {


            for (int i = 0; i < poses.Length; i++)
            {
                yield return new WaitForSeconds(delays[i]);
                changePose(poses[i]);
            }
        }
        while (repeat);

        //this.animationCoroutine = null;
    }

    public void startAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
        StartCoroutine(animate());
    }

    private void Start()
    {
        if(autostart)
        {
            startAnimation();
        }
    }

    void OnDisable()
    {
        changePose(0);
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

}

