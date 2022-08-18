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
        bossObj.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex], false, commonUtils.bosses[currUtilsIndex].IsFirstMeetDone);

        if (commonUtils.bosses[currUtilsIndex].IsFirstMeetDone)
        {
            GameManager.instance.dialogActive = false;
        }
        else
        {
            //to stop user move input
            GameManager.instance.dialogActive = true;
            DOTween.To(() => PlayerController.instance.transform.position, x => PlayerController.instance.transform.position = x, new Vector3(0.6f, -1.69f, 0f), 0.5f).SetEase(Ease.Linear);
        }
    }

    private void OnFinishedConversation()
    {
        commonUtils.bosses[currUtilsIndex].IsFirstMeetDone = true;
        bossObj.canShowAlert = false;
    }

    void Update()
    {
        bossObj.UpdateRun();
    }

    private void OnDestroy()
    {
        bossObj.onFinishedConversationCallback -= OnFinishedConversation;
    }

}
