using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationModeBossObject : MonoBehaviour
{
    [Header("Main")]
    public RectTransform rectTrans;
    public CanvasGroup canvasGrp;

    [Header("ID")]
    public CharacterID id;

    [Header("Ani")]
    public Image img;
    public List<Sprite> sprites_Idle = new List<Sprite>();
    public List<Sprite> sprites_Talk = new List<Sprite>();

    [Header("For M03 only. Can be NULL")]
    public RectTransform teethRect;
    public List<Vector2> teethPos_Idle = new List<Vector2>();
    public List<Vector2> teethPos_Talk = new List<Vector2>();

    int currIndex_Idle = 0;
    float aniTime_Idle = 0.25f;

    int currIndex_Talk = 0;
    float aniTime_Talk = 0.25f;

    enum BossStage
    {
        None,
        Idle,
        Talk
    }
    BossStage currStage = BossStage.None;

    public void ChangeAni_Idle()
    {
        if (currStage != BossStage.Idle)
        {
            CancelInvoke("LoopAni_Talk");
            LoopAni_Idle();
            currStage = BossStage.Idle;
        }
    }

    public void ChangeAni_Talk()
    {
        if (currStage != BossStage.Talk)
        {
            CancelInvoke("LoopAni_Idle");
            LoopAni_Talk();
            currStage = BossStage.Talk;
        }
    }

    void LoopAni_Idle()
    {
        currIndex_Idle++;

        if (currIndex_Idle == sprites_Idle.Count)
        {
            currIndex_Idle = 0;
        }

        img.sprite = sprites_Idle[currIndex_Idle];
        if (teethRect != null)
        {
            teethRect.anchoredPosition = teethPos_Idle[currIndex_Idle];
        }
        Invoke("LoopAni_Idle", aniTime_Idle);
    }

    void LoopAni_Talk()
    {
        currIndex_Talk++;

        if (currIndex_Talk == sprites_Talk.Count)
        {
            currIndex_Talk = 0;
        }

        img.sprite = sprites_Talk[currIndex_Talk];
        if (teethRect != null)
        {
            teethRect.anchoredPosition = teethPos_Talk[currIndex_Talk];
        }
        Invoke("LoopAni_Talk", aniTime_Talk);
    }

}
