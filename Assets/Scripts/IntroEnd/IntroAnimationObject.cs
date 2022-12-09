using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroAnimationObject : MonoBehaviour
{
    public Image img;
    public List<Sprite> sprites;

    public float fps_Normal;
    public float fps_Fast;
    public int loopFirstFrame;

    int currIndex;
    float aniTime;
    bool isFspNormal = true;

    public void Setup(float fps_Normal, float fps_Fast, int loopFirstFrame)
    {
        //this.fps_Normal = fps_Normal;
        //this.fps_Fast = fps_Fast;
        //this.loopFirstFrame = loopFirstFrame;
    }

    void ChangeFPS_Normal()
    {
        aniTime = 1f / fps_Normal;
        isFspNormal = true;
    }
    public void ChangeFPS_Fast()
    {
        if (currIndex < loopFirstFrame)
        {
            aniTime = 1f / fps_Fast;
            isFspNormal = false;
        }
    }
    public void JumpToLoop()
    {
        if (currIndex < loopFirstFrame)
        {
            CancelInvoke("Ani");
            currIndex = loopFirstFrame;
            img.sprite = sprites[currIndex];
            Ani();
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

    public void ResetAll()
    {
        CancelInvoke("Ani");
        currIndex = 0;
    }
}
