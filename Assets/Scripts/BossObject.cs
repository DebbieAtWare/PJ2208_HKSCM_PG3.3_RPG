using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossObject : MonoBehaviour
{
    [Header("BossAlert")]
    public OnTriggerControl alertTriggerControl;

    [Header("Info")]
    public ConfigData_DialogBox dialogBox_Alert;
    public ConfigData_Character info;

    [Header("Curr")]
    public bool isFirstMeetDone;
    public int currDialogLine;
    public bool isAtAlertTrigger;
    public bool isAtFirstTrigger;

    public void Setup(ConfigData_DialogBox _dialogBox_Alert, ConfigData_Character _info)
    {
        dialogBox_Alert = _dialogBox_Alert;
        info = _info;

        alertTriggerControl.onTriggerEnterCallback += AlertTrigger_OnEnter;
        alertTriggerControl.onTriggerExitCallback += AlertTrigger_OnExit;
    }

    private void AlertTrigger_OnEnter()
    {
        isAtAlertTrigger = true;
        GameManager.instance.dialogActive = true;
        DialogBoxManager.instance.ShowDialog(dialogBox_Alert.Text_TC);
    }

    private void AlertTrigger_OnExit()
    {
        isAtAlertTrigger = false;
    }

    public void UpdateRun()
    {
        if (Input.GetButtonDown("RPGConfirmPC") && isAtAlertTrigger)
        {
            DialogBoxManager.instance.HideDialog();
            GameManager.instance.dialogActive = false;
        }
    }
}
