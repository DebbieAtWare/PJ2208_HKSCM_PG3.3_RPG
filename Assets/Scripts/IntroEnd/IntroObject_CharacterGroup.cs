using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroObject_CharacterGroup : MonoBehaviour
{
    public Image img;
    public List<Sprite> sprites;
    public TextMeshProUGUI text_TC;
    public TextMeshProUGUI text_SC;
    public TextMeshProUGUI text_EN;

    public float fps_Normal;
    public float fps_Fast;
    public int loopFirstFrame;

    int currIndex;
    float aniTime;
    bool isFspNormal = true;

    public void Setup()
    {
        AlphaAni(0, 0);
        text_TC.gameObject.SetActive(false);
        text_SC.gameObject.SetActive(false);
        text_EN.gameObject.SetActive(false);
    }

    public void AlphaAni(float val, float aniTime)
    {
        img.DOFade(val, aniTime);
    }

    void ChangeFPS_Normal()
    {
        aniTime = 1f / fps_Normal;
    }
    public void ChangeFPS_Fast()
    {
        if (currIndex < loopFirstFrame)
        {
            aniTime = 1f / fps_Fast;
            isFspNormal = false;
        }
    }

    public void Play()
    {
        currIndex = 0;
        ChangeFPS_Normal();
        img.sprite = sprites[currIndex];
        if (CommonUtils.instance.currLang == Language.TC)
        {
            text_TC.gameObject.SetActive(true);
            text_SC.gameObject.SetActive(false);
            text_EN.gameObject.SetActive(false);
        }
        else if (CommonUtils.instance.currLang == Language.SC)
        {
            text_TC.gameObject.SetActive(false);
            text_SC.gameObject.SetActive(true);
            text_EN.gameObject.SetActive(false);
        }
        else if (CommonUtils.instance.currLang == Language.EN)
        {
            text_TC.gameObject.SetActive(false);
            text_SC.gameObject.SetActive(false);
            text_EN.gameObject.SetActive(true);
        }
        Ani();
    }

    void Ani()
    {
        currIndex++;

        //when speed up and at loop first frame
        //change back to normal speed
        if (currIndex > loopFirstFrame && !isFspNormal)
        {
            ChangeFPS_Normal();
        }

        if (currIndex == sprites.Count)
        {
            currIndex = loopFirstFrame;
        }

        img.sprite = sprites[currIndex];
        Invoke("Ani", aniTime);
    }

    public bool IsInLoopArea()
    {
        return currIndex >= loopFirstFrame;
    }

    public void ResetAll()
    {
        CancelInvoke("Ani");
        currIndex = 0;
        ChangeFPS_Normal();
        isFspNormal = true;
    }
}
