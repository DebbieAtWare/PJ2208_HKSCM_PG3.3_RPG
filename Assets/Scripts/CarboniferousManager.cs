using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarboniferousManager : MonoBehaviour
{
    [Header("NPC")]
    public List<NPCObject> NPCObjs = new List<NPCObject>();

    [Header("Boss")]
    public BossObject bossObj;

    CommonUtils commonUtils;

    void Start()
    {
        commonUtils = CommonUtils.instance;
        commonUtils.onSetupDoneCallback += CommonUtils_OnSetupDone;

        if (commonUtils.isFirstMeetDone_Boss01 && !commonUtils.isSuccessCollectDone_Boss01)
        {
            GameManager.instance.dialogActive = true;
            CollectionBookManager.instance.ShowSuccessCollect(commonUtils.Boss01.Name_TC);
            Invoke("CloseSuccessCollect", 2f);
        }
    }

    void CloseSuccessCollect()
    {
        CollectionBookManager.instance.HideSuccessCollect();
        GameManager.instance.dialogActive = false;
        commonUtils.isSuccessCollectDone_Boss01 = true;
    }

    private void CommonUtils_OnSetupDone()
    {
        for (int i = 0; i < commonUtils.NPC_Carboniferous.Count; i++)
        {
            if (NPCObjs[i].id.ToString() == commonUtils.NPC_Carboniferous[i].Id)
            {
                NPCObjs[i].Setup(commonUtils.NPC_Carboniferous[i]);
            }
        }

        bossObj.Setup(commonUtils.dialogBox_BossAlert, commonUtils.Boss01, true, commonUtils.isFirstMeetDone_Boss01);
    }

    void Update()
    {
        for (int i = 0; i < NPCObjs.Count; i++)
        {
            NPCObjs[i].UpdateRun();
        }

        bossObj.UpdateRun();
    }

    private void OnDestory()
    {
        commonUtils.onSetupDoneCallback -= CommonUtils_OnSetupDone;
    }
}
