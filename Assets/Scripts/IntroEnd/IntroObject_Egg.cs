using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IntroObject_Egg : MonoBehaviour
{
    public CanvasGroup canvasGrp;
    public Image img;
    public List<Sprite> sprites;
    public TextMeshProUGUI text_TC_L;
    public TextMeshProUGUI text_SC_L;
    public TextMeshProUGUI text_EN_L;
    public TextMeshProUGUI text_TC_R;
    public TextMeshProUGUI text_SC_R;
    public TextMeshProUGUI text_EN_R;

    public float fps_Normal;

    int textTriggerFrameL = 20;
    int textTriggerFrameR = 4;
    int currIndex;
    float aniTime;

    CommonUtils commonUtils;

    public void Setup()
    {
        commonUtils = CommonUtils.instance;
        text_TC_L.alpha = 0;
        text_SC_L.alpha = 0;
        text_EN_L.alpha = 0;
        text_TC_R.alpha = 0;
        text_SC_R.alpha = 0;
        text_EN_R.alpha = 0;
        AlphaAni(0, 0);
        
    }

    public void AlphaAni(float val, float aniTime)
    {
        canvasGrp.DOFade(val, aniTime);
        img.DOFade(val, aniTime);
        if (val == 0)
        {
            text_TC_L.DOFade(val, aniTime);
            text_SC_L.DOFade(val, aniTime);
            text_EN_L.DOFade(val, aniTime);
            text_TC_R.DOFade(val, aniTime);
            text_SC_R.DOFade(val, aniTime);
            text_EN_R.DOFade(val, aniTime);
        }
    }

    public void Play()
    {
        currIndex = 0;
        aniTime = 1f / fps_Normal;
        img.sprite = sprites[currIndex];
        Ani();
    }

    void Ani()
    {
        currIndex++;

        if (commonUtils.currLang == Language.TC)
        {
            if (currIndex == textTriggerFrameL && text_TC_L.alpha == 0)
            {
                text_TC_L.alpha = 1;
            }
            if (currIndex == textTriggerFrameR && text_TC_R.alpha == 0)
            {
                text_TC_R.alpha = 1;
            }
        }
        else if (commonUtils.currLang == Language.SC)
        {
            if (currIndex == textTriggerFrameL && text_SC_L.alpha == 0)
            {
                text_SC_L.alpha = 1;
            }
            if (currIndex == textTriggerFrameR && text_SC_R.alpha == 0)
            {
                text_SC_R.alpha = 1;
            }
        }
        else if (commonUtils.currLang == Language.EN)
        {
            if (currIndex == textTriggerFrameL && text_EN_L.alpha == 0)
            {
                text_EN_L.alpha = 1;
            }
            if (currIndex == textTriggerFrameR && text_EN_R.alpha == 0)
            {
                text_EN_R.alpha = 1;
            }
        }

        if (currIndex == sprites.Count)
        {
            currIndex = 0;
        }

        img.sprite = sprites[currIndex];
        Invoke("Ani", aniTime);
    }

    public void DirectShowAllText()
    {
        if (commonUtils.currLang == Language.TC)
        {
            text_TC_L.alpha = 1;
            text_TC_R.alpha = 1;
        }
        else if (commonUtils.currLang == Language.SC)
        {
            text_SC_L.alpha = 1;
            text_SC_R.alpha = 1;
        }
        else if (commonUtils.currLang == Language.EN)
        {
            text_EN_L.alpha = 1;
            text_EN_R.alpha = 1;
        }
    }

    public void ResetAll()
    {
        CancelInvoke("Ani");
        currIndex = 0;
    }
}
