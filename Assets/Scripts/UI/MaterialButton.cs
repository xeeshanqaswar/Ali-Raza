using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ripplePrefab;
    public float rippleScale = 1f;
    public float rippleDuration = 0.5f;
    public float offsetX = 10f; //
    public bool login;
    public bool disable;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        
    }

    public float duration;
    void OnClick()
    {
        RectTransform rt = Instantiate(ripplePrefab, transform).GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;

        // Set the initial position to the center of the button
        rt.anchoredPosition = Vector2.zero;

        // Use DOTween to animate the ripple effect
        rt.DOScale(Vector3.one * 2.5f, 0.4f).SetEase(Ease.Linear).OnComplete(() => Destroy(rt.gameObject));
        Invoke("InvokeOtherClickEvents", 0.4f);
    }

    void InvokeOtherClickEvents()
    {
        if(login)
            Constants.Disable?.Invoke();
        if (disable)
        {
            Constants.NewDisable?.Invoke();
        }
  
    }


}


