using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossObject : MonoBehaviour
{
    public delegate void OnFinishedConversation();
    public OnFinishedConversation onFinishedConversationCallback;

    [Header("Sprite")]
    public Sprite bossSprite;
    public Sprite collectionBookThumbnailSprite;

    [Header("Trigger")]
    public OnTriggerControl alertTriggerControl;
    public OnTriggerControl firstTriggerControl;

    [Header("Arrow")]
    public GameObject arrowObj_Green;
    public GameObject arrowObj_Grey;

    [Header("Info")]
    public ConfigData_DialogBox dialogBox_Alert;
    public ConfigData_Character info;

    [Header("Curr")]
    public int currDialogLine;
    public bool canShowAlert;
    public bool isAtAlertTrigger;
    public bool isAtFirstTrigger;

    public void Setup(ConfigData_DialogBox _dialogBox_Alert, ConfigData_Character _info, bool _canShowAlert, bool isFirstMeetDone)
    {
        dialogBox_Alert = _dialogBox_Alert;
        info = _info;
        canShowAlert = _canShowAlert;

        alertTriggerControl.onTriggerEnterCallback += AlertTrigger_OnEnter;
        alertTriggerControl.onTriggerExitCallback += AlertTrigger_OnExit;

        firstTriggerControl.onTriggerEnterCallback += FirstTrigger_OnEnter;
        firstTriggerControl.onTriggerExitCallback += FirstTrigger_OnExit;

        currDialogLine = 0;
        isAtAlertTrigger = false;
        isAtFirstTrigger = false;

        if (isFirstMeetDone)
        {
            arrowObj_Green.SetActive(false);
            arrowObj_Grey.SetActive(true);
        }
        else
        {
            arrowObj_Green.SetActive(true);
            arrowObj_Grey.SetActive(false);
        }
    }

    private void AlertTrigger_OnEnter()
    {
        isAtAlertTrigger = true;
        if (canShowAlert)
        {
            GameManager.instance.dialogActive = true;
            DialogBoxManager.instance.ShowDialog(dialogBox_Alert);
        }
    }

    private void AlertTrigger_OnExit()
    {
        isAtAlertTrigger = false;
    }

    //-------

    private void FirstTrigger_OnEnter()
    {
        isAtFirstTrigger = true;
        ViewBoxManager.instance.ShowViewBox();
    }

    private void FirstTrigger_OnExit()
    {
        isAtFirstTrigger = false;
        ViewBoxManager.instance.HideViewBox();
    }

    //-------

    public void UpdateRun()
    {
        if (Input.GetButtonDown("RPGConfirmPC") && isAtAlertTrigger)
        {
            DialogBoxManager.instance.HideDialog();
            GameManager.instance.dialogActive = false;
        }
        else if (Input.GetButtonDown("RPGConfirmPC") && isAtFirstTrigger)
        {
            ViewBoxManager.instance.HideViewBox();
            
            if (currDialogLine == 0)
            {
                ConversationModeManager.instance.Show1(info.Name_TC, info.DescriptionTag_TC, bossSprite);
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                currDialogLine++;
            }
            else if (currDialogLine == 1)
            {
                ConversationModeManager.instance.Show2();
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                currDialogLine++;
            }
            else if (currDialogLine == info.DialogBoxes.Count)
            {
                if (onFinishedConversationCallback != null)
                {
                    onFinishedConversationCallback.Invoke();
                }
                DialogBoxManager.instance.HideDialog();
                //ConversationModeManager.instance.Hide();
                currDialogLine = 0;
                GameManager.instance.dialogActive = false;
                arrowObj_Green.SetActive(false);
                arrowObj_Grey.SetActive(true);
            }
            else
            {
                ConversationModeManager.instance.Show3();
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                currDialogLine++;
            }
        }
    }
}
