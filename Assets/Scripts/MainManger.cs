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
    EndLab_Restart,
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

    [Header("After ending at lab")]
    public int afterEndAtLab_CurrDialogIndex;
    public int afterEndAtLab_CurrArrowIndex;
    public GameObject afterEndAtLab_Restart_RootObj;
    public List<GameObject> afterEndAtLab_Restart_ArrowObjs = new List<GameObject>();
    public int afterEndAtLab_Restart_CurrArrowIndex;

    [Header("Language")]
    public List<GameObject> langObjs_TC = new List<GameObject>();
    public List<GameObject> langObjs_SC = new List<GameObject>();
    public List<GameObject> langObjs_EN = new List<GameObject>();

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
        commonUtils.onChangeLangCallback += CommonUtils_OnChangeLang;
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
        afterEndAtLab_CurrDialogIndex = -1;
        afterEndAtLab_CurrArrowIndex = 0;
        afterEndAtLab_Restart_CurrArrowIndex = 0;
        afterEndAtLab_Restart_ArrowObjs[0].SetActive(true);
        afterEndAtLab_Restart_ArrowObjs[1].SetActive(false);
        afterEndAtLab_Restart_RootObj.SetActive(false);

        ChangeLanguage();

        commonUtils.isAtHomePage = true;
        currStage = MainStage.None;
    }

    private void InputManager_OnValueChanged_Vertical(int val)
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
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
                //你想前往哪個時期探險？ option
                else if (currStage == MainStage.StartLab && startLab_CurrDialogIndex == 4)
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
                else if (currStage == MainStage.EndLab_CollectionBookTrigger && afterEndAtLab_CurrDialogIndex == 2)
                {
                    SoundManager.instance.Play_Input(0);
                    if (afterEndAtLab_CurrArrowIndex == 0)
                    {
                        if (val == -1)
                        {
                            afterEndAtLab_CurrArrowIndex = 1;
                            DialogBoxManager.instance.SetOptionArrow(afterEndAtLab_CurrArrowIndex);
                        }
                    }
                    else if (afterEndAtLab_CurrArrowIndex == 1)
                    {
                        if (val == 1)
                        {
                            afterEndAtLab_CurrArrowIndex = 0;
                            DialogBoxManager.instance.SetOptionArrow(afterEndAtLab_CurrArrowIndex);
                        }
                    }
                }
                else if (currStage == MainStage.EndLab_Restart)
                {
                    SoundManager.instance.Play_Input(0);
                    if (afterEndAtLab_Restart_CurrArrowIndex == 0)
                    {
                        if (val == -1)
                        {
                            afterEndAtLab_Restart_ArrowObjs[afterEndAtLab_Restart_CurrArrowIndex].SetActive(false);
                            afterEndAtLab_Restart_CurrArrowIndex = 1;
                            afterEndAtLab_Restart_ArrowObjs[afterEndAtLab_Restart_CurrArrowIndex].SetActive(true);
                        }
                    }
                    else if (afterEndAtLab_Restart_CurrArrowIndex == 1)
                    {
                        if (val == 1)
                        {
                            afterEndAtLab_Restart_ArrowObjs[afterEndAtLab_Restart_CurrArrowIndex].SetActive(false);
                            afterEndAtLab_Restart_CurrArrowIndex = 0;
                            afterEndAtLab_Restart_ArrowObjs[afterEndAtLab_Restart_CurrArrowIndex].SetActive(true);
                        }
                    }
                }
            }
        }
    }

    private void InputManager_OnValueChanged_Horizontal(int val)
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
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
        
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
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
                        }
                        else if (startLab_CurrDialogIndex == 1)
                        {
                            //旅途中有甚麼疑難就問我吧。相信我們將會是最佳拍檔！
                            startLab_CurrDialogIndex++;
                            DialogBoxManager.instance.HideControl();
                            DialogBoxManager.instance.ShowDialog(commonUtils.gameplayInstructions[startLab_CurrDialogIndex]);
                        }
                        else if (startLab_CurrDialogIndex == 2)
                        {
                            //你想前往哪個時期探險？ question only 
                            startLab_CurrDialogIndex++;
                            DialogBoxManager.instance.ShowDialog(commonUtils.gameplayInstructions[startLab_CurrDialogIndex]);
                        }
                        else if (startLab_CurrDialogIndex == 3)
                        {
                            //你想前往哪個時期探險？ option (carbon or permian)
                            DialogBoxManager.instance.ShowOption(commonUtils.gameplayInstructions[startLab_CurrDialogIndex]);
                            DialogBoxManager.instance.SetOptionArrow(startLab_CurrArrowIndex);
                            startLab_CurrDialogIndex++;
                        }
                        else if (startLab_CurrDialogIndex == 4)
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
                    if (DialogBoxManager.instance.dialogWriterSingle != null && DialogBoxManager.instance.dialogWriterSingle.IsActive())
                    {
                        DialogBoxManager.instance.FinishCurrentDialog();
                    }
                    else
                    {
                        if (afterEndAtLab_CurrDialogIndex == 0)
                        {
                            //來看看我們辛苦收集的資料吧！ question only
                            afterEndAtLab_CurrDialogIndex++;
                            DialogBoxManager.instance.ShowDialog(commonUtils.endCheck_AfterEndingVideos[afterEndAtLab_CurrDialogIndex]);
                        }
                        else if (afterEndAtLab_CurrDialogIndex == 1)
                        {
                            //來看看我們辛苦收集的資料吧！ option
                            DialogBoxManager.instance.ShowOption(commonUtils.endCheck_AfterEndingVideos[afterEndAtLab_CurrDialogIndex]);
                            afterEndAtLab_CurrDialogIndex++;
                        }
                        else if (afterEndAtLab_CurrDialogIndex == 2)
                        {
                            if (afterEndAtLab_CurrArrowIndex == 0)
                            {
                                currStage = MainStage.EndLab_CollectionBookUpdate;
                                DialogBoxManager.instance.HideDialog();
                                CollectionBookManager.instance.Show_Main();
                            }
                            else
                            {
                                //restart game double confirm popup
                                currStage = MainStage.EndLab_Restart;
                                afterEndAtLab_Restart_RootObj.SetActive(true);
                            }
                        }

                        //if (afterEndAtLab_CurrDialogIndex == commonUtils.endCheck_AfterEndingVideos.Count - 1)
                        //{
                        //    if (afterEndAtLab_CurrArrowIndex == 0)
                        //    {
                        //        currStage = MainStage.EndLab_CollectionBookUpdate;
                        //        DialogBoxManager.instance.HideDialog();
                        //        CollectionBookManager.instance.Show_Main();
                        //    }
                        //    else
                        //    {
                        //        //restart game double confirm popup
                        //        currStage = MainStage.EndLab_Restart;
                        //        afterEndAtLab_Restart_RootObj.SetActive(true);
                        //    }
                        //}
                        //else
                        //{
                        //    afterEndAtLab_CurrDialogIndex++;
                        //    DialogBoxManager.instance.ShowDialog(commonUtils.endCheck_AfterEndingVideos[afterEndAtLab_CurrDialogIndex]);
                        //}
                    }
                }
                else if (currStage == MainStage.EndLab_Restart)
                {
                    SoundManager.instance.Play_Input(2);
                    if (afterEndAtLab_Restart_CurrArrowIndex == 0)
                    {
                        commonUtils.ResetGame();
                    }
                    else if (afterEndAtLab_Restart_CurrArrowIndex == 1)
                    {
                        afterEndAtLab_Restart_RootObj.SetActive(false);
                        currStage = MainStage.EndLab_CollectionBookTrigger;
                    }
                }
            }
        }

        
    }

    private void InputManager_OnValueChanged_Option()
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
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
        TimeoutManager.instance.Setup();

        StatusBarManager.instance.Hide_Carbon(0f);
        StatusBarManager.instance.Hide_Permian(0f);
        MinimapManager.instance.Hide(0f);

        PlayerController.instance.SetDirection(PlayerDirection.Down);
        PlayerController.instance.transform.position = new Vector3(-0.96f, 0f, 0f);
        DroneController.instance.ChangePos(new Vector3(0.68f, -0.49f, 0f));
        DroneController.instance.canShowTalkHint = false;
    }

    private void CommonUtils_OnChangeLang()
    {
        ChangeLanguage();
    }
    public void ChangeLanguage()
    {
        if (commonUtils.currLang == Language.TC)
        {
            for (int i = 0; i < langObjs_TC.Count; i++)
            {
                langObjs_TC[i].SetActive(true);
            }
            for (int i = 0; i < langObjs_SC.Count; i++)
            {
                langObjs_SC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_EN.Count; i++)
            {
                langObjs_EN[i].SetActive(false);
            }
        }
        else if (commonUtils.currLang == Language.SC)
        {
            for (int i = 0; i < langObjs_TC.Count; i++)
            {
                langObjs_TC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_SC.Count; i++)
            {
                langObjs_SC[i].SetActive(true);
            }
            for (int i = 0; i < langObjs_EN.Count; i++)
            {
                langObjs_EN[i].SetActive(false);
            }
        }
        else if (commonUtils.currLang == Language.EN)
        {
            for (int i = 0; i < langObjs_TC.Count; i++)
            {
                langObjs_TC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_SC.Count; i++)
            {
                langObjs_SC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_EN.Count; i++)
            {
                langObjs_EN[i].SetActive(true);
            }
        }
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
        homeControl.SetAlpha(0, 0.5f);
        langGrp_CanvasGrp.DOFade(1f, 0.5f).OnComplete(ChangeStageLangCompleted);
    }
    void ChangeStageLangCompleted()
    {
        commonUtils.isAtHomePage = false;
        currStage = MainStage.Language;
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
        afterEndAtLab_CurrDialogIndex++;
        DialogBoxManager.instance.ShowDialog(commonUtils.endCheck_AfterEndingVideos[afterEndAtLab_CurrDialogIndex]);
        OptionManager.instance.SetActive(true);
        inputManager.canInput_Option = true;
    }

    private void OnDestroy()
    {
        commonUtils.onSetupDoneCallback -= CommonUtils_OnSetupDone;
        commonUtils.onChangeLangCallback -= CommonUtils_OnChangeLang;

        inputManager.onValueChanged_VerticalCallback -= InputManager_OnValueChanged_Vertical;
        inputManager.onValueChanged_HorizontalCallback -= InputManager_OnValueChanged_Horizontal;
        inputManager.onValueChanged_ConfirmCallback -= InputManager_OnValueChanged_Confirm;
        inputManager.onValueChanged_OptionCallback -= InputManager_OnValueChanged_Option;

        introVideoManager.onVideoStartedCallback -= IntroVideoManager_OnVideoStarted;
        introVideoManager.onVideoFinishedCallback -= IntroVideoManager_OnVideoFinished;
    }
}
