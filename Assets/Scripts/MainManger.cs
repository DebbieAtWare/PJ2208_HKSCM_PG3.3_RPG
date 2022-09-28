using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum MainStage
{
    None,
    Home,
    Language,
    Intro,
    StartLab,
    InGame
}

public class MainManger : MonoBehaviour
{
    public static MainManger instance;

    [Header("Home")]
    public CanvasGroup homeGrp_CanvasGrp;

    [Header("Language")]
    public CanvasGroup langGrp_CanvasGrp;
    public List<GameObject> langGrp_ArrowObjs = new List<GameObject>();
    public int langGrp_CurrIndex;

    [Header("Intro")]
    public CanvasGroup introGrp_CanvasGrp;

    [Header("Start Lab")]
    public int startLab_CurrDialogIndex;
    public int startLab_CurrArrowIndex;

    [Header("Ending")]
    public CanvasGroup endingGrp_CanvasGrp;

    [Header("Curr")]
    public MainStage currStage;

    CommonUtils commonUtils;
    InputManager inputManager;

    void Awake()
    {
        Debug.Log("MainManger Awake");
        if (instance != null)
        {
            Debug.Log("More than one instance of MainManger");
            return;
        }
        instance = this;
    }

    void Start()
    {
        commonUtils = CommonUtils.instance;
        commonUtils.onSetupDoneCallback += CommonUtils_OnSetupDone;
        commonUtils.Setup();

        inputManager = InputManager.instance;
        inputManager.onValueChanged_VerticalCallback += InputManager_OnValueChanged_Vertical;
        inputManager.onValueChanged_HorizontalCallback += InputManager_OnValueChanged_Horizontal;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;
        inputManager.onValueChanged_OptionCallback += InputManager_OnValueChanged_Option;
        inputManager.canInput_Vertical = true;
        inputManager.canInput_Horizontal = true;
        inputManager.canInput_Confirm = true;

        homeGrp_CanvasGrp.alpha = 1;
        for (int i = 0; i < langGrp_ArrowObjs.Count; i++)
        {
            if (i == 0)
            {
                langGrp_ArrowObjs[i].SetActive(true);
            }
            else
            {
                langGrp_ArrowObjs[i].SetActive(false);
            }
        }
        langGrp_CurrIndex = 0;
        langGrp_CanvasGrp.alpha = 0;
        introGrp_CanvasGrp.alpha = 0;
        startLab_CurrDialogIndex = -1;
        startLab_CurrArrowIndex = 0;
        endingGrp_CanvasGrp.alpha = 0;

        currStage = MainStage.None;
    }

    private void InputManager_OnValueChanged_Vertical(int val)
    {
        if (OptionManager.instance.currStage == OptionStage.None)
        {
            if (currStage == MainStage.None)
            {
                ChangeStage_Language();
            }
            else if (currStage == MainStage.Language)
            {
                if (langGrp_CurrIndex == 0)
                {
                    if (val == -1)
                    {
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(false);
                        langGrp_CurrIndex = 1;
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(true);
                    }
                }
                else if (langGrp_CurrIndex == 1)
                {
                    if (val == 1)
                    {
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(false);
                        langGrp_CurrIndex = 0;
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(true);
                    }
                    else if (val == -1)
                    {
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(false);
                        langGrp_CurrIndex = 2;
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(true);
                    }
                }
                else if (langGrp_CurrIndex == 2)
                {
                    if (val == 1)
                    {
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(false);
                        langGrp_CurrIndex = 1;
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(true);
                    }
                }
            }
            else if (currStage == MainStage.StartLab && startLab_CurrDialogIndex == 2)
            {
                if (startLab_CurrArrowIndex == 0)
                {
                    if (val == -1)
                    {
                        startLab_CurrArrowIndex = 1;
                        DialogBoxManager.instance.SetOptionArrow(startLab_CurrArrowIndex);
                    }
                }
                else if (startLab_CurrArrowIndex == 1)
                {
                    if (val == 1)
                    {
                        startLab_CurrArrowIndex = 0;
                        DialogBoxManager.instance.SetOptionArrow(startLab_CurrArrowIndex);
                    }
                }
            }
        }
    }

    private void InputManager_OnValueChanged_Horizontal(int val)
    {
        if (OptionManager.instance.currStage == OptionStage.None)
        {
            if (currStage == MainStage.None)
            {
                ChangeStage_Language();
            }
        }
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (OptionManager.instance.currStage == OptionStage.None)
        {
            if (currStage == MainStage.None)
            {
                ChangeStage_Language();
            }
            else if (currStage == MainStage.Language)
            {
                if (langGrp_CurrIndex == 0)
                {
                    commonUtils.ChangeLanguage(Language.TC);
                }
                else if (langGrp_CurrIndex == 1)
                {
                    commonUtils.ChangeLanguage(Language.SC);
                }
                else if (langGrp_CurrIndex == 2)
                {
                    commonUtils.ChangeLanguage(Language.EN);
                }
                ChangeStage_Intro();
            }
            else if (currStage == MainStage.StartLab)
            {
                if (DialogBoxManager.instance.dialogWriterSingle.IsActive())
                {
                    DialogBoxManager.instance.FinishCurrentDialog();
                }
                else
                {
                    if (startLab_CurrDialogIndex == 0)
                    {
                        Vector3 targetPos = new Vector3(-0.7f, 0, 0);
                        float dist = Vector3.Distance(PlayerController.instance.transform.position, targetPos);
                        float time = dist * commonUtils.playerAutoWalkSpeed;
                        PlayerController.instance.transform.DOMove(targetPos, time);
                        startLab_CurrDialogIndex++;
                        DialogBoxManager.instance.ShowControl();
                    }
                    else if (startLab_CurrDialogIndex == 1)
                    {
                        startLab_CurrDialogIndex++;
                        DialogBoxManager.instance.HideControl();
                        DialogBoxManager.instance.ShowDialog(commonUtils.gameplayInstructions[startLab_CurrDialogIndex]);
                        DialogBoxManager.instance.SetOptionArrow(startLab_CurrArrowIndex);
                    }
                    else if (startLab_CurrDialogIndex == 2)
                    {
                        DialogBoxManager.instance.HideDialog();
                        if (startLab_CurrArrowIndex == 0)
                        {
                            TransitionManager.instance.ChangeMap(commonUtils.currMapId, MapID.Carboniferous);
                        }
                        else if (startLab_CurrArrowIndex == 1)
                        {
                            TransitionManager.instance.ChangeMap(commonUtils.currMapId, MapID.Permian);
                        }
                    }
                }
            }
        }
    }

    private void InputManager_OnValueChanged_Option()
    {
        if (OptionManager.instance.currStage == OptionStage.None)
        {
            if (currStage == MainStage.None)
            {
                ChangeStage_Language();
            }
        }
    }

    private void CommonUtils_OnSetupDone()
    {
        GameManager.instance.dialogActive = true;

        StatusBarManager.instance.Setup();
        CollectionBookManager.instance.Setup();
        OptionManager.instance.Setup();
        DialogBoxManager.instance.Setup();

        StatusBarManager.instance.Hide_Carbon(0f);
        StatusBarManager.instance.Hide_Permian(0f);
        MinimapManager.instance.Hide(0f);

        PlayerController.instance.SetDirection(PlayerDirection.Down);
        PlayerController.instance.transform.position = new Vector3(-1.6f, 0f, 0f);
        DroneController.instance.ChangePos(new Vector3(0.8f, -0.8f, 0f));


        //TransitionManager.instance.ChangeMap(commonUtils.currMapId, MapID.Carboniferous);
        //TransitionManager.instance.ChangeMap(commonUtils.currMapId, MapID.Permian);
    }

    void ChangeStage_Language()
    {
        langGrp_CanvasGrp.DOFade(1f, 0.5f).OnComplete(() => currStage = MainStage.Language);
    }

    void ChangeStage_Intro()
    {
        //TODO play intro video
        currStage = MainStage.Intro;

        //tmp
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            introGrp_CanvasGrp.DOFade(1f, 1f);
            yield return new WaitForSeconds(3f);
            homeGrp_CanvasGrp.alpha = 0;
            langGrp_CanvasGrp.alpha = 0;
            introGrp_CanvasGrp.DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);
            ChangeStage_StartLab();
        }
    }

    void ChangeStage_StartLab()
    {
        currStage = MainStage.StartLab;
        DroneController.instance.ForceShowTalkHint();
        startLab_CurrDialogIndex++;
        DialogBoxManager.instance.ShowDialog(commonUtils.gameplayInstructions[startLab_CurrDialogIndex]);
    }
}
