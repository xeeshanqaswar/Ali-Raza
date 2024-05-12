using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class FadeinFadeout : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public float fadeInDuration = 2f;

    public void fadeinout(float duration)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, duration);
    }

    public void VideoFadeOut(float duration)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0f, duration).OnComplete(() => FadeComplete());
    }

    void FadeComplete()
    {
        RefrenceManager.instance.gameManager.SwitchState(GameManager.GameState.Model);
        //RefrenceManager.instance.gameManager.ModelStateHandling();
    }
}
