using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager instance;

    public CanvasGroup canvasGrp;

    void Start()
    {
        instance = this;
    }

    public void Show(float aniTime)
    {
        canvasGrp.gameObject.SetActive(true);
        canvasGrp.DOFade(1, aniTime);
    }

    public void Hide(float aniTime)
    {
        canvasGrp.DOFade(0, aniTime).OnComplete(() => canvasGrp.gameObject.SetActive(false));
    }
}
