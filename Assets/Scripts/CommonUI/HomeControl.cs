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
    public Image img_Bkg;
    public Image img_Drone;
    public Image img_Title_TC;
    public Image img_Title_SC;
    public List<Sprite> sprites_Bkg;
    public List<Sprite> sprites_Drone;

    [Header("Text")]
    public GameObject text_TC;
    public GameObject text_SC;
    public GameObject text_EN;
    int currTextIndex = 0;
    float textInvokeTime = 2;

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
        text_TC.SetActive(true);
        text_SC.SetActive(false);
        text_EN.SetActive(false);
        BkgLoopAni();
        DroneLoopAni();
        TextAni();
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
        CancelInvoke("DroneLoopAni");
        CancelInvoke("TextAni");
        currIndex_Bkg = 0;
        currIndex_Drone = 0;
        currTextIndex = 0;
        SetAlpha(0, 0);
    }
}
