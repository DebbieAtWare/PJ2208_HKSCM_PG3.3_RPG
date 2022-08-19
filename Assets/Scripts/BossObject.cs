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

    [Header("Renderer")]
    public Renderer bossRenderer;
    public Renderer caveRenderer;

    [Header("Trigger")]
    public OnTriggerControl alertTriggerControl;
    public OnTriggerControl firstTriggerControl;

    [Header("Arrow")]
    public List<GameObject> arrowObjs_Green = new List<GameObject>();
    public List<GameObject> arrowObjs_Grey = new List<GameObject>();

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
            for (int i = 0; i < arrowObjs_Green.Count; i++)
            {
                arrowObjs_Green[i].SetActive(false);
                arrowObjs_Grey[i].SetActive(true);
            }
            OutlineHide();
        }
        else
        {
            for (int i = 0; i < arrowObjs_Green.Count; i++)
            {
                arrowObjs_Green[i].SetActive(true);
                arrowObjs_Grey[i].SetActive(false);
            }
            OutlineControl();
        }
    }

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
        if (Input.GetButtonDown("RPGConfirmPC") && isAtAlertTrigger && !isAtFirstTrigger)
        {
            DialogBoxManager.instance.HideDialog();
            GameManager.instance.dialogActive = false;
        }
        else if (Input.GetButtonDown("RPGConfirmPC") && isAtAlertTrigger && isAtFirstTrigger)
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
                DialogBoxManager.instance.HideDialog();
                ConversationModeManager.instance.HideFade(0.5f);
                currDialogLine = 0;
                GameManager.instance.dialogActive = false;
                for (int i = 0; i < arrowObjs_Green.Count; i++)
                {
                    arrowObjs_Green[i].SetActive(false);
                    arrowObjs_Grey[i].SetActive(true);
                }
                OutlineHide();
                Invoke("FinishedConversationControl", 0.5f);
            }
            else
            {
                ConversationModeManager.instance.Show3();
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                currDialogLine++;
            }
        }
    }

    void FinishedConversationControl()
    {
        if (onFinishedConversationCallback != null)
        {
            onFinishedConversationCallback.Invoke();
        }
    }
}
