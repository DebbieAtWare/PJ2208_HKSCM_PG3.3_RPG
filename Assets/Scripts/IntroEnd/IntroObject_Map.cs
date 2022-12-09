using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroObject_Map : MonoBehaviour
{
    public delegate void OnTransitionStartDone();
    public OnTransitionStartDone onTransitionStartDoneCallback;

    public Image img;
    public List<Sprite> sprites;

    public float fps_Normal;
    public float fps_Fast;
    public int transitionEndFrame;
    public int loopFirstFrame;

    int currIndex;
    float aniTime;
    bool isFspNormal = true;

    public void Setup()
    {
        AlphaAni(0, 0);
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
        Ani();
    }


    void Ani()
    {
        currIndex++;

        if (currIndex == transitionEndFrame)
        {
            if (onTransitionStartDoneCallback != null)
            {
                onTransitionStartDoneCallback.Invoke();
            }
        }

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
