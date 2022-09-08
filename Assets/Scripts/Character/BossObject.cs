using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum BossStage
{
    None,
    Alert,
    AutoPlay,
    View,
    ConversationMode
}

public class BossObject : MonoBehaviour
{
    public delegate void OnFinishedConversation();
    public OnFinishedConversation onFinishedConversationCallback;

    [Header("Sprite")]
    public Sprite bossSprite;
    public Sprite collectionBookThumbnailSprite;

    [Header("Renderer")]
    public Renderer bossRenderer;
    public Renderer caveRenderer;

    [Header("Trigger")]
    public OnTriggerControl alertTriggerControl;
    public OnTriggerControl viewTriggerControl;

    [Header("Arrow")]
    public List<GameObject> arrowObjs_Green = new List<GameObject>();
    public List<GameObject> arrowObjs_Grey = new List<GameObject>();
    public GameObject minimapDot_Red;
    public GameObject minimapDot_Grey;

    [Header("Info")]
    public ConfigData_DialogBox dialogBox_Alert;
    public ConfigData_Character info;

    [Header("Curr")]
    public int currDialogLine;
    public bool canShowAlert;
    public bool isAtAlertTrigger;
    public bool isAtViewTrigger;
    public BossStage currBossStage;

    InputManager inputManager;

    public void Setup(ConfigData_DialogBox _dialogBox_Alert, ConfigData_Character _info, bool _canShowAlert, bool isFirstMeetDone)
    {
        inputManager = InputManager.instance;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;

        dialogBox_Alert = _dialogBox_Alert;
        info = _info;
        canShowAlert = _canShowAlert;

        alertTriggerControl.onTriggerEnterCallback += AlertTrigger_OnEnter;
        alertTriggerControl.onTriggerExitCallback += AlertTrigger_OnExit;

        viewTriggerControl.onTriggerEnterCallback += ViewTrigger_OnEnter;
        viewTriggerControl.onTriggerExitCallback += ViewTrigger_OnExit;

        currDialogLine = 0;
        isAtAlertTrigger = false;
        isAtViewTrigger = false;
        currBossStage = BossStage.None;

        if (isFirstMeetDone)
        {
            for (int i = 0; i < arrowObjs_Green.Count; i++)
            {
                arrowObjs_Green[i].SetActive(false);
                arrowObjs_Grey[i].SetActive(true);
            }
            minimapDot_Red.SetActive(false);
            minimapDot_Grey.SetActive(true);
            OutlineHide();
        }
        else
        {
            for (int i = 0; i < arrowObjs_Green.Count; i++)
            {
                arrowObjs_Green[i].SetActive(true);
                arrowObjs_Grey[i].SetActive(false);
            }
            minimapDot_Red.SetActive(true);
            minimapDot_Grey.SetActive(false);
            OutlineControl();
        }
    }

    private void InputManager_OnValueChanged_Confirm()
    {
       if (currBossStage == BossStage.Alert)
       {
            SoundManager.instance.Play_Input(2);
            DialogBoxManager.instance.HideDialog();
            DroneController.instance.canShowTalkHint = true;
            Invoke("DroneShowTalkHint", 0.1f);
            GameManager.instance.dialogActive = false;
            currBossStage = BossStage.None;
       }
       else if (currBossStage == BossStage.View)
       {
            SoundManager.instance.Play_BGM(1);
            ViewBoxManager.instance.HideViewBox();
            if (currDialogLine == 0)
            {
                SoundManager.instance.Play_Input(2);
                MinimapManager.instance.Hide(0.5f);
                StatusBarManager.instance.Hide_Carbon(0.5f);
                StatusBarManager.instance.Hide_Permian(0.5f);
                ConversationModeManager.instance.Show1(info.Name_TC, info.DescriptionTag_TC, bossSprite);
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                currDialogLine++;
            }
            currBossStage = BossStage.ConversationMode;
        }
       else if (currBossStage == BossStage.ConversationMode)
       {
            if (currDialogLine == 1)
            {
                SoundManager.instance.Play_Input(2);
                ConversationModeManager.instance.Show2();
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                currDialogLine++;
            }
            else if (currDialogLine == info.DialogBoxes.Count)
            {
                SoundManager.instance.Play_Input(2);
                DialogBoxManager.instance.HideDialog();
                ConversationModeManager.instance.HideFade(0.5f);
                currDialogLine = 0;
                GameManager.instance.dialogActive = false;
                for (int i = 0; i < arrowObjs_Green.Count; i++)
                {
                    arrowObjs_Green[i].SetActive(false);
                    arrowObjs_Grey[i].SetActive(true);
                }
                minimapDot_Red.SetActive(false);
                minimapDot_Grey.SetActive(true);
                OutlineHide();
                Invoke("FinishedConversationControl", 0.5f);
            }
            else
            {
                SoundManager.instance.Play_Input(2);
                ConversationModeManager.instance.Show3();
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                currDialogLine++;
            }
        }
    }

    void DroneShowTalkHint()
    {
        DroneController.instance.ShowTalkHint();
    }

    //---------

    void OutlineControl()
    {
        StartCoroutine(OutlineAni());
    }
    void OutlineHide()
    {
        CancelInvoke("OutlineControl");
        bossRenderer.material.DOFloat(0f, "_OutlineAlpha", 1.5f);
        if (caveRenderer != null)
        {
            caveRenderer.material.DOFloat(0f, "_OutlineAlpha", 1.5f);
        }
    }

    IEnumerator OutlineAni()
    {
        bossRenderer.material.DOFloat(1, "_OutlineAlpha", 1f);
        if (caveRenderer != null)
        {
            caveRenderer.material.DOFloat(1, "_OutlineAlpha", 1f);
        }
        yield return new WaitForSeconds(1.3f);
        bossRenderer.material.DOFloat(0.4f, "_OutlineAlpha", 1f);
        if (caveRenderer != null)
        {
            caveRenderer.material.DOFloat(0.4f, "_OutlineAlpha", 1f);
        }
        yield return new WaitForSeconds(1f);
        Invoke("OutlineControl", 0f);
    }

    //---------

    private void AlertTrigger_OnEnter()
    {
        isAtAlertTrigger = true;
        if (canShowAlert)
        {
            GameManager.instance.dialogActive = true;
            DialogBoxManager.instance.ShowDialog(dialogBox_Alert);
            DroneController.instance.canShowTalkHint = false;
            DroneController.instance.HideTalkHint();
            currBossStage = BossStage.Alert;
        }
    }

    private void AlertTrigger_OnExit()
    {
        isAtAlertTrigger = false;
    }

    //-------

    private void ViewTrigger_OnEnter()
    {
        isAtViewTrigger = true;
        ViewBoxManager.instance.ShowViewBox();
        currBossStage = BossStage.View;
    }

    private void ViewTrigger_OnExit()
    {
        isAtViewTrigger = false;
        ViewBoxManager.instance.HideViewBox();
        currBossStage = BossStage.None;
    }

    //-------

    void FinishedConversationControl()
    {
        if (onFinishedConversationCallback != null)
        {
            onFinishedConversationCallback.Invoke();
        }
    }

    private void OnDestroy()
    {
        inputManager.onValueChanged_ConfirmCallback -= InputManager_OnValueChanged_Confirm;

        alertTriggerControl.onTriggerEnterCallback -= AlertTrigger_OnEnter;
        alertTriggerControl.onTriggerExitCallback -= AlertTrigger_OnExit;

        viewTriggerControl.onTriggerEnterCallback -= ViewTrigger_OnEnter;
        viewTriggerControl.onTriggerExitCallback -= ViewTrigger_OnExit;
    }
}
