using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum NPCStage
{
    None,
    View,
    Dialog
}

public class NPCObject : MonoBehaviour
{
    [Header("Id")]
    public CharacterID id;

    [Header("Scan")]
    public SpriteRenderer scan_FrameRenderer;
    public SpriteRenderer scan_LightRenderer;
    public GameObject scan_LightPosObj_Start;
    public GameObject scan_LightPosObj_End;
    public GameObject scan_AvatarPosObj;
    public GameObject scan_DronePosObj;
    public PlayerDirection scan_AvatarDir;
    //3 sec scan 7 unit. 3/7 = 0.428
    //1 unit use how many sec
    public float scanSpeed = 0.428f;

    [Header("ViewTrigger")]
    public OnTriggerControl viewTriggerControl;

    [Header("Renderer")]
    public Renderer npcRenderer;

    [Header("Info")]
    public ConfigData_Character info;

    [Header("Curr")]
    public int currDialogLine;
    public bool isAtViewTrigger;
    public NPCStage currStage;

    CommonUtils commonUtils;
    InputManager inputManager;

    public void Setup(ConfigData_Character _info)
    {
        commonUtils = CommonUtils.instance;
        inputManager = InputManager.instance;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;

        info = _info;

        viewTriggerControl.onTriggerEnterCallback += ViewTrigger_OnEnter;
        viewTriggerControl.onTriggerExitCallback += ViewTrigger_OnExit;

        currDialogLine = 0;
        isAtViewTrigger = false;

        OutlineControl();
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (isAtViewTrigger && currStage == NPCStage.View)
        {
            SoundManager.instance.Play_Input(2);
            currStage = NPCStage.Dialog;
            if (currDialogLine == 0)
            {
                StartCoroutine(ScanAni());
                IEnumerator ScanAni()
                {
                    GameManager.instance.dialogActive = true;
                    ViewBoxManager.instance.HideViewBox();
                    DroneController.instance.canShowTalkHint = false;
                    DroneController.instance.HideTalkHint();
                    PlayerController.instance.transform.DOLocalMove(scan_AvatarPosObj.transform.position, 0.5f);
                    PlayerController.instance.SetDirection(scan_AvatarDir);
                    DroneController.instance.transform.DOLocalMove(scan_DronePosObj.transform.position, 0.5f);
                    yield return new WaitForSeconds(0.5f);
                    DroneController.instance.animator.SetTrigger("Scan");
                    SoundManager.instance.Play_SFX(10);
                    scan_FrameRenderer.DOFade(1f, 0.3f);
                    scan_LightRenderer.DOFade(1f, 0.3f);
                    //float dist = Vector3.Distance(scan_LightPosObj_Start.transform.position, scan_LightPosObj_End.transform.position);
                    //Debug.Log(dist);
                    //float time = dist * scanSpeed;
                    scan_LightRenderer.transform.DOLocalMove(scan_LightPosObj_End.transform.localPosition, 3f).From(scan_LightPosObj_Start.transform.localPosition).SetEase(Ease.Linear);
                    yield return new WaitForSeconds(3f);
                    SoundManager.instance.FadeOutStop_SFX(0.5f);
                    DroneController.instance.animator.SetTrigger("Idle");
                    scan_FrameRenderer.DOFade(0f, 0.3f);
                    scan_LightRenderer.DOFade(0f, 0.3f);
                    DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                    currDialogLine++;
                }
            }
        }
        else if (currStage == NPCStage.Dialog)
        {
            if (currDialogLine == info.DialogBoxes.Count)
            {
                SoundManager.instance.Play_Input(2);
                DialogBoxManager.instance.HideDialog();
                if (id == CharacterID.NPC_P11 || id == CharacterID.NPC_P12)
                {
                    DialogBoxManager.instance.HideZoomImg(0.5f);
                }
                currDialogLine = 0;
                info.IsFirstMeetDone = true;
                GameManager.instance.dialogActive = false;
                DroneController.instance.canShowTalkHint = true;
                currStage = NPCStage.None;
            }
            else
            {
                SoundManager.instance.Play_Input(2);
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                if (id == CharacterID.NPC_P11 || id == CharacterID.NPC_P12)
                {
                    DialogBoxManager.instance.ShowZoomImg(id, 0.5f);
                }
                currDialogLine++;
            }
        }
    }

    void OutlineControl()
    {
        StartCoroutine(OutlineAni());
    }

    IEnumerator OutlineAni()
    {
        npcRenderer.material.DOFloat(1, "_OutlineAlpha", 1f);
        yield return new WaitForSeconds(1.3f);
        npcRenderer.material.DOFloat(0.4f, "_OutlineAlpha", 1f);
        yield return new WaitForSeconds(1f);
        Invoke("OutlineControl", 0f);
    }

    private void ViewTrigger_OnEnter()
    {
        currStage = NPCStage.View;
        ViewBoxManager.instance.ShowViewBox();
        isAtViewTrigger = true;
        DroneController.instance.canShowTalkHint = false;
        DroneController.instance.HideTalkHint();
    }

    private void ViewTrigger_OnExit()
    {
        if (currStage != NPCStage.Dialog)
        {
            currStage = NPCStage.None;
            ViewBoxManager.instance.HideViewBox();
            isAtViewTrigger = false;
            DroneController.instance.canShowTalkHint = true;
            DroneController.instance.ShowTalkHint();
        }
    }

    private void OnDestroy()
    {
        viewTriggerControl.onTriggerEnterCallback -= ViewTrigger_OnEnter;
        viewTriggerControl.onTriggerExitCallback -= ViewTrigger_OnExit;
        inputManager.onValueChanged_ConfirmCallback -= InputManager_OnValueChanged_Confirm;
    }
}
