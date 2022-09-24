using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationModeAvatarObject : MonoBehaviour
{
    public Image img;
    public List<Sprite> sprites_Idle;
    public List<Sprite> sprites_Walk_L;
    public List<Sprite> sprites_Walk_R;

    int currIndex_Walk_L = 0;
    int currIndex_Walk_R = 0;
    int currIndex_Idle = 0;
    float aniTime = 0.25f;

    public void ChangeAni_Walk_R()
    {
        CancelInvoke("LoopAni_Walk_L");
        currIndex_Walk_R = 0;
        img.sprite = sprites_Walk_R[currIndex_Walk_R];
        LoopAni_Walk_R();
    }

    void LoopAni_Walk_R()
    {
        currIndex_Walk_R++;

        if (currIndex_Walk_R == sprites_Walk_R.Count)
        {
            currIndex_Walk_R = 0;
        }

        img.sprite = sprites_Walk_R[currIndex_Walk_R];
        Invoke("LoopAni_Walk_R", aniTime);
    }

    //------

    public void ChangeAni_Idle()
    {
        CancelInvoke("LoopAni_Walk_R");
        LoopAni_Idle();
    }

    void LoopAni_Idle()
    {
        currIndex_Idle++;

        if (currIndex_Idle == sprites_Idle.Count)
        {
            currIndex_Idle = 0;
        }

        img.sprite = sprites_Idle[currIndex_Idle];
        Invoke("LoopAni_Idle", aniTime);
    }

    //-----

    public void ChangeAni_Walk_L()
    {
        CancelInvoke("LoopAni_Idle");
        LoopAni_Walk_L();
    }

    void LoopAni_Walk_L()
    {
        currIndex_Walk_L++;

        if (currIndex_Walk_L == sprites_Walk_L.Count)
        {
            currIndex_Walk_L = 0;
        }

        img.sprite = sprites_Walk_L[currIndex_Walk_L];
        Invoke("LoopAni_Walk_L", aniTime);
    }


}
