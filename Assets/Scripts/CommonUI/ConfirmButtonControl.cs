using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ConfirmButtonControl : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public RectTransform arrowRect;
    public RectTransform frontRect;
    public bool isAutoPlay = false;

    private void Start()
    {
        PlayAni();
        if (isAutoPlay)
        {
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }

    void PlayAni()
    {
        Sequence s1 = DOTween.Sequence();
        s1.Append(arrowRect.DOAnchorPos(new Vector2(0, -8), 1f).From(new Vector2(0, 0)));
        s1.AppendInterval(0.2f);
        s1.SetLoops(-1, LoopType.Yoyo);
        Sequence s2 = DOTween.Sequence();
        s2.Append(frontRect.DOAnchorPos(new Vector2(0, 0), 1f).From(new Vector2(0, 8)));
        s2.AppendInterval(0.2f);
        s2.SetLoops(-1, LoopType.Yoyo);
    }

    public void SetAlpha(float val, float aniTime)
    {
        canvasGroup.DOFade(val, aniTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SetAlpha(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetAlpha(0, 0);
        }
    }
}
