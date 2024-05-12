using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YaxisAnimation : MonoBehaviour
{
    public RectTransform targetRectTransform;
    public float targetY = 0f; // Target Y position
    public float duration = 2f; // Animation duration
    public bool isOptions, isLessons;
    public RectTransform[] options;
    public bool trueFlase;
    public int num;
    public float[] optTargets;
    public bool isQuestion;
    public Vector2 actualPosition;
    public GameObject optionsBlocker;
    public bool isAnimated;

    void OnEnable()
    {
        if (optionsBlocker)
        {
            optionsBlocker.SetActive(true);
        }

        for (int i = 0; i < options.Length; i++)
        {    
            options[i].gameObject.SetActive(false);
        }

        // Call the AnimateYAxis function when the script starts
        if (isOptions)
        {
            if (RefrenceManager.instance.gameManager.isFirstQuestion)
            {
                Invoke(nameof(Animate_Options), 2f);
            }
            else
            {
                Invoke(nameof(Animate_Options), 1f);
            }
        }
        else if(isQuestion && RefrenceManager.instance.gameManager.isFirstQuestion)
        {
            targetRectTransform = GetComponent<RectTransform>();
            AnimateQuestionTypeToOriginal();
            Invoke(nameof(AnimateYAxis), 1f);
            Invoke(nameof(CallIsFirstQuestion), 3f);
        }
        else
        {
            targetRectTransform = GetComponent<RectTransform>();
            AnimateYAxis();
        }

    }

    void CallIsFirstQuestion()
    {
        RefrenceManager.instance.gameManager.FirstQuestionFalse();
    }

    void AnimateYAxis()
    {
        // Use DOTween to animate the position along the Y-axis
        targetRectTransform.DOAnchorPosY(targetY, duration)
        .SetEase(Ease.OutQuad) // You can change the ease type as per your requirement
        .OnComplete(OnAnimationComplete);  // Optional callback when the animation is complete
    }

    void AnimateQuestionTypeToOriginal()
    {
        targetRectTransform.DOAnchorPosY(actualPosition.y, 0f).SetEase(Ease.OutQuad);
    }

    IEnumerator AnimateOptions()
    {

        if (trueFlase)
        {
            for (int i = 0; i < num; i++)
            {
                options[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                options[i].DOAnchorPosY(optTargets[i], 0.2f).SetEase(Ease.OutQuad);
            }

            if (optionsBlocker)
            {
                optionsBlocker.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < options.Length; i++)
            {
                options[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                options[i].DOAnchorPosY(optTargets[i], 0.2f).SetEase(Ease.OutQuad);
            }

            if (optionsBlocker)
            {
                optionsBlocker.SetActive(false);
            }

        }
    
    }
    
    void Animate_Options()
    {
        StartCoroutine(AnimateOptions());
    }

    void OnAnimationComplete()
    {

    }

    private void OnDisable()
    {
        if(isLessons)
        {
            targetRectTransform.anchoredPosition = actualPosition;
        }
    }
}
