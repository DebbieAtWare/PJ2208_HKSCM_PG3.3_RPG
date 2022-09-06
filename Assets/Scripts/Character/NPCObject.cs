using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPCObject : MonoBehaviour
{
    [Header("Id")]
    public CharacterID id;

    [Header("ViewTrigger")]
    public OnTriggerControl viewTriggerControl;

    [Header("Renderer")]
    public Renderer npcRenderer;

    [Header("Info")]
    public ConfigData_Character info;

    [Header("Curr")]
    public int currDialogLine;
    public bool isAtViewTrigger;

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
        if (isAtViewTrigger)
        {
            if (currDialogLine == 0)
            {
                GameManager.instance.dialogActive = true;
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                ViewBoxManager.instance.HideViewBox();
                currDialogLine++;
            }
            else if (currDialogLine == info.DialogBoxes.Count)
            {
                DialogBoxManager.instance.HideDialog();
                if (id == CharacterID.NPC_P11 || id == CharacterID.NPC_P12)
                {
                    DialogBoxManager.instance.HideZoomImg(0.5f);
                }
                currDialogLine = 0;
                info.IsFirstMeetDone = true;
                GameManager.instance.dialogActive = false;
            }
            else
            {
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
        ViewBoxManager.instance.ShowViewBox();
        isAtViewTrigger = true;
        DroneController.instance.canShowTalkHint = false;
        DroneController.instance.HideTalkHint();
    }

    private void ViewTrigger_OnExit()
    {
        ViewBoxManager.instance.HideViewBox();
        isAtViewTrigger = false;
        DroneController.instance.canShowTalkHint = true;
        DroneController.instance.ShowTalkHint();
    }

    private void OnDestroy()
    {
        viewTriggerControl.onTriggerEnterCallback -= ViewTrigger_OnEnter;
        viewTriggerControl.onTriggerExitCallback -= ViewTrigger_OnExit;
        inputManager.onValueChanged_ConfirmCallback -= InputManager_OnValueChanged_Confirm;
    }
}
