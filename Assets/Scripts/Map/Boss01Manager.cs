using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01Manager : MonoBehaviour
{
    public static Boss01Manager instance;

    [Header("Boss")]
    public BossObject bossObj;

    CommonUtils commonUtils;
    int currUtilsIndex;

    void Awake()
    {
        Debug.Log("Boss01Manager Awake");
        if (instance != null)
        {
            Debug.Log("More than one instance of Boss01Manager");
            return;
        }
        instance = this;
    }

    public void Setup()
    {
        commonUtils = CommonUtils.instance;
        for (int i = 0; i < commonUtils.bosses.Count; i++)
        {
            if (commonUtils.bosses[i].Id == CharacterID.M01.ToString())
            {
                currUtilsIndex = i;
                break;
            }
        }

        bossObj.onFinishedConversationCallback += OnFinishedConversation;
        bossObj.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex], false, commonUtils.bosses[currUtilsIndex].IsFirstMeetDone, false);

        //to stop user move input
        GameManager.instance.dialogActive = true;
        DOTween.To(() => PlayerController.instance.transform.position, x => PlayerController.instance.transform.position = x, new Vector3(0.6f, -1.69f, 0f), 0.8f).SetEase(Ease.Linear).SetDelay(0.8f);

        MinimapManager.instance.Hide(0.5f);
    }

    private void OnFinishedConversation()
    {
        InputManager.instance.canInput_Confirm = false;
        commonUtils.bosses[currUtilsIndex].IsFirstMeetDone = true;
        bossObj.canShowAlert = false;
        if (!commonUtils.bosses[currUtilsIndex].IsSuccessCollectDone)
        {
            SoundManager.instance.Play_SFX(9);
            GameManager.instance.dialogActive = true;
            CollectionBookManager.instance.Show_Success(commonUtils.successCollectText, commonUtils.bosses[currUtilsIndex], 0.5f);
            Invoke("CloseSuccessCollect", 5f);
        }
        else
        {
            GameManager.instance.dialogActive = true;
            Invoke("TeleportControl", 0.5f);
        }
    }

    void CloseSuccessCollect()
    {
        SoundManager.instance.FadeOutStop_SFX(0.5f);
        CollectionBookManager.instance.Hide_Succuss(0.5f);
        StatusBarManager.instance.Show_Carbon(0.5f);
        StatusBarManager.instance.BadgeAni_Carbon(0.5f);
        commonUtils.bosses[currUtilsIndex].IsSuccessCollectDone = true;
        Invoke("TeleportControl", 3.5f);
    }

    void TeleportControl()
    {
        TransitionManager.instance.ChangeToOutsideTreeCave();
        //inside transition manager will lock and release input
    }

    private void OnDestroy()
    {
        bossObj.onFinishedConversationCallback -= OnFinishedConversation;
    }

}
