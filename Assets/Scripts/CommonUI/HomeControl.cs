using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HomeControl : MonoBehaviour
{
    [Header("Main")]
    public CanvasGroup canvasGrp;

    [Header("Bkg")]
    public Image img;
    public List<Sprite> sprites;

    int currIndex = 0;
    float fps = 12;
    float aniTime;

    public void SetAlpha(float val, float aniTime)
    {
        canvasGrp.DOFade(val, aniTime);
    }

    public void PlayBkg()
    {
        currIndex = 0;
        aniTime = 1f / fps;
        img.sprite = sprites[currIndex];
        BkgLoopAni();
    }

    void BkgLoopAni()
    {
        currIndex++;

        if (currIndex == sprites.Count)
        {
            currIndex = 0;
        }

        img.sprite = sprites[currIndex];
        Invoke("BkgLoopAni", aniTime);
    }

    public void ResetAll()
    {
        CancelInvoke("BkgLoopAni");
        currIndex = 0;
        SetAlpha(0, 0);
    }
}
