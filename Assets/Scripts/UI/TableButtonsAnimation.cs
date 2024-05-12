using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class TableButtonsAnimation : MonoBehaviour
{
    public RectTransform content;
    private float spacing = -97;
    private float duration = 0.3f;


    void OnAnimationComplete()
    {
        //content.GetComponent<VerticalLayoutGroup>().spacing = 11;
    }

    public void ButtonsAnimation(List<GameObject> tableButtons)
    {
        float initialY = -43f;
        Sequence sequence = DOTween.Sequence();

        RectTransform firstLesson = tableButtons[0].GetComponent<RectTransform>();

        for (int i = 0; i < tableButtons.Count; i++)
        {
            RectTransform buttonRectTransform = tableButtons[i].GetComponent<RectTransform>();

            float targetY = initialY + (i * spacing);

            float delay = i * duration;     //apply the delay based on the index

            buttonRectTransform.position = new Vector2(buttonRectTransform.position.x, 1000f);


            //sequence.Append(buttonRectTransform.DOAnchorPosY(targetY, duration)
            //.SetDelay(delay)
            //.SetEase(Ease.OutQuad));

            buttonRectTransform.DOAnchorPosY(targetY, duration)
                .SetDelay(delay)
                .SetEase(Ease.OutQuad).OnComplete(() => OnAnimationComplete());

            //CustomAnimateYAxis(buttonRectTransform, targetY, delay, Ease.OutQuad);

        }
    }

    void CustomAnimateYAxis(RectTransform targetRectTransform, float targetY, float animDuration, Ease ease)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(targetRectTransform.DOAnchorPosY(targetY, animDuration).SetEase(ease));
        
        sequence.OnComplete(() => OnAnimationComplete());       //optional callback
    }
}
