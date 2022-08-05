using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarboniferousManager : MonoBehaviour
{
    [Header("NPC")]
    public List<NPCObject> NPCObjs = new List<NPCObject>();

    CommonUtils commonUtils;

    void Start()
    {
        commonUtils = CommonUtils.instance;
        commonUtils.onSetupDoneCallback += CommonUtils_OnSetupDone;
    }

    private void CommonUtils_OnSetupDone()
    {
        for (int i = 0; i < CommonUtils.instance.NPC_Carboniferous.Count; i++)
        {
            if (NPCObjs[i].id.ToString() == CommonUtils.instance.NPC_Carboniferous[i].Id)
            {
                NPCObjs[i].Setup(CommonUtils.instance.NPC_Carboniferous[i].Name_TC, CommonUtils.instance.NPC_Carboniferous[i].Name_SC, CommonUtils.instance.NPC_Carboniferous[i].Name_EN,
                    CommonUtils.instance.NPC_Carboniferous[i].IsCollectable, CommonUtils.instance.NPC_Carboniferous[i].DialogBoxes);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < NPCObjs.Count; i++)
        {
            NPCObjs[i].UpdateRun();
        }

    }

    private void OnDestory()
    {
        commonUtils.onSetupDoneCallback -= CommonUtils_OnSetupDone;
    }
}
