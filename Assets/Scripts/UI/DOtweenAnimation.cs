using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DOtweenAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private float slideDuration = 1f;
    public Text text;
    private Vector2 originalPosition;

    private void OnEnable()
    {
        // Store the original position of the panel
        originalPosition = panelRectTransform.anchoredPosition;

        // Slide the panel upward by its height
        panelRectTransform.DOLocalMoveY(500, 1).OnComplete((ResetPanel));

    }

    public void ResetPanel()
    {
        // Enable the panel game object
        gameObject.SetActive(false);
        panelRectTransform.anchoredPosition = originalPosition;

        // Reset the panel position to its original position

    }
}