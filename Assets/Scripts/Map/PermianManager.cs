using System;
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
            bossObj2.onFinishedConversationCallback += OnFinishedConversation_Boss2;
            bossObj2.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex_Boss2], true, commonUtils.bosses[currUtilsIndex_Boss2].IsFirstMeetDone, true);
        }
        else
        {
            bossObj2.onFinishedConversationCallback += OnFinishedConversation_Boss2;
            bossObj2.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex_Boss2], false, commonUtils.bosses[currUtilsIndex_Boss2].IsFirstMeetDone, true);
        }


        if (!commonUtils.bosses[currUtilsIndex_Boss3].IsFirstMeetDone)
        {
            bossObj3.onFinishedConversationCallback += OnFinishedConversation_Boss3;
            bossObj3.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex_Boss3], true, commonUtils.bosses[currUtilsIndex_Boss3].IsFirstMeetDone, true);
        }
        else
        {
            bossObj3.onFinishedConversationCallback += OnFinishedConversation_Boss3;
            bossObj3.Setup(commonUtils.dialogBox_BossAlert, commonUtils.bosses[currUtilsIndex_Boss3], false, commonUtils.bosses[currUtilsIndex_Boss3].IsFirstMeetDone, true);
        }

        MinimapManager.instance.Show(0.5f);

        StatusBarManager.instance.Hide_Carbon(0f);
        StatusBarManager.instance.Show_Permian(0.5f);

        SoundManager.instance.Play_BGM(3);
    }

    private void OnFinishedConversation_Boss2()
    {
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            InputManager.instance.canInput_Confirm = false;
            commonUtils.bosses[currUtilsIndex_Boss2].IsFirstMeetDone = true;
            bossObj2.canShowAlert = false;
            if (!commonUtils.bosses[currUtilsIndex_Boss2].IsSuccessCollectDone)
            {
                SoundManager.instance.Play_SFX(9);
                GameManager.instance.dialogActive = true;
                yield return new WaitForSeconds(0.5f);
                CollectionBookManager.instance.Show_Success(commonUtils.successCollectText, commonUtils.bosses[currUtilsIndex_Boss2], 0.5f);
                Invoke("CloseSuccessCollect_Boss2", 5f);
            }
            else
            {
                SoundManager.instance.Play_BGM(3);
                MinimapManager.instance.Show(0.5f);
                StatusBarManager.instance.Show_Permian(0.5f);
                InputManager.instance.canInput_Confirm = true;
                GameManager.instance.dialogActive = false;
            }
        }
        
    }

    private void OnFinishedConversation_Boss3()
    {
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            InputManager.instance.canInput_Confirm = false;
            commonUtils.bosses[currUtilsIndex_Boss3].IsFirstMeetDone = true;
            bossObj3.canShowAlert = false;
            if (!commonUtils.bosses[currUtilsIndex_Boss3].IsSuccessCollectDone)
            {
                SoundManager.instance.Play_SFX(9);
                GameManager.instance.dialogActive = true;
                yield return new WaitForSeconds(0.5f);
                CollectionBookManager.instance.Show_Success(commonUtils.successCollectText, commonUtils.bosses[currUtilsIndex_Boss3], 0.5f);
                Invoke("CloseSuccessCollect_Boss3", 5f);
            }
            else
            {
                SoundManager.instance.Play_BGM(3);
                MinimapManager.instance.Show(0.5f);
                StatusBarManager.instance.Show_Permian(0.5f);
                InputManager.instance.canInput_Confirm = true;
                GameManager.instance.dialogActive = false;
            }
        }
    }

    void CloseSuccessCollect_Boss2()
    {
        SoundManager.instance.FadeOutStop_SFX(0.5f);
        SoundManager.instance.Play_BGM(3);
        CollectionBookManager.instance.Hide_Succuss(0.5f);
        MinimapManager.instance.Show(0.5f);
        StatusBarManager.instance.Show_Permian(0.5f);
        StatusBarManager.instance.BadgeAni_Permian1(0.5f);
        commonUtils.bosses[currUtilsIndex_Boss2].IsSuccessCollectDone = true;
        GameManager.instance.dialogActive = false;
        InputManager.instance.canInput_Confirm = true;
        DroneController.instance.canShowTalkHint = true;
    }

    void CloseSuccessCollect_Boss3()
    {
        SoundManager.instance.FadeOutStop_SFX(0.5f);
        SoundManager.instance.Play_BGM(3);
        CollectionBookManager.instance.Hide_Succuss(0.5f);
        MinimapManager.instance.Show(0.5f);
        StatusBarManager.instance.Show_Permian(0.5f);
        StatusBarManager.instance.BadgeAni_Permian2(0.5f);
        commonUtils.bosses[currUtilsIndex_Boss3].IsSuccessCollectDone = true;
        GameManager.instance.dialogActive = false;
        InputManager.instance.canInput_Confirm = true;
        DroneController.instance.canShowTalkHint = true;
    }

    private void OnDestroy()
    {
        bossObj2.onFinishedConversationCallback -= OnFinishedConversation_Boss2;
        bossObj3.onFinishedConversationCallback -= OnFinishedConversation_Boss3;
    }
}
