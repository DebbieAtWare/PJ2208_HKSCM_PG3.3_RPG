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
    Transition_ConversationStart,
    ConversationMode,
    Transition_ConversationEnd
}

public class BossObject : MonoBehaviour
{
    public delegate void OnFinishedConversation();
    public OnFinishedConversation onFinishedConversationCallback;

    [Header("ID")]
    public CharacterID id;

    [Header("Sprite")]
    public Sprite dialogBoxProfileSprite;

    [Header("Renderer")]
    public Renderer bossRenderer;
    public Renderer caveRenderer;

    [Header("Trigger")]
    public OnTriggerControl alertTriggerControl;
    public OnTriggerControl viewTriggerControl;
    public GameObject avatarAutoPosObj;

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
    public bool canFadeOutAlertSFX;
    public bool isAtAlertTrigger;
    public bool isAtViewTrigger;
    public BossStage currBossStage;

    InputManager inputManager;
    CommonUtils commonUtils;
    DialogBoxManager dialogBoxManager;

    public void Setup(ConfigData_DialogBox _dialogBox_Alert, ConfigData_Character _info, bool _canShowAlert, bool isFirstMeetDone, bool _canFadeOutAlertSFX)
    {
        commonUtils = CommonUtils.instance;
        inputManager = InputManager.instance;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;
        dialogBoxManager = DialogBoxManager.instance;
        dialogBoxManager.onDialogEndCallback += DialogBoxManager_OnDialogEnd;
     

        dialogBox_Alert = _dialogBox_Alert;
        info = _info;
        canShowAlert = _canShowAlert;
        canFadeOutAlertSFX = _canFadeOutAlertSFX;

        alertTriggerControl.onTriggerEnterCallback += AlertTrigger_OnEnter;
        alertTriggerControl.onTriggerExitCallback += AlertTrigger_OnExit;

        viewTriggerControl.onTriggerEnterCallback += ViewTrigger_OnEnter;
        viewTriggerControl.onTriggerExitCallback += ViewTrigger_OnExit;

        currDialogLine = -1;
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
        if (OptionManager.instance.currStage == OptionStage.None)
        {
            if (currBossStage == BossStage.Alert)
            {
                if (DialogBoxManager.instance.dialogWriterSingle.IsActive())
                {
                    DialogBoxManager.instance.FinishCurrentDialog();
                }
                else
                {
                    StartCoroutine(Alert());
                    IEnumerator Alert()
                    {
                        SoundManager.instance.FadeOutStop_Dialog(0.3f);
                        SoundManager.instance.Play_Input(2);
                        SoundManager.instance.Play_SFX(5);
                        DialogBoxManager.instance.HideDialog();
                        float dist = Vector3.Distance(PlayerController.instance.transform.position, avatarAutoPosObj.transform.position);
                        float time = dist * commonUtils.playerAutoWalkSpeed;
                        PlayerController.instance.transform.DOMove(avatarAutoPosObj.transform.position, time);
                        PlayerController.instance.SetAutoWalk(1);
                        yield return new WaitForSeconds(time);
                        PlayerController.instance.SetAutoWalk(0);
                        if (canFadeOutAlertSFX)
                        {
                            //in carbon can't fade out caz auto walk and jump to cave sound using same track
                            SoundManager.instance.FadeOutStop_SFX(0.5f);
                        }
                    }
                }
            }
            else if (currBossStage == BossStage.View)
            {
                StartCoroutine(Ani());
                IEnumerator Ani()
                {
                    GameManager.instance.dialogActive = true;
                    currBossStage = BossStage.Transition_ConversationStart;
                    SoundManager.instance.Play_BGM(1);
                    ViewBoxManager.instance.HideViewBox_NPC();
                    SoundManager.instance.Play_Input(2);
                    MinimapManager.instance.Hide(0.5f);
                    StatusBarManager.instance.Hide_Carbon(0.5f);
                    StatusBarManager.instance.Hide_Permian(0.5f);
                    ConversationModeManager.instance.Ani_Start(id, info.Name_TC, info.DescriptionTag_TC);
                    ConversationModeManager.instance.BossAni_Idle();
                    yield return new WaitForSeconds(3f);
                    currDialogLine++;
                    DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                    if (info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M01.ToString() || info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M02.ToString() || info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M03.ToString())
                    {
                        ConversationModeManager.instance.BossAni_Talk();
                    }
                    currBossStage = BossStage.ConversationMode;
                }
            }
            else if (currBossStage == BossStage.ConversationMode)
            {
                SoundManager.instance.Play_Input(2);
                if (DialogBoxManager.instance.dialogWriterSingle.IsActive())
                {
                    DialogBoxManager.instance.FinishCurrentDialog();
                    ConversationModeManager.instance.BossAni_Idle();
                }
                else
                {

                    if (currDialogLine == 0)
                    {
                        SoundManager.instance.Play_Input(2);
                        ConversationModeManager.instance.Ani_AvatarIn();
                        currDialogLine++;
                        DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                        if (info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M01.ToString() || info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M02.ToString() || info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M03.ToString())
                        {
                            ConversationModeManager.instance.BossAni_Talk();
                        }
                    }
                    else if (currDialogLine == (info.DialogBoxes.Count - 2))
                    {
                        ConversationModeManager.instance.Ani_BossCenter();
                        currDialogLine++;
                        DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                        if (info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M01.ToString() || info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M02.ToString() || info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M03.ToString())
                        {
                            ConversationModeManager.instance.BossAni_Talk();
                        }
                    }
                    else if (currDialogLine == (info.DialogBoxes.Count - 1))
                    {
                        StartCoroutine(LastLine());
                        IEnumerator LastLine()
                        {
                            currBossStage = BossStage.Transition_ConversationEnd;
                            DialogBoxManager.instance.HideDialog();
                            ConversationModeManager.instance.HideFade(1f);
                            currDialogLine = -1;
                            for (int i = 0; i < arrowObjs_Green.Count; i++)
                            {
                                arrowObjs_Green[i].SetActive(false);
                                arrowObjs_Grey[i].SetActive(true);
                            }
                            minimapDot_Red.SetActive(false);
                            minimapDot_Grey.SetActive(true);
                            OutlineHide();
                            yield return new WaitForSeconds(1f);
                            currBossStage = BossStage.None;
                            if (onFinishedConversationCallback != null)
                            {
                                onFinishedConversationCallback.Invoke();
                            }
                        }
                    }
                    else
                    {
                        ConversationModeManager.instance.Ani_AvatarOut();
                        currDialogLine++;
                        DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                        if (info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M01.ToString() || info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M02.ToString() || info.DialogBoxes[currDialogLine].ByWhom == CharacterID.M03.ToString())
                        {
                            ConversationModeManager.instance.BossAni_Talk();
                        }
                    }
                }
            }
        }
    }

    private void DialogBoxManager_OnDialogEnd()
    {
        ConversationModeManager.instance.BossAni_Idle();
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
            StartCoroutine(Alert());
            IEnumerator Alert()
            {
                GameManager.instance.dialogActive = true;
                DroneController.instance.canShowTalkHint = false;
                DroneController.instance.HideTalkHint();
                SoundManager.instance.Play_BGM(5);
                yield return new WaitForSeconds(0.5f);
                DialogBoxManager.instance.ShowDialog(dialogBox_Alert);
                currBossStage = BossStage.Alert;
                yield return new WaitForSeconds(0.5f);
                SoundManager.instance.FadeOutStop_SFX(0.5f);
            }
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
        ViewBoxManager.instance.ShowViewBox_NPC();
        currBossStage = BossStage.View;
    }

    private void ViewTrigger_OnExit()
    {
        isAtViewTrigger = false;
        ViewBoxManager.instance.HideViewBox_NPC();
        currBossStage = BossStage.None;
    }

    //-------

    private void OnDestroy()
    {
        inputManager.onValueChanged_ConfirmCallback -= InputManager_OnValueChanged_Confirm;

        alertTriggerControl.onTriggerEnterCallback -= AlertTrigger_OnEnter;
        alertTriggerControl.onTriggerExitCallback -= AlertTrigger_OnExit;

        viewTriggerControl.onTriggerEnterCallback -= ViewTrigger_OnEnter;
        viewTriggerControl.onTriggerExitCallback -= ViewTrigger_OnExit;
    }
}
