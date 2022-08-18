using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermianManager : MonoBehaviour
{
    public static PermianManager instance;

    [Header("NPC")]
    public List<NPCObject> NPCObjs = new List<NPCObject>();

    [Header("Boss")]
    public BossObject bossObj2;
    public BossObject bossObj3;

    CommonUtils commonUtils;
    int currUtilsIndex_Boss2;
    int currUtilsIndex_Boss3;

    void Awake()
    {
        Debug.Log("PermianManager Awake");
        if (instance != null)
        {
            Debug.Log("More than one instance of PermianManager");
            return;
        }
        instance = this;
    }

    public void Setup()
    {
        commonUtils = CommonUtils.instance;

        for (int i = 0; i < commonUtils.bosses.Count; i++)
        {
            if (commonUtils.bosses[i].Id == CharacterID.M02.ToString())
            {
                currUtilsIndex_Boss2 = i;
            }
            else if (commonUtils.bosses[i].Id == CharacterID.M03.ToString())
            {
                currUtilsIndex_Boss3 = i;
            }
        }

        for (int i = 0; i < commonUtils.NPC_Permian.Count; i++)
        {
            if (NPCObjs[i].id.ToString() == commonUtils.NPC_Permian[i].Id)
            {
                NPCObjs[i].Setup(commonUtils.NPC_Permian[i]);
            }
        }

        if (!commonUtils.bosses[currUtilsIndex_Boss2].IsFirstMeetDone)
        {
            bossObj2.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex_Boss2], true, commonUtils.bosses[currUtilsIndex_Boss2].IsFirstMeetDone);
        }
        else
        {
            bossObj2.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex_Boss2], false, commonUtils.bosses[currUtilsIndex_Boss2].IsFirstMeetDone);
        }


        if (!commonUtils.bosses[currUtilsIndex_Boss3].IsFirstMeetDone)
        {
            bossObj3.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex_Boss3], true, commonUtils.bosses[currUtilsIndex_Boss3].IsFirstMeetDone);
        }
        else
        {
            bossObj3.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex_Boss3], false, commonUtils.bosses[currUtilsIndex_Boss3].IsFirstMeetDone);
        }
    }

    void Update()
    {
        for (int i = 0; i < NPCObjs.Count; i++)
        {
            NPCObjs[i].UpdateRun();
        }

        bossObj2.UpdateRun();
        bossObj3.UpdateRun();
    }
}
