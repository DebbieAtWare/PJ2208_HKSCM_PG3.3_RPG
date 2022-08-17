using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarboniferousManager : MonoBehaviour
{
    public static CarboniferousManager instance;

    [Header("NPC")]
    public List<NPCObject> NPCObjs = new List<NPCObject>();

    [Header("Boss")]
    public BossObject bossObj;

    CommonUtils commonUtils;
    int currUtilsIndex_Boss;

    void Awake()
    {
        Debug.Log("CarboniferousManager Awake");
        if (instance != null)
        {
            Debug.Log("More than one instance of CarboniferousManager");
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
                currUtilsIndex_Boss = i;
                break;
            }
        }

        for (int i = 0; i < commonUtils.NPC_Carboniferous.Count; i++)
        {
            if (NPCObjs[i].id.ToString() == commonUtils.NPC_Carboniferous[i].Id)
            {
                NPCObjs[i].Setup(commonUtils.NPC_Carboniferous[i]);
            }
        }

        if (!commonUtils.bosses[currUtilsIndex_Boss].IsFirstMeetDone)
        {
            bossObj.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex_Boss], true, commonUtils.bosses[currUtilsIndex_Boss].IsFirstMeetDone);
        }
        else
        {
            bossObj.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex_Boss], false, commonUtils.bosses[currUtilsIndex_Boss].IsFirstMeetDone);

            if (!commonUtils.bosses[currUtilsIndex_Boss].IsSuccessCollectDone)
            {
                GameManager.instance.dialogActive = true;
                CollectionBookManager.instance.ShowSuccessCollect(commonUtils.bosses[currUtilsIndex_Boss].Name_TC);
                Invoke("CloseSuccessCollect", 2f);
            }
        }
    }

    void CloseSuccessCollect()
    {
        CollectionBookManager.instance.HideSuccessCollect();
        GameManager.instance.dialogActive = false;
        commonUtils.bosses[currUtilsIndex_Boss].IsSuccessCollectDone = true;
    }

    void Update()
    {
        for (int i = 0; i < NPCObjs.Count; i++)
        {
            NPCObjs[i].UpdateRun();
        }

        bossObj.UpdateRun();
    }
}
