using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HomeControl : MonoBehaviour
{
    [Header("Main")]
    public CanvasGroup canvasGrp;

    [Header("Bkg")]
    public Image img_Bkg;
    public Image img_Drone;
    public Image img_Title_TC;
    public Image img_Title_SC;
    public List<Sprite> sprites_Bkg;
    public List<Sprite> sprites_Drone;
    int currTitleIndex = 0;
    float titleInvokeTime = 8f;

    [Header("Text")]
    public TextMeshProUGUI text_TC;
    public TextMeshProUGUI text_SC;
    public TextMeshProUGUI text_EN;
    int currTextIndex = 0;
    float textInvokeTime = 2f;

    int currIndex_Bkg = 0;
    int currIndex_Drone = 0;
    float fps = 12;
    float aniTime;

    public void SetAlpha(float val, float aniTime)
    {
        canvasGrp.DOFade(val, aniTime);
    }

    public void PlayBkg()
    {
        aniTime = 1f / fps;
        currIndex_Bkg = 0;
        currIndex_Drone = 0;
        img_Bkg.sprite = sprites_Bkg[currIndex_Bkg];
        img_Drone.sprite = sprites_Drone[currIndex_Drone];
        text_TC.DOFade(1, 0f);
        text_SC.DOFade(0, 0f);
        text_EN.DOFade(0, 0f);
        BkgLoopAni();
        DroneLoopAni();
        TextAni();
        TitleAni();
    }

    void BkgLoopAni()
    {
        currIndex_Bkg++;

        if (currIndex_Bkg == sprites_Bkg.Count)
        {
            currIndex_Bkg = 0;
        }

        img_Bkg.sprite = sprites_Bkg[currIndex_Bkg];
        Invoke("BkgLoopAni", aniTime);
    }
    void DroneLoopAni()
    {
        currIndex_Drone++;

        if (currIndex_Drone == sprites_Drone.Count)
        {
            currIndex_Drone = 0;
        }

        img_Drone.sprite = sprites_Drone[currIndex_Drone];
        Invoke("DroneLoopAni", aniTime);
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
            text_TC.DOFade(1, 0.2f).SetDelay(0.4f);
            text_SC.DOFade(0, 0.2f);
            text_EN.DOFade(0, 0.2f);
        }
        else if (currTextIndex == 1)
        {
            text_TC.DOFade(0, 0.2f);
            text_SC.DOFade(1, 0.2f).SetDelay(0.4f);
            text_EN.DOFade(0, 0.2f);
        }
        else if (currTextIndex == 2)
        {
            text_TC.DOFade(0, 0.2f);
            text_SC.DOFade(0, 0.2f);
            text_EN.DOFade(1, 0.2f).SetDelay(0.4f);
        }
        Invoke("TextAni", textInvokeTime);
    }

    void TitleAni()
    {
        currTitleIndex++;
        if (currTitleIndex >= 2)
        {
            currTitleIndex = 0;
        }

        if (currTitleIndex == 0)
        {
            img_Title_TC.DOFade(1, 0.3f).SetDelay(0.4f);
            img_Title_SC.DOFade(0, 0.3f);
        }
        else if (currTitleIndex == 1)
        {
            img_Title_TC.DOFade(0, 0.3f);
            img_Title_SC.DOFade(1, 0.3f).SetDelay(0.4f);
        }
        Invoke("TitleAni", titleInvokeTime);
    }

    public void ResetAll()
    {
        CancelInvoke("BkgLoopAni");
        CancelInvoke("DroneLoopAni");
        CancelInvoke("TextAni");
        CancelInvoke("TitleAni");
        currIndex_Bkg = 0;
        currIndex_Drone = 0;
        currTextIndex = 0;
        SetAlpha(0, 0);
    }
}
