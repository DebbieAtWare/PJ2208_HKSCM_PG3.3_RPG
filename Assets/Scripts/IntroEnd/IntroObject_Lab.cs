using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IntroObject_Lab : MonoBehaviour
{
    public delegate void OnTransitionStartDone();
    public OnTransitionStartDone onTransitionStartDoneCallback;

    public Image img;
    public List<Sprite> sprites;

    public float fps_Normal;
    public float fps_Fast;
    public int loopFirstFrame;

    int currIndex;
    float aniTime;
    bool isFirstPlay = true;

    public void Setup()
    {
        AlphaAni(0, 0);
    }

    void ChangeFPS_Normal()
    {
        aniTime = 1f / fps_Normal;
    }

    public void AlphaAni(float val, float aniTime)
    {
        img.DOFade(val, aniTime);
    }

    public void Play()
    {
        currIndex = 0;
        ChangeFPS_Normal();
        img.sprite = sprites[currIndex];
        Ani();
    }

    void Ani()
    {
        currIndex++;

        if (isFirstPlay && currIndex == loopFirstFrame)
        {
            isFirstPlay = false;
            if (onTransitionStartDoneCallback != null)
            {
                onTransitionStartDoneCallback.Invoke();
            }
        }

        if (currIndex == sprites.Count)
        {
            currIndex = loopFirstFrame;
        }

        img.sprite = sprites[currIndex];
        Invoke("Ani", aniTime);
    }

    public void ResetAll()
    {
        CancelInvoke("Ani");
        currIndex = 0;
    }
}
