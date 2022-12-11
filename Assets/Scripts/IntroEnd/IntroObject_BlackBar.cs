using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroObject_BlackBar : MonoBehaviour
{
    public delegate void OnVideoEnd();
    public OnVideoEnd onVideoEndCallback;

    public Image img;
    public List<Sprite> sprites;

    public float fps_Normal;

    int currIndex;
    float aniTime;

    public void Setup()
    {
        AlphaAni(0, 0);
    }

    public void AlphaAni(float val, float aniTime)
    {
        img.DOFade(val, aniTime);
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

        if (currIndex == sprites.Count)
        {
            CancelInvoke("Ani");
            if (onVideoEndCallback != null)
            {
                onVideoEndCallback.Invoke();
            }
        }
        else
        {
            img.sprite = sprites[currIndex];
            Invoke("Ani", aniTime);
        }
    }

    public void ResetAll()
    {
        CancelInvoke("Ani");
        currIndex = 0;
    }
}
