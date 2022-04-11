using rccg.frontend.animations;
using UnityEngine;
using UnityEngine.EventSystems;

public class MGTextField : MonoBehaviour, IPointerClickHandler
{
    public MGMinigame miniGame;

    public TextByLetterAnimator textAnimator;

    public string id;

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO: minigame
    }

    internal void setText(string text)
    {
        textAnimator.setFixedSpeed(0.3f);
        textAnimator.reset(text);
        StartCoroutine(textAnimator.startAnimation());
    }
}
