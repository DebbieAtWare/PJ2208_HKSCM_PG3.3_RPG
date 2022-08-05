using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01Manager : MonoBehaviour
{
    [Header("Boss")]
    public BossObject bossObj;

    CommonUtils commonUtils;

    void Start()
    {
        commonUtils = CommonUtils.instance;

        bossObj.onFinishedConversationCallback += OnFinishedConversation;
        bossObj.Setup(commonUtils.dialogBox_BossAlert, commonUtils.Boss01, false, commonUtils.isFirstMeetDone_Boss01);

        if (commonUtils.isFirstMeetDone_Boss01)
        {
            GameManager.instance.dialogActive = false;
        }
        else
        {
            //to stop user move input
            GameManager.instance.dialogActive = true;
            DOTween.To(() => PlayerController.instance.transform.position, x => PlayerController.instance.transform.position = x, new Vector3(0.5f, -1.18f, 0f), 2.5f).SetEase(Ease.Linear);
        }
    }

    private void OnFinishedConversation()
    {
        commonUtils.isFirstMeetDone_Boss01 = true;
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
