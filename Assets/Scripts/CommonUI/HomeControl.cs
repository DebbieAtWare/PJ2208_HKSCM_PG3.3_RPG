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

    [Header("Text")]
    public GameObject text_TC;
    public GameObject text_SC;
    public GameObject text_EN;
    int currTextIndex = 0;
    float textInvokeTime = 5;

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
        text_TC.SetActive(true);
        text_SC.SetActive(false);
        text_EN.SetActive(false);
        BkgLoopAni();
        //TextAni();
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

    void TextAni()
    {
        currTextIndex++;
        if (currTextIndex >= 3)
        {
            currTextIndex = 0;
        }

        if (currTextIndex == 0)
        {
            text_TC.SetActive(true);
            text_SC.SetActive(false);
            text_EN.SetActive(false);
        }
        else if (currTextIndex == 1)
        {
            text_TC.SetActive(false);
            text_SC.SetActive(true);
            text_EN.SetActive(false);
        }
        else if (currTextIndex == 2)
        {
            text_TC.SetActive(false);
            text_SC.SetActive(false);
            text_EN.SetActive(true);
        }
        Invoke("TextAni", textInvokeTime);
    }

    public void ResetAll()
    {
        CancelInvoke("BkgLoopAni");
        CancelInvoke("TextAni");
        currIndex = 0;
        currTextIndex = 0;
        SetAlpha(0, 0);
    }
}
