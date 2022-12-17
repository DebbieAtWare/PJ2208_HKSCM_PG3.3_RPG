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
    InGame,
    EndLab_CollectionBookTrigger,
    EndLab_CollectionBookUpdate
}

public class MainManger : MonoBehaviour
{
    public static MainManger instance;

    public delegate void OnEndingVideoFinished();
    public OnEndingVideoFinished onEndingVideoFinishedCallback;

    [Header("Home")]
    public HomeControl homeControl;

    [Header("Language")]
    public CanvasGroup langGrp_CanvasGrp;
    public List<GameObject> langGrp_ArrowObjs = new List<GameObject>();
    public int langGrp_CurrIndex;

    [Header("Start Lab")]
    public int startLab_CurrDialogIndex;
    public int startLab_CurrArrowIndex;

    [Header("Curr")]
    public MainStage currStage;

    CommonUtils commonUtils;
    InputManager inputManager;
    IntroVideoManager introVideoManager;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("MainManger Awake");
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
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

        introVideoManager = IntroVideoManager.instance;
        introVideoManager.onVideoStartedCallback += IntroVideoManager_OnVideoStarted;
        introVideoManager.onVideoFinishedCallback += IntroVideoManager_OnVideoFinished;

        SoundManager.instance.Play_BGM(0);

        homeControl.SetAlpha(1, 0);
        homeControl.PlayBkg();

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
        startLab_CurrDialogIndex = -1;
        startLab_CurrArrowIndex = 0;

        currStage = MainStage.None;
    }

    private void InputManager_OnValueChanged_Vertical(int val)
    {
        if (OptionManager.instance.currStage == OptionStage.None)
        {
            if (currStage == MainStage.None)
            {
                SoundManager.instance.Play_Input(0);
                ChangeStage_Language();
            }
            else if (currStage == MainStage.Language)
            {
                SoundManager.instance.Play_Input(0);
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
                SoundManager.instance.Play_Input(0);
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
                SoundManager.instance.Play_Input(0);
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
                SoundManager.instance.Play_Input(2);
                ChangeStage_Language();
            }
            else if (currStage == MainStage.Language)
            {
                SoundManager.instance.Play_Input(2);
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
                currStage = MainStage.Intro;
                introVideoManager.Play();
            }
            else if (currStage == MainStage.StartLab)
            {
                SoundManager.instance.Play_Input(2);
                if (DialogBoxManager.instance.dialogWriterSingle != null && DialogBoxManager.instance.dialogWriterSingle.IsActive())
                {
                    DialogBoxManager.instance.FinishCurrentDialog();
                }
                else
                {
                    if (startLab_CurrDialogIndex == 0)
                    {
                        //first dialog is text, second dialog in control diagram
                        startLab_CurrDialogIndex++;
                        DialogBoxManager.instance.ShowControl();
                        //DialogBoxManager.instance.ShowDialog(commonUtils.gameplayInstructions[startLab_CurrDialogIndex]);
                        //DialogBoxManager.instance.HideControl();
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
            else if (currStage == MainStage.EndLab_CollectionBookTrigger)
            {
                SoundManager.instance.Play_Input(2);
                currStage = MainStage.EndLab_CollectionBookUpdate;
                DialogBoxManager.instance.HideDialog();
                CollectionBookManager.instance.Show_Main();
            }
        }
    }

    private void InputManager_OnValueChanged_Option()
    {
        if (OptionManager.instance.currStage == OptionStage.None)
        {
            if (currStage == MainStage.None)
            {
                SoundManager.instance.Play_Input(2);
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
        IntroVideoManager.instance.Setup();
        EndVideoManager.instance.Setup();
        ViewBoxManager.instance.Setup();
        ConversationModeManager.instance.Setup();

        StatusBarManager.instance.Hide_Carbon(0f);
        StatusBarManager.instance.Hide_Permian(0f);
        MinimapManager.instance.Hide(0f);

        PlayerController.instance.SetDirection(PlayerDirection.Down);
        PlayerController.instance.transform.position = new Vector3(-0.96f, 0f, 0f);
        DroneController.instance.ChangePos(new Vector3(0.68f, -0.49f, 0f));
        DroneController.instance.canShowTalkHint = false;
    }

    private void IntroVideoManager_OnVideoStarted()
    {
        homeControl.ResetAll();
        langGrp_CanvasGrp.alpha = 0;
    }
    private void IntroVideoManager_OnVideoFinished()
    {
        ChangeStage_StartLab();
    }

    void ChangeStage_Language()
    {
        langGrp_CanvasGrp.DOFade(1f, 0.5f).OnComplete(() => currStage = MainStage.Language);
    }

    void ChangeStage_StartLab()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            currStage = MainStage.StartLab;
            SoundManager.instance.Play_BGM(4);
            OptionManager.instance.SetActive(true);
            yield return new WaitForSeconds(1f);
            DroneController.instance.canShowTalkHint = true;
            DroneController.instance.ForceShowTalkHint();
            //first dialog is text, second dialog in control diagram
            startLab_CurrDialogIndex++;
            DialogBoxManager.instance.ShowDialog(commonUtils.gameplayInstructions[startLab_CurrDialogIndex]);
            //DialogBoxManager.instance.ShowControl();
            SoundManager.instance.Play_Dialog(1);
            yield return new WaitForSeconds(0.5f);
            SoundManager.instance.FadeOutStop_Dialog(0.3f);
        }
    }

    public void ChangeStage_EndLab()
    {
        currStage = MainStage.EndLab_CollectionBookTrigger;
        SoundManager.instance.Play_BGM(4);
        DialogBoxManager.instance.ShowDialog(commonUtils.endCheck_AfterEndingVideo);
        OptionManager.instance.SetActive(true);
        inputManager.canInput_Option = true;
    }
}
