using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class XaxisAnimation : MonoBehaviour
{
    public RectTransform targetRectTransform;
    public float targetX = 0f; // Target Y position
    public float duration = 2f; // Animation duration
    public Vector2 actualPosition;

    public bool isSelectBtn, isWelcome;


    void OnEnable()
    {
        targetRectTransform = GetComponent<RectTransform>();

        // Call the AnimateYAxis function when the script starts
        if (isWelcome)
        {
            Invoke("AnimateXAxis", 1f);
        }
        else
        {
            AnimateXAxis();
        }
    }

    private void Start()
    {
        
    }

    void AnimateXAxis()
    {
        targetRectTransform.DOAnchorPosX(targetX, duration)
        .SetEase(Ease.Linear);
    }


    private void OnDisable()
    {
        //   RefrenceManager.instance.uIManager.DisbaleBgPanel();
        targetRectTransform.anchoredPosition = actualPosition;

    }
}
