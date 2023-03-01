using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ConfirmButtonControl : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public RectTransform arrowRect;
    public RectTransform frontRect;
    public List<Image> imgs = new List<Image>();
    public bool isAutoPlay = false;
    public bool isBlack = false;

    private void OnEnable()
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
        if (isBlack)
        {
            for (int i = 0; i < imgs.Count; i++)
            {
                Color c;
                ColorUtility.TryParseHtmlString("#141414", out c);
                imgs[i].color = c;
            }
        }
    }

    void PlayAni()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            arrowRect.DOAnchorPos(new Vector2(0, -4), 0.6f);
            yield return new WaitForSeconds(0.2f);
            frontRect.DOAnchorPos(new Vector2(0, 0), 0.6f);
            yield return new WaitForSeconds(0.4f);
            arrowRect.DOAnchorPos(new Vector2(0, 4), 0.6f);
            yield return new WaitForSeconds(0.2f);
            frontRect.DOAnchorPos(new Vector2(0, 8), 0.6f);
            yield return new WaitForSeconds(0.6f);
            PlayAni();
        }
    }

    public void SetAlpha(float val, float aniTime)
    {
        canvasGroup.DOFade(val, aniTime);
    }
}
