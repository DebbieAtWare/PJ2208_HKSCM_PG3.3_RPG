﻿using FlexFramework.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MapID
{
    Lab,
    Carboniferous,
    Permian
}

public enum CharacterID
{
    AVA,
    DRO,
    M01,
    M02,
    M03,
    NPC_C01,
    NPC_C02,
    NPC_C03,
    NPC_C04,
    NPC_C05,
    NPC_C06,
    NPC_P01,
    NPC_P02,
    NPC_P03,
    NPC_P04,
    NPC_P05,
    NPC_P06,
    NPC_P07,
    NPC_P08,
    NPC_P09,
    NPC_P10,
}

public enum Language
{
    TC,
    SC,
    EN
}

public enum EndingCheckStage
{
    None,
    ToCarboniferous,
    OneLeftInPermian,
    ToPermian,
    ToEndingVideo
}

public class CommonUtils : MonoBehaviour
{
    public static CommonUtils instance;

    public delegate void OnSetupDone();
    public OnSetupDone onSetupDoneCallback;

    public delegate void OnChangeLang();
    public OnChangeLang onChangeLangCallback;

    [Header("PlayerPos")]
    public Vector3 playerPos_Carboniferous;
    public Vector3 playerPos_Permian;
    public Vector3 playerPos_InsideTreeCave;
    public Vector3 playerPos_OutsideTreeCave;

    [Header("PlayerDir")]
    public PlayerDirection playerDir_Carboniferous;
    public PlayerDirection playerDir_Permian;
    public PlayerDirection playerDir_InsideTreeCave;
    public PlayerDirection playerDir_OutsideTreeCave;

    [Header("DronePos")]
    public Vector3 dronePos_Carboniferous;
    public Vector3 dronePos_Permian;
    public Vector3 dronePos_InsideTreeCave;
    public Vector3 dronePos_OutsideTreeCave;

    [Header("AutoWalkSpeed")]
    //1 unit use how many sec
    public float playerAutoWalkSpeed = 0.2f;
    public float droneAutoWalkSpeed = 0.2f;

    [Header("ConfigData - Intro")]
    public List<ConfigData_DialogBox> introVideoDialogs = new List<ConfigData_DialogBox>();

    [Header("ConfigData - Gameplay Instruction")]
    public List<ConfigData_DialogBox> gameplayInstructions = new List<ConfigData_DialogBox>();

    [Header("ConfigData - First Greeting")]
    public List<ConfigData_DialogBox> firstGreeting_Carboniferous = new List<ConfigData_DialogBox>();
    public List<ConfigData_DialogBox> firstGreeting_Permian = new List<ConfigData_DialogBox>();

    [Header("ConfigData - General interaction")]
    public ConfigData_Text generalInteraction_Drone;
    public ConfigData_Text generalInteraction_NPC;
    public ConfigData_DialogBox dialogBox_BossAlert;
    public ConfigData_Text successCollectText;

    [Header("ConfigData - Drone")]
    public ConfigData_DialogBox dialogBox_TipsByDrone;
    public List<ConfigData_DialogBox> dialogBox_TipsByDrone_Hints = new List<ConfigData_DialogBox>();
    public ConfigData_DialogBox dialogBox_TipsByDrone_CollectionBook;
    public ConfigData_DialogBox dialogBox_TipsByDrone_ChangeMap;

    [Header("ConfigData - End Check")]
    public ConfigData_DialogBox endCheck_ChangeToPermian = new ConfigData_DialogBox();
    public ConfigData_DialogBox endCheck_PermianOneLeft = new ConfigData_DialogBox();
    public ConfigData_DialogBox endCheck_ChangeToCarboniferous = new ConfigData_DialogBox();
    public List<ConfigData_DialogBox> endCheck_ChangeToEndingVideos = new List<ConfigData_DialogBox>();
    public List<ConfigData_DialogBox> endCheck_AfterEndingVideos = new List<ConfigData_DialogBox>();
    int endCheck_ChangeToEndingVideoDialogIndex = -1;

    [Header("ConfigData - Ending")]
    public List<ConfigData_Text> endVideoTexts = new List<ConfigData_Text>();

    //----

    [Header("ConfigData - Conversation Boss")]
    public List<ConfigData_Character> bosses = new List<ConfigData_Character>();

    [Header("ConfigData - Conversation NPC")]
    public List<ConfigData_Character> NPC_Carboniferous = new List<ConfigData_Character>();
    public List<ConfigData_Character> NPC_Permian = new List<ConfigData_Character>();

    [Header("Curr")]
    public MapID currMapId;
    public Language currLang;
    public EndingCheckStage currEndingCheck;
    public bool isAtHomePage;

    InputManager inputManager;
    EndVideoManager endVideoManager;

    //for CMS excel
    public delegate void DownloadHandler(byte[] bytes);

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("CommonUtils Awake");
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

    public void Setup()
    {
        Debug.Log("CommonUtils Start");

        SceneManager.sceneLoaded += OnSceneLoaded;

        inputManager = InputManager.instance;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;

        endVideoManager = EndVideoManager.instance;

        endCheck_ChangeToEndingVideoDialogIndex = -1;
        currEndingCheck = EndingCheckStage.None;

        TmpExcelControl();
        LoadCMSExcel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("!!CommonUtils OnSceneLoaded " + scene.name + "  " + loadSceneMode.ToString());

        if (scene.name == "MainScene")
        {
            Debug.Log("MainScene");
        }
        else if (scene.name == "CarboniferousScene")
        {
            CarboniferousManager.instance.Setup();
        }
        else if (scene.name == "Boss01Scene")
        {
            Boss01Manager.instance.Setup();
        }
        else if (scene.name == "PermianScene")
        {
            PermianManager.instance.Setup();
        }
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
        {
            if (OptionManager.instance.currStage == OptionStage.None)
            {
                if (currEndingCheck != EndingCheckStage.None)
                {
                    SoundManager.instance.Play_Input(2);
                    if (DialogBoxManager.instance.dialogWriterSingle.IsActive())
                    {
                        DialogBoxManager.instance.FinishCurrentDialog();
                    }
                    else
                    {
                        if (currEndingCheck == EndingCheckStage.OneLeftInPermian)
                        {
                            DialogBoxManager.instance.HideDialog();
                            GameManager.instance.dialogActive = false;
                            currEndingCheck = EndingCheckStage.None;
                        }
                        else if (currEndingCheck == EndingCheckStage.ToPermian)
                        {
                            DialogBoxManager.instance.HideDialog();
                            TransitionManager.instance.ChangeMap(currMapId, MapID.Permian);
                        }
                        else if (currEndingCheck == EndingCheckStage.ToCarboniferous)
                        {
                            DialogBoxManager.instance.HideDialog();
                            TransitionManager.instance.ChangeMap(currMapId, MapID.Carboniferous);
                        }
                        else if (currEndingCheck == EndingCheckStage.ToEndingVideo)
                        {
                            if (DialogBoxManager.instance.dialogWriterSingle.IsActive())
                            {
                                DialogBoxManager.instance.FinishCurrentDialog();
                            }
                            else
                            {
                                if (endCheck_ChangeToEndingVideoDialogIndex == (endCheck_ChangeToEndingVideos.Count - 1))
                                {
                                    currEndingCheck = EndingCheckStage.None;
                                    DialogBoxManager.instance.HideDialog();
                                    StatusBarManager.instance.Hide_Carbon(0);
                                    StatusBarManager.instance.Hide_Permian(0);
                                    MinimapManager.instance.Hide(0);
                                    OptionManager.instance.SetActive(false);
                                    inputManager.canInput_Option = false;
                                    endVideoManager.Play();
                                }
                                else
                                {
                                    endCheck_ChangeToEndingVideoDialogIndex++;
                                    DialogBoxManager.instance.ShowDialog(endCheck_ChangeToEndingVideos[endCheck_ChangeToEndingVideoDialogIndex]);
                                }
                            }

                        }
                    }
                }
            }
        }

        
    }

    private void VideoManager_OnVideoFinished_Ending()
    {
        TransitionManager.instance.EndingVideoToLab();
    }

    public void ChangeLanguage(Language lang)
    {
        currLang = lang;
        if (onChangeLangCallback != null)
        {
            onChangeLangCallback.Invoke();
        }
    }

    void LoadCMSExcel()
    {
        StartCoroutine(LoadFileAsync("Exh3.3_Content.xlsx", bytes =>
        {
            var book = new WorkBook(bytes);

            int general_IDCol = 3;
            int general_CharacterImgCol = 5;
            int general_TCCol = 6;
            int general_SCCol = 7;
            int general_ENCol = 8;
            int general_Option1_TCCol = 9;
            int general_Option1_SCCol = 10;
            int general_Option1_ENCol = 11;
            int general_Option2_TCCol = 12;
            int general_Option2_SCCol = 13;
            int general_Option2_ENCol = 14;
            int general_Option3_TCCol = 15;
            int general_Option3_SCCol = 16;
            int general_Option3_ENCol = 17;
            int general_Option4_TCCol = 18;
            int general_Option4_SCCol = 19;
            int general_Option4_ENCol = 20;

            //book[0] = General
            #region General
            for (int i = 0; i < 130; i++)
            {
                //intro video
                if (book[0].Rows[i][general_IDCol].Text == "2.01" ||
                    book[0].Rows[i][general_IDCol].Text == "2.02" ||
                    book[0].Rows[i][general_IDCol].Text == "2.03" ||
                    book[0].Rows[i][general_IDCol].Text == "2.04" ||
                    book[0].Rows[i][general_IDCol].Text == "2.05" ||
                    book[0].Rows[i][general_IDCol].Text == "2.06" ||
                    book[0].Rows[i][general_IDCol].Text == "2.07" ||
                    book[0].Rows[i][general_IDCol].Text == "2.08" ||
                    book[0].Rows[i][general_IDCol].Text == "2.09")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    introVideoDialogs.Add(dialog);
                }
                //game play instruction
                else if (book[0].Rows[i][general_IDCol].Text == "3.01" ||
                         book[0].Rows[i][general_IDCol].Text == "3.02" ||
                         book[0].Rows[i][general_IDCol].Text == "3.03")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    gameplayInstructions.Add(dialog);
                }
                //game play instruction
                else if (book[0].Rows[i][general_IDCol].Text == "3.04")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    List<string> option_TC = new List<string>();
                    List<string> option_SC = new List<string>();
                    List<string> option_EN = new List<string>();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_TCCol].Text)) && 
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option1_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option1_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option1_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option2_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option2_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option2_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option3_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option3_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option3_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option4_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option4_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option4_ENCol].Text));
                    }
                    dialog.OptionTexts_TC = option_TC;
                    dialog.OptionTexts_SC = option_SC;
                    dialog.OptionTexts_EN = option_EN;
                    gameplayInstructions.Add(dialog);
                }
                //greeting in carbon
                else if (book[0].Rows[i][general_IDCol].Text == "5.01" ||
                         book[0].Rows[i][general_IDCol].Text == "5.02" ||
                         book[0].Rows[i][general_IDCol].Text == "5.03")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    firstGreeting_Carboniferous.Add(dialog);
                }
                //greeting in permian
                else if (book[0].Rows[i][general_IDCol].Text == "5.04" ||
                         book[0].Rows[i][general_IDCol].Text == "5.05" ||
                         book[0].Rows[i][general_IDCol].Text == "5.06")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    firstGreeting_Permian.Add(dialog);
                }
                //interaction with drone
                else if (book[0].Rows[i][general_IDCol].Text == "6.01")
                {
                    ConfigData_Text text = new ConfigData_Text();
                    text.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    text.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    text.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    generalInteraction_Drone = text;
                }
                //interaction with npc
                else if (book[0].Rows[i][general_IDCol].Text == "6.02")
                {
                    ConfigData_Text text = new ConfigData_Text();
                    text.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    text.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    text.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    generalInteraction_NPC = text;
                }
                //interaction with boss alert
                else if (book[0].Rows[i][general_IDCol].Text == "6.03")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    dialogBox_BossAlert = dialog;
                }
                //interaction boss success collect
                else if (book[0].Rows[i][general_IDCol].Text == "6.04")
                {
                    ConfigData_Text text = new ConfigData_Text();
                    text.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    text.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    text.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    successCollectText = text;
                }
                //Tips by drone
                else if (book[0].Rows[i][general_IDCol].Text == "7.01")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    List<string> option_TC = new List<string>();
                    List<string> option_SC = new List<string>();
                    List<string> option_EN = new List<string>();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option1_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option1_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option1_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option2_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option2_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option2_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option3_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option3_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option3_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option4_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option4_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option4_ENCol].Text));
                    }
                    dialog.OptionTexts_TC = option_TC;
                    dialog.OptionTexts_SC = option_SC;
                    dialog.OptionTexts_EN = option_EN;
                    dialogBox_TipsByDrone = dialog;
                }
                //Tips with drone - hints
                else if (book[0].Rows[i][general_IDCol].Text == "7.02" ||
                         book[0].Rows[i][general_IDCol].Text == "7.03" ||
                         book[0].Rows[i][general_IDCol].Text == "7.04")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    dialogBox_TipsByDrone_Hints.Add(dialog);
                }
                //Tips by drone - collection book
                else if (book[0].Rows[i][general_IDCol].Text == "7.05")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    List<string> option_TC = new List<string>();
                    List<string> option_SC = new List<string>();
                    List<string> option_EN = new List<string>();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option1_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option1_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option1_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option2_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option2_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option2_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option3_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option3_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option3_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option4_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option4_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option4_ENCol].Text));
                    }
                    dialog.OptionTexts_TC = option_TC;
                    dialog.OptionTexts_SC = option_SC;
                    dialog.OptionTexts_EN = option_EN;
                    dialogBox_TipsByDrone_CollectionBook = dialog;
                }
                //Tips by drone - change map
                else if (book[0].Rows[i][general_IDCol].Text == "7.06")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    List<string> option_TC = new List<string>();
                    List<string> option_SC = new List<string>();
                    List<string> option_EN = new List<string>();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option1_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option1_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option1_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option2_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option2_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option2_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option3_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option3_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option3_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option4_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option4_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option4_ENCol].Text));
                    }
                    dialog.OptionTexts_TC = option_TC;
                    dialog.OptionTexts_SC = option_SC;
                    dialog.OptionTexts_EN = option_EN;
                    dialogBox_TipsByDrone_ChangeMap = dialog;
                }
                //Mission complete - M1 found - Force to Permian
                else if (book[0].Rows[i][general_IDCol].Text == "9.01")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    endCheck_ChangeToPermian = dialog;
                }
                //Mission complete - M2&M3 found - Force to Carboniferous
                else if (book[0].Rows[i][general_IDCol].Text == "9.02")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    endCheck_ChangeToCarboniferous = dialog;
                }
                //Mission complete - Only M2/M3 found
                else if (book[0].Rows[i][general_IDCol].Text == "9.03")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    endCheck_PermianOneLeft = dialog;
                }
                //Mission complete - endCheck_ChangeToEndingVideos
                else if (book[0].Rows[i][general_IDCol].Text == "9.04")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    endCheck_ChangeToEndingVideos.Add(dialog);
                }
                //Mission complete - Ending video
                else if (book[0].Rows[i][general_IDCol].Text == "9.05" ||
                         book[0].Rows[i][general_IDCol].Text == "9.06" ||
                         book[0].Rows[i][general_IDCol].Text == "9.07" ||
                         book[0].Rows[i][general_IDCol].Text == "9.08")
                {
                    ConfigData_Text text = new ConfigData_Text();
                    text.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    text.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    text.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    endVideoTexts.Add(text);
                }
                //Mission complete - endCheck_AfterEndingVideos
                else if (book[0].Rows[i][general_IDCol].Text == "9.09")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    endCheck_AfterEndingVideos.Add(dialog);
                }
                //Mission complete - endCheck_AfterEndingVideos
                else if (book[0].Rows[i][general_IDCol].Text == "9.10")
                {
                    ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                    List<string> option_TC = new List<string>();
                    List<string> option_SC = new List<string>();
                    List<string> option_EN = new List<string>();
                    dialog.ByWhom = GetSingleLineString(book[0].Rows[i][general_CharacterImgCol].Text);
                    dialog.Text_TC = GetSingleLineString(book[0].Rows[i][general_TCCol].Text);
                    dialog.Text_SC = GetSingleLineString(book[0].Rows[i][general_SCCol].Text);
                    dialog.Text_EN = GetSingleLineString(book[0].Rows[i][general_ENCol].Text);
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option1_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option1_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option1_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option1_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option2_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option2_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option2_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option2_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option3_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option3_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option3_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option3_ENCol].Text));
                    }
                    if (!string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_TCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_SCCol].Text)) &&
                        !string.IsNullOrEmpty(GetSingleLineString(book[0].Rows[i][general_Option4_ENCol].Text)))
                    {
                        option_TC.Add(GetSingleLineString(book[0].Rows[i][general_Option4_TCCol].Text));
                        option_SC.Add(GetSingleLineString(book[0].Rows[i][general_Option4_SCCol].Text));
                        option_EN.Add(GetSingleLineString(book[0].Rows[i][general_Option4_ENCol].Text));
                    }
                    dialog.OptionTexts_TC = option_TC;
                    dialog.OptionTexts_SC = option_SC;
                    dialog.OptionTexts_EN = option_EN;
                    endCheck_AfterEndingVideos.Add(dialog);
                }
            }
            #endregion

            if (onSetupDoneCallback != null)
            {
                onSetupDoneCallback.Invoke();
            }
        }));


    }
    private IEnumerator LoadFileAsync(string path, DownloadHandler handler)
    {
        var url = Path.Combine(Application.streamingAssetsPath, path);
        using (var req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();
            var bytes = req.downloadHandler.data;
            handler(bytes);
        }
    }
    string GetSingleLineString(string txt)
    {
        return txt.Replace("\r", "").Replace("\n", "");
    }

    public void EndingCheck()
    {
        if (bosses[0].IsSuccessCollectDone && bosses[1].IsSuccessCollectDone && bosses[2].IsSuccessCollectDone)
        {
            currEndingCheck = EndingCheckStage.ToEndingVideo;
            GameManager.instance.dialogActive = true;
            endCheck_ChangeToEndingVideoDialogIndex++;
            DialogBoxManager.instance.ShowDialog(endCheck_ChangeToEndingVideos[endCheck_ChangeToEndingVideoDialogIndex]);
        }
        else
        {
            if (currMapId == MapID.Carboniferous)
            {
                if (bosses[0].IsSuccessCollectDone)
                {
                    currEndingCheck = EndingCheckStage.ToPermian;
                    GameManager.instance.dialogActive = true;
                    DialogBoxManager.instance.ShowDialog(endCheck_ChangeToPermian);
                }
            }
            else if (currMapId == MapID.Permian)
            {
                if (bosses[1].IsSuccessCollectDone && bosses[2].IsSuccessCollectDone)
                {
                    currEndingCheck = EndingCheckStage.ToCarboniferous;
                    GameManager.instance.dialogActive = true;
                    DialogBoxManager.instance.ShowDialog(endCheck_ChangeToCarboniferous);
                }
                else 
                {
                    currEndingCheck = EndingCheckStage.OneLeftInPermian;
                    GameManager.instance.dialogActive = true;
                    DialogBoxManager.instance.ShowDialog(endCheck_PermianOneLeft);
                }
            }
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("ResetScene");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        inputManager.onValueChanged_ConfirmCallback -= InputManager_OnValueChanged_Confirm;
    }









    //-------- tmp

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeLanguage(Language.TC);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeLanguage(Language.SC);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeLanguage(Language.EN);
        }
    }

    void TmpExcelControl()
    {
        ConfigData_Character npc_1 = new ConfigData_Character();
        npc_1.Id = "NPC_C01";
        npc_1.Name_TC = "鱗木屬";
        npc_1.IsCollectable = false;
        npc_1.IsFirstMeetDone = false;
        npc_1.IsSuccessCollectDone = false;
        ConfigData_Text info1 = new ConfigData_Text();
        info1.Text_TC = "<b>鱗木屬</b><br>屬名：<i>Lepidodendron</i><br>意思：帶鱗片的樹木<br>分類：石松綱<br>生存時代：石炭紀<br>高度：可超過 30 米<br>化石分布：世界各地<br>";
        info1.Text_SC = "<b>鳞木属</b><br>属名：<i>Lepidodendron</i><br>意思：带鳞片的树木<br>分类：石松纲<br>生存时代：石炭纪<br>高度：可超过 30 米<br>化石分布：世界各地<br>";
        info1.Text_EN = "<b><i>Lepidodendron</i></b><br>Genus: <i>Lepidodendron</i><br>Meaning: Scale tree<br>Classification: Lycopodiopsida<br>Period: Carboniferous<br>Length: can be over 30 m<br>Fossil distribution: Worldwide";
        npc_1.InfoText = info1;
        List<ConfigData_Text> features1 = new List<ConfigData_Text>();
        ConfigData_Text feature1 = new ConfigData_Text();
        feature1.Text_TC = "早期無籽維管植物，現代石松的近親";
        feature1.Text_SC = "早期无籽维管植物，现代石松的近亲";
        feature1.Text_EN = "Early seedless vascular plants closely related to club mosses of today";
        features1.Add(feature1);
        ConfigData_Text feature2 = new ConfigData_Text();
        feature2.Text_TC = "石炭紀最常見的植物之一";
        feature2.Text_SC = "石炭纪最常见的植物之一";
        feature2.Text_EN = "One of the most abundant plants of the Carboniferous";
        features1.Add(feature2);
        ConfigData_Text feature3 = new ConfigData_Text();
        feature3.Text_TC = "樹幹被葉覆蓋，葉脫落後留下鑽石型的葉痕";
        feature3.Text_SC = "树干被叶覆盖，叶脱落后留下钻石型的叶痕";
        feature3.Text_EN = "Trunk covered with leaves that leave diamond - shaped scars";
        features1.Add(feature3);
        npc_1.FeatureTexts = features1;
        ConfigData_DialogBox dialog_11 = new ConfigData_DialogBox();
        dialog_11.ByWhom = "DRO";
        dialog_11.ImagePath = "";
        dialog_11.Text_TC = "看看這個是鱗木屬呀!";
        npc_1.DialogBoxes.Add(dialog_11);
        ConfigData_DialogBox dialog_12 = new ConfigData_DialogBox();
        dialog_12.ByWhom = "DRO";
        dialog_12.ImagePath = "";
        dialog_12.Text_TC = "鱗木是一屬已滅絕的石松類，為一類高大的樹狀蕨類，生長在炎熱潮濕的沼澤地帶中。";
        npc_1.DialogBoxes.Add(dialog_12);
        NPC_Carboniferous.Add(npc_1);

        ConfigData_Character npc_2 = new ConfigData_Character();
        npc_2.Id = "NPC_C02";
        npc_2.Name_TC = "科達樹";
        npc_2.IsCollectable = false;
        npc_2.IsFirstMeetDone = false;
        npc_2.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_21 = new ConfigData_DialogBox();
        dialog_21.ByWhom = "AVA";
        dialog_21.ImagePath = "";
        dialog_21.Text_TC = "這些科達樹長得真高！";
        npc_2.DialogBoxes.Add(dialog_21);
        ConfigData_DialogBox dialog_22 = new ConfigData_DialogBox();
        dialog_22.ByWhom = "DRO";
        dialog_22.ImagePath = "";
        dialog_22.Text_TC = "科達樹可達 30 米以上，葉片窄長，呈舌頭狀。它們是裸子植物，有原始的毬果，是松柏類植物的祖先。";
        npc_2.DialogBoxes.Add(dialog_22);
        ConfigData_DialogBox dialog_23 = new ConfigData_DialogBox();
        dialog_23.ByWhom = "DRO";
        dialog_23.ImagePath = "";
        dialog_23.Text_TC = "石炭紀的氣候溫暖潮濕，非常適合植物生長。科達樹和其他植物殘骸最終形成大量的煤。";
        npc_2.DialogBoxes.Add(dialog_23);
        ConfigData_DialogBox dialog_24 = new ConfigData_DialogBox();
        dialog_24.ByWhom = "AVA";
        dialog_24.ImagePath = "";
        dialog_24.Text_TC = "嗯嗯…石炭的意思就是煤，亦是這個地質時期命名為「石炭紀」的原因。";
        npc_2.DialogBoxes.Add(dialog_24);
        NPC_Carboniferous.Add(npc_2);

        ConfigData_Character npc_3 = new ConfigData_Character();
        npc_3.Id = "NPC_C03";
        npc_3.Name_TC = "節胸屬";
        npc_3.IsCollectable = false;
        npc_3.IsFirstMeetDone = false;
        npc_3.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_31 = new ConfigData_DialogBox();
        dialog_31.ByWhom = "DRO";
        dialog_31.ImagePath = "";
        dialog_31.Text_TC = "看看這個是節胸屬呀!";
        npc_3.DialogBoxes.Add(dialog_31);
        ConfigData_DialogBox dialog_32 = new ConfigData_DialogBox();
        dialog_32.ByWhom = "DRO";
        dialog_32.ImagePath = "";
        dialog_32.Text_TC = "節胸屬，又稱節胸蜈蚣屬，是史前的倍足綱動物，即現今蜈蚣及馬陸的遠古親屬。";
        npc_3.DialogBoxes.Add(dialog_32);
        NPC_Carboniferous.Add(npc_3);

        ConfigData_Character npc_4 = new ConfigData_Character();
        npc_4.Id = "NPC_C04";
        npc_4.Name_TC = "巨脈蜻蜓";
        npc_4.IsCollectable = false;
        npc_4.IsFirstMeetDone = false;
        npc_4.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_41 = new ConfigData_DialogBox();
        dialog_41.ByWhom = "DRO";
        dialog_41.ImagePath = "";
        dialog_41.Text_TC = "看看這個是巨脈蜻蜓呀!";
        npc_4.DialogBoxes.Add(dialog_41);
        ConfigData_DialogBox dialog_42 = new ConfigData_DialogBox();
        dialog_42.ByWhom = "DRO";
        dialog_42.ImagePath = "";
        dialog_42.Text_TC = "巨脈蜻蜓，又名大尾蜻蜓或巨尾蜻蜓，是3億年前石炭紀一種已滅絕的昆蟲。";
        npc_4.DialogBoxes.Add(dialog_42);
        NPC_Carboniferous.Add(npc_4);


        ConfigData_Character npc_5 = new ConfigData_Character();
        npc_5.Id = "NPC_C05";
        npc_5.Name_TC = "芬氏彼得足螈";
        npc_5.IsCollectable = false;
        npc_5.IsFirstMeetDone = false;
        npc_5.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_51 = new ConfigData_DialogBox();
        dialog_51.ByWhom = "AVA";
        dialog_51.ImagePath = "Images/SupportImage/Lystrosaurus_SupportImg1";
        dialog_51.Text_TC = "噢，是彼得足螈！";
        npc_5.DialogBoxes.Add(dialog_51);
        ConfigData_DialogBox dialog_52 = new ConfigData_DialogBox();
        dialog_52.ByWhom = "DRO";
        dialog_52.ImagePath = "";
        dialog_52.Text_TC = "彼得足螈是水棲四足類和陸棲四足類之間的過渡物種。";
        npc_5.DialogBoxes.Add(dialog_52);
        ConfigData_DialogBox dialog_53 = new ConfigData_DialogBox();
        dialog_53.ByWhom = "DRO";
        dialog_53.ImagePath = "";
        dialog_53.Text_TC = "牠的腳趾朝向身體前方，能夠有效地行走，所以彼得足螈被認為是最早適應在陸地「行走」的脊椎動物！";
        npc_5.DialogBoxes.Add(dialog_53);
        ConfigData_DialogBox dialog_54 = new ConfigData_DialogBox();
        dialog_54.ByWhom = "AVA";
        dialog_54.ImagePath = "";
        dialog_54.Text_TC = "可是，牠只能緩步前進。";
        npc_5.DialogBoxes.Add(dialog_54);
        ConfigData_DialogBox dialog_55 = new ConfigData_DialogBox();
        dialog_55.ByWhom = "NPC_C05";
        dialog_55.ImagePath = "";
        dialog_55.Text_TC = "一二，一二......等等......";
        npc_5.DialogBoxes.Add(dialog_55);
        NPC_Carboniferous.Add(npc_5);

        ConfigData_Character npc_6 = new ConfigData_Character();
        npc_6.Id = "NPC_C06";
        npc_6.Name_TC = "引螈屬";
        npc_6.IsCollectable = false;
        npc_6.IsFirstMeetDone = false;
        npc_6.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_61 = new ConfigData_DialogBox();
        dialog_61.ByWhom = "DRO";
        dialog_61.ImagePath = "";
        dialog_61.Text_TC = "看看這個是引螈屬呀!";
        npc_6.DialogBoxes.Add(dialog_61);
        ConfigData_DialogBox dialog_62 = new ConfigData_DialogBox();
        dialog_62.ByWhom = "DRO";
        dialog_62.ImagePath = "";
        dialog_62.Text_TC = "引螈是石炭紀、二疊紀探索上最大的動物。體長1.8米以上。";
        npc_6.DialogBoxes.Add(dialog_62);
        NPC_Carboniferous.Add(npc_6);

        //---

        ConfigData_Character boss1 = new ConfigData_Character();
        boss1.Id = "M01";
        boss1.Name_TC = "林蜥";
        boss1.Name_SC = "林蜥";
        boss1.Name_EN = "<i>Hylonomus</i>";
        boss1.DescriptionTag_TC = "身長：約20厘米";
        boss1.DescriptionTag_SC = "身长：约20厘米";
        boss1.DescriptionTag_EN = "Length: About 20 cm";
        boss1.IsCollectable = true;
        boss1.IsFirstMeetDone = false;
        boss1.IsSuccessCollectDone = false;
        ConfigData_Text info_b1 = new ConfigData_Text();
        info_b1.Text_TC = "<b>林蜥</b><br>屬名：<i>Hylonomus</i> <br>意思：森林居住者<br>分類：蜥形綱<br>生存時代：石炭紀晚期<br>身長：約 20 厘米<br>食物：昆蟲<br>化石分布：加拿大";
        info_b1.Text_SC = "<b>林蜥</b><br>属名：<i>Hylonomus</i> <br>意思：森林居住者<br>分类：蜥形纲<br>生存时代：石炭纪晚期<br>身长：约 20 厘米<br>食物：昆虫<br>化石分布：加拿大";
        info_b1.Text_EN = "<b><i>Hylonomus</i></b><br>Genus: <i>Hylonomus</i><br>Meaning: Forest dweller<br>Classification: Sauropsida<br>Period: Late Carboniferous<br>Length: ~20 cm<br>Diet: Insects<br>Fossil distribution: Canada";
        boss1.InfoText = info_b1;
        List<ConfigData_Text> features_b1 = new List<ConfigData_Text>();
        ConfigData_Text feature_b11 = new ConfigData_Text();
        feature_b11.Text_TC = "外形與現代蜥蝪非常相似，有長長的尾巴和四足行走";
        feature_b11.Text_SC = "外形与现代蜥蝪非常相似，有长长的尾巴和四足行走";
        feature_b11.Text_EN = "Superficially lizard-like overall, with a long tail and walk on four legs";
        features_b1.Add(feature_b11);
        ConfigData_Text feature_b12 = new ConfigData_Text();
        feature_b12.Text_TC = "皮膚由鱗片覆蓋，牙齒尖銳和向內彎曲";
        feature_b12.Text_SC = "皮肤由鳞片覆盖，牙齿尖锐和向内弯曲";
        feature_b12.Text_EN = "Skin covered in scale, pointy teeth curved inward";
        features_b1.Add(feature_b12);
        ConfigData_Text feature_b13 = new ConfigData_Text();
        feature_b13.Text_TC = "已知最早的爬行動物";
        feature_b13.Text_SC = "已知最早的爬行动物";
        feature_b13.Text_EN = "The earliest known reptile";
        features_b1.Add(feature_b13);
        boss1.FeatureTexts = features_b1;
        ConfigData_DialogBox dialog_m11 = new ConfigData_DialogBox();
        dialog_m11.ByWhom = "AVA";
        dialog_m11.ImagePath = "";
        dialog_m11.Text_TC = "咦？是林蜥！";
        boss1.DialogBoxes.Add(dialog_m11);
        ConfigData_DialogBox dialog_m12 = new ConfigData_DialogBox();
        dialog_m12.ByWhom = "M01";
        dialog_m12.ImagePath = "";
        dialog_m12.Text_TC = "你好。";
        boss1.DialogBoxes.Add(dialog_m12);
        ConfigData_DialogBox dialog_m13 = new ConfigData_DialogBox();
        dialog_m13.ByWhom = "AVA";
        dialog_m13.ImagePath = "";
        dialog_m13.Text_TC = "林蜥是已知最早期的爬行動物，可以說是所有爬行動物的「老祖宗」！";
        boss1.DialogBoxes.Add(dialog_m13);
        ConfigData_DialogBox dialog_m131 = new ConfigData_DialogBox();
        dialog_m131.ByWhom = "AVA";
        dialog_m131.ImagePath = "";
        dialog_m131.Text_TC = "林蜥，我們想多了解你，請你介紹一下自己吧。";
        boss1.DialogBoxes.Add(dialog_m131);
        ConfigData_DialogBox dialog_m132 = new ConfigData_DialogBox();
        dialog_m132.ByWhom = "M01";
        dialog_m132.ImagePath = "Images/SupportImage/Hylonomus_SupportImg2";
        dialog_m132.Text_TC = "就如你所見，我的外形與現代蜥蝪非常相似，有長長的尾巴和以四足行走。";
        boss1.DialogBoxes.Add(dialog_m132);
        ConfigData_DialogBox dialog_m14 = new ConfigData_DialogBox();
        dialog_m14.ByWhom = "M01";
        dialog_m14.ImagePath = "";
        dialog_m14.Text_TC = "我的皮膚被鱗片覆蓋，可以減少水分流失，防止在陸地上脫水。";
        boss1.DialogBoxes.Add(dialog_m14);
        ConfigData_DialogBox dialog_m15 = new ConfigData_DialogBox();
        dialog_m15.ByWhom = "M01";
        dialog_m15.ImagePath = "Images/SupportImage/Hylonomus_SupportImg1";
        dialog_m15.Text_TC = "尖銳而向內彎的牙齒可以牢牢咬住掙扎中的獵物，例如昆蟲和節肢動物，並能夠輕易地撕碎牠們。";
        boss1.DialogBoxes.Add(dialog_m15);
        ConfigData_DialogBox dialog_m16 = new ConfigData_DialogBox();
        dialog_m16.ByWhom = "DRO";
        dialog_m16.ImagePath = "";
        dialog_m16.Text_TC = "想不到你小小一隻，竟然是恐龍的祖先！";
        boss1.DialogBoxes.Add(dialog_m16);
        ConfigData_DialogBox dialog_m17 = new ConfigData_DialogBox();
        dialog_m17.ByWhom = "DRO";
        dialog_m17.ImagePath = "";
        dialog_m17.Text_TC = "博士，是時候去下個地方了 。";
        boss1.DialogBoxes.Add(dialog_m17);
        ConfigData_DialogBox dialog_m18 = new ConfigData_DialogBox();
        dialog_m18.ByWhom = "AVA";
        dialog_m18.ImagePath = "";
        dialog_m18.Text_TC = "林蜥再見~";
        boss1.DialogBoxes.Add(dialog_m18);
        bosses.Add(boss1);

        ConfigData_Character boss2 = new ConfigData_Character();
        boss2.Id = "M02";
        boss2.Name_TC = "異齒龍";
        boss2.Name_SC = "异齿龙";
        boss2.Name_EN = "<i>Dimetrodon</i>";
        boss2.DescriptionTag_TC = "身長：約3.5米";
        boss2.DescriptionTag_SC = "身长：约3.5米";
        boss2.DescriptionTag_EN = "Length: About 3.5m";
        boss2.IsCollectable = true;
        boss2.IsFirstMeetDone = false;
        boss2.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_m21 = new ConfigData_DialogBox();
        dialog_m21.ByWhom = "M02";
        dialog_m21.ImagePath = "";
        dialog_m21.Text_TC = "你好呀！我就係異齒龍喇！";
        boss2.DialogBoxes.Add(dialog_m21);
        ConfigData_DialogBox dialog_m22 = new ConfigData_DialogBox();
        dialog_m22.ByWhom = "AVA";
        dialog_m22.ImagePath = "";
        dialog_m22.Text_TC = "你好呀！我係從未來嚟嘅科學家！大概花了半生的運氣才能遇到你，真幸運！你看起來很威猛呢。";
        boss2.DialogBoxes.Add(dialog_m22);
        ConfigData_DialogBox dialog_m23 = new ConfigData_DialogBox();
        dialog_m23.ByWhom = "M02";
        dialog_m23.ImagePath = "";
        dialog_m23.Text_TC = "我是肉食性合弓動物的一屬，生存於二疊紀。";
        boss2.DialogBoxes.Add(dialog_m23);
        ConfigData_DialogBox dialog_m231 = new ConfigData_DialogBox();
        dialog_m231.ByWhom = "M02";
        dialog_m231.ImagePath = "Images/SupportImage/Dimetrodon_SupportImg1";
        dialog_m231.Text_TC = "我的大型頭顱骨中有兩種不同型態的牙齒，切割用的牙齒與銳利的犬齒。我以往側邊攤開的四肢及大型尾巴來支撐身體。";
        boss2.DialogBoxes.Add(dialog_m231);
        ConfigData_DialogBox dialog_m232 = new ConfigData_DialogBox();
        dialog_m232.ByWhom = "M02";
        dialog_m232.ImagePath = "Images/SupportImage/Dimetrodon_SupportImg2";
        dialog_m232.Text_TC = "我最明顯的特徵是背部的高大背帆，可用來控制體溫。背帆的表面可使加熱、冷卻更有效率。";
        boss2.DialogBoxes.Add(dialog_m232);
        ConfigData_DialogBox dialog_m24 = new ConfigData_DialogBox();
        dialog_m24.ByWhom = "M02";
        dialog_m24.ImagePath = "";
        dialog_m24.Text_TC = "好似唔小心就講咗好多關於自己嘅嘢，有啲唔好意思，下次有機會再傾多啲。";
        boss2.DialogBoxes.Add(dialog_m24);
        ConfigData_DialogBox dialog_m25 = new ConfigData_DialogBox();
        dialog_m25.ByWhom = "M02";
        dialog_m25.ImagePath = "";
        dialog_m25.Text_TC = "你快啲去繼續探索吓啦，仲有好多新奇有趣嘅事物等緊你！";
        boss2.DialogBoxes.Add(dialog_m25);
        bosses.Add(boss2);

        ConfigData_Character boss3 = new ConfigData_Character();
        boss3.Id = "M03";
        boss3.Name_TC = "水龍獸";
        boss3.Name_SC = "水龙兽";
        boss3.Name_EN = "<i>Lystrosaurus</i>";
        boss3.DescriptionTag_TC = "身長：約1米";
        boss3.DescriptionTag_SC = "身长：约1米";
        boss3.DescriptionTag_EN = "Length: About 1m";
        boss3.IsCollectable = true;
        boss3.IsFirstMeetDone = false;
        boss3.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_m31 = new ConfigData_DialogBox();
        dialog_m31.ByWhom = "M03";
        dialog_m31.ImagePath = "";
        dialog_m31.Text_TC = "你好呀！我就係水龍獸喇！";
        boss3.DialogBoxes.Add(dialog_m31);
        ConfigData_DialogBox dialog_m32 = new ConfigData_DialogBox();
        dialog_m32.ByWhom = "AVA";
        dialog_m32.ImagePath = "";
        dialog_m32.Text_TC = "你好呀！我係從未來嚟嘅科學家！大概花了半生的運氣才能遇到你，真幸運！你看起來很威猛呢。";
        boss3.DialogBoxes.Add(dialog_m32);
        ConfigData_DialogBox dialog_m33 = new ConfigData_DialogBox();
        dialog_m33.ByWhom = "M03";
        dialog_m33.ImagePath = "";
        dialog_m33.Text_TC = "我是種常見的合弓動物，屬於獸孔目二齒獸下目，體型接近豬，身長約0.9米，重量約90公斤。";
        boss3.DialogBoxes.Add(dialog_m33);
        ConfigData_DialogBox dialog_m331 = new ConfigData_DialogBox();
        dialog_m331.ByWhom = "M03";
        dialog_m331.ImagePath = "Images/SupportImage/Lystrosaurus_SupportImg1";
        dialog_m331.Text_TC = "與其他獸孔目不同，我的口鼻部相當短，只有兩顆上頜的長牙。口鼻部的前端有角質喙狀嘴，用來切碎植物。";
        boss3.DialogBoxes.Add(dialog_m331);
        ConfigData_DialogBox dialog_m332 = new ConfigData_DialogBox();
        dialog_m332.ByWhom = "M03";
        dialog_m332.ImagePath = "Images/SupportImage/Lystrosaurus_SupportImg2";
        dialog_m332.Text_TC = "我的頜部關節原始，只能做出往前、往後的動作，而不能做出往上、往下的動作。";
        boss3.DialogBoxes.Add(dialog_m332);
        ConfigData_DialogBox dialog_m34 = new ConfigData_DialogBox();
        dialog_m34.ByWhom = "M03";
        dialog_m34.ImagePath = "";
        dialog_m34.Text_TC = "好似唔小心就講咗好多關於自己嘅嘢，有啲唔好意思，下次有機會再傾多啲。";
        boss3.DialogBoxes.Add(dialog_m34);
        ConfigData_DialogBox dialog_m35 = new ConfigData_DialogBox();
        dialog_m35.ByWhom = "M03";
        dialog_m35.ImagePath = "";
        dialog_m35.Text_TC = "你快啲去繼續探索吓啦，仲有好多新奇有趣嘅事物等緊你！";
        boss3.DialogBoxes.Add(dialog_m35);
        bosses.Add(boss3);

        //----

        ConfigData_Character npc_p2 = new ConfigData_Character();
        npc_p2.Id = "NPC_P01";
        npc_p2.Name_TC = "節胸屬";
        npc_p2.IsCollectable = false;
        npc_p2.IsFirstMeetDone = false;
        npc_p2.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p21 = new ConfigData_DialogBox();
        dialog_p21.ByWhom = "DRO";
        dialog_p21.ImagePath = "";
        dialog_p21.Text_TC = "看看這個是節胸屬呀!";
        npc_p2.DialogBoxes.Add(dialog_p21);
        ConfigData_DialogBox dialog_p22 = new ConfigData_DialogBox();
        dialog_p22.ByWhom = "DRO";
        dialog_p22.ImagePath = "";
        dialog_p22.Text_TC = "節胸屬在石炭紀才是全盛時期。";
        npc_p2.DialogBoxes.Add(dialog_p22);
        NPC_Permian.Add(npc_p2);

        ConfigData_Character npc_p6 = new ConfigData_Character();
        npc_p6.Id = "NPC_P02";
        npc_p6.Name_TC = "引螈屬";
        npc_p6.IsCollectable = false;
        npc_p6.IsFirstMeetDone = false;
        npc_p6.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p61 = new ConfigData_DialogBox();
        dialog_p61.ByWhom = "DRO";
        dialog_p61.ImagePath = "";
        dialog_p61.Text_TC = "看看這個是引螈屬呀!";
        npc_p6.DialogBoxes.Add(dialog_p61);
        ConfigData_DialogBox dialog_p62 = new ConfigData_DialogBox();
        dialog_p62.ByWhom = "DRO";
        dialog_p62.ImagePath = "";
        dialog_p62.Text_TC = "引螈屬在石炭紀才是全盛時期。";
        npc_p6.DialogBoxes.Add(dialog_p62);
        NPC_Permian.Add(npc_p6);

        ConfigData_Character npc_p4 = new ConfigData_Character();
        npc_p4.Id = "NPC_P03";
        npc_p4.Name_TC = "銀杏目";
        npc_p4.IsCollectable = false;
        npc_p4.IsFirstMeetDone = false;
        npc_p4.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p41 = new ConfigData_DialogBox();
        dialog_p41.ByWhom = "DRO";
        dialog_p41.ImagePath = "";
        dialog_p41.Text_TC = "看看這個是銀杏目呀!";
        npc_p4.DialogBoxes.Add(dialog_p41);
        ConfigData_DialogBox dialog_p42 = new ConfigData_DialogBox();
        dialog_p42.ByWhom = "DRO";
        dialog_p42.ImagePath = "";
        dialog_p42.Text_TC = "銀杏類植物為高大多枝落葉喬木、具有挺拔的樹幹與獨特的扇形葉片。";
        npc_p4.DialogBoxes.Add(dialog_p42);
        NPC_Permian.Add(npc_p4);

        ConfigData_Character npc_p5 = new ConfigData_Character();
        npc_p5.Id = "NPC_P04";
        npc_p5.Name_TC = "舌羊齒屬";
        npc_p5.IsCollectable = false;
        npc_p5.IsFirstMeetDone = false;
        npc_p5.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p51 = new ConfigData_DialogBox();
        dialog_p51.ByWhom = "DRO";
        dialog_p51.ImagePath = "";
        dialog_p51.Text_TC = "看看這個是舌羊齒屬呀!";
        npc_p5.DialogBoxes.Add(dialog_p51);
        ConfigData_DialogBox dialog_p52 = new ConfigData_DialogBox();
        dialog_p52.ByWhom = "DRO";
        dialog_p52.ImagePath = "";
        dialog_p52.Text_TC = "舌羊齒的典型植株高度約為8公尺，呈喬木狀。葉片的大小與寬度各異，呈羊舌狀。";
        npc_p5.DialogBoxes.Add(dialog_p52);
        NPC_Permian.Add(npc_p5);

        ConfigData_Character npc_p7 = new ConfigData_Character();
        npc_p7.Id = "NPC_P05";
        npc_p7.Name_TC = "空尾蜥屬";
        npc_p7.IsCollectable = false;
        npc_p7.IsFirstMeetDone = false;
        npc_p7.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p71 = new ConfigData_DialogBox();
        dialog_p71.ByWhom = "DRO";
        dialog_p71.ImagePath = "";
        dialog_p71.Text_TC = "看看這個是空尾蜥屬呀!";
        npc_p7.DialogBoxes.Add(dialog_p71);
        ConfigData_DialogBox dialog_p72 = new ConfigData_DialogBox();
        dialog_p72.ByWhom = "DRO";
        dialog_p72.ImagePath = "";
        dialog_p72.Text_TC = "空尾蜥屬擁有特化的類似翅膀結構，使牠可以滑翔。";
        npc_p7.DialogBoxes.Add(dialog_p72);
        NPC_Permian.Add(npc_p7);

        ConfigData_Character npc_p8 = new ConfigData_Character();
        npc_p8.Id = "NPC_P06";
        npc_p8.Name_TC = "瘤頭龍屬";
        npc_p8.IsCollectable = false;
        npc_p8.IsFirstMeetDone = false;
        npc_p8.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p81 = new ConfigData_DialogBox();
        dialog_p81.ByWhom = "DRO";
        dialog_p81.ImagePath = "";
        dialog_p81.Text_TC = "看看這個是瘤頭龍屬呀!";
        npc_p8.DialogBoxes.Add(dialog_p81);
        ConfigData_DialogBox dialog_p82 = new ConfigData_DialogBox();
        dialog_p82.ByWhom = "DRO";
        dialog_p82.ImagePath = "";
        dialog_p82.Text_TC = "瘤頭龍屬與現代牛差不多大，背部有一個多節的頭骨和骨質板甲。";
        npc_p8.DialogBoxes.Add(dialog_p82);
        NPC_Permian.Add(npc_p8);

        ConfigData_Character npc_p9 = new ConfigData_Character();
        npc_p9.Id = "NPC_P07";
        npc_p9.Name_TC = "盾甲龍屬";
        npc_p9.IsCollectable = false;
        npc_p9.IsFirstMeetDone = false;
        npc_p9.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p91 = new ConfigData_DialogBox();
        dialog_p91.ByWhom = "DRO";
        dialog_p91.ImagePath = "";
        dialog_p91.Text_TC = "看看這個是盾甲龍屬呀!";
        npc_p9.DialogBoxes.Add(dialog_p91);
        ConfigData_DialogBox dialog_p92 = new ConfigData_DialogBox();
        dialog_p92.ByWhom = "DRO";
        dialog_p92.ImagePath = "";
        dialog_p92.Text_TC = "盾甲龍屬是大型無孔類爬行動物，而且不像其他爬行動物，牠們的腿是位在他們的身體底下，以支撐牠們的重量。";
        npc_p9.DialogBoxes.Add(dialog_p92);
        NPC_Permian.Add(npc_p9);

        ConfigData_Character npc_p10 = new ConfigData_Character();
        npc_p10.Id = "NPC_P08";
        npc_p10.Name_TC = "狼蜥獸屬";
        npc_p10.IsCollectable = false;
        npc_p10.IsFirstMeetDone = false;
        npc_p10.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p101 = new ConfigData_DialogBox();
        dialog_p101.ByWhom = "DRO";
        dialog_p101.ImagePath = "";
        dialog_p101.Text_TC = "看看這個是狼蜥獸屬呀!";
        npc_p10.DialogBoxes.Add(dialog_p101);
        ConfigData_DialogBox dialog_p102 = new ConfigData_DialogBox();
        dialog_p102.ByWhom = "DRO";
        dialog_p102.ImagePath = "";
        dialog_p102.Text_TC = "狼蜥獸屬是四足動物，四肢直立於身體下方。頭顱骨長45公分，身長約3到4公尺。";
        npc_p10.DialogBoxes.Add(dialog_p102);
        NPC_Permian.Add(npc_p10);

        ConfigData_Character npc_p12 = new ConfigData_Character();
        npc_p12.Id = "NPC_P09";
        npc_p12.Name_TC = "原鞘翅目";
        npc_p12.IsCollectable = false;
        npc_p12.IsFirstMeetDone = false;
        npc_p12.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p121 = new ConfigData_DialogBox();
        dialog_p121.ByWhom = "AVA";
        dialog_p121.ImagePath = "";
        dialog_p121.Text_TC = "這些小不點是原始甲蟲。";
        npc_p12.DialogBoxes.Add(dialog_p121);
        ConfigData_DialogBox dialog_p122 = new ConfigData_DialogBox();
        dialog_p122.ByWhom = "DRO";
        dialog_p122.ImagePath = "";
        dialog_p122.Text_TC = "這些原始甲蟲屬於已滅絕的原鞘翅目，主要食物是木材。";
        npc_p12.DialogBoxes.Add(dialog_p122);
        ConfigData_DialogBox dialog_p123 = new ConfigData_DialogBox();
        dialog_p123.ByWhom = "AVA";
        dialog_p123.ImagePath = "";
        dialog_p123.Text_TC = "相比石炭紀的昆蟲，二疊紀的昆蟲較細小。";
        npc_p12.DialogBoxes.Add(dialog_p123);
        NPC_Permian.Add(npc_p12);

        ConfigData_Character npc_p13 = new ConfigData_Character();
        npc_p13.Id = "NPC_P10";
        npc_p13.Name_TC = "古網翅目";
        npc_p13.IsCollectable = false;
        npc_p13.IsFirstMeetDone = false;
        npc_p13.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p131 = new ConfigData_DialogBox();
        dialog_p131.ByWhom = "AVA";
        dialog_p131.ImagePath = "";
        dialog_p131.Text_TC = "這些是古網翅目昆蟲。牠的樣子哪裏怪怪的。";
        npc_p13.DialogBoxes.Add(dialog_p131);
        ConfigData_DialogBox dialog_p132 = new ConfigData_DialogBox();
        dialog_p132.ByWhom = "DRO";
        dialog_p132.ImagePath = "";
        dialog_p132.Text_TC = "不像現代的昆蟲有四隻翅膀，牠們有六隻翅膀。";
        npc_p13.DialogBoxes.Add(dialog_p132);
        ConfigData_DialogBox dialog_p133 = new ConfigData_DialogBox();
        dialog_p133.ByWhom = "DRO";
        dialog_p133.ImagePath = "";
        dialog_p133.Text_TC = "牠們用像鳥喙一般的口器吮吸植物的汁液。";
        npc_p13.DialogBoxes.Add(dialog_p133);
        ConfigData_DialogBox dialog_p134 = new ConfigData_DialogBox();
        dialog_p134.ByWhom = "NPC_P10";
        dialog_p134.ImagePath = "";
        dialog_p134.Text_TC = "真美味，你們也要來一口嗎？";
        npc_p13.DialogBoxes.Add(dialog_p134);
        NPC_Permian.Add(npc_p13);

        //-------

        //-------

    }
}

[Serializable]
public class ConfigData_Character
{
    //for excel
    public string Id;
    public string Name_TC;
    public string Name_SC;
    public string Name_EN;
    public string DescriptionTag_TC;
    public string DescriptionTag_SC;
    public string DescriptionTag_EN;
    public bool IsCollectable;
    public List<ConfigData_DialogBox> DialogBoxes = new List<ConfigData_DialogBox>();
    public ConfigData_Text InfoText = new ConfigData_Text();
    public List<ConfigData_Text> FeatureTexts = new List<ConfigData_Text>();

    //for program
    public bool IsFirstMeetDone;
    public bool IsSuccessCollectDone;
}

[Serializable]
public class ConfigData_DialogBox
{
    public string ByWhom;
    public string ImagePath;
    public string Text_TC;
    public string Text_SC;
    public string Text_EN;
    public List<string> OptionTexts_TC;
    public List<string> OptionTexts_SC;
    public List<string> OptionTexts_EN;
}

[Serializable]
public class ConfigData_Text
{
    public string Text_TC;
    public string Text_SC;
    public string Text_EN;
}

[Serializable]
public class ConfigData_Menu
{
    public ConfigData_Text MenuBtn;
    public ConfigData_Text CloseBtn;

    //Main
    public List<ConfigData_Text> Main_OptionTexts = new List<ConfigData_Text>();

    //Language
    public List<ConfigData_Text> Language_OptionTexts = new List<ConfigData_Text>();

    //Control
    public List<ConfigData_Text> Control_OptionTexts = new List<ConfigData_Text>();
    public ConfigData_Text Control_ArrowText1;
    public ConfigData_Text Control_ArrowText2;
    public ConfigData_Text Control_ABtnText;
    public ConfigData_Text Control_BBtnText;

    //Reset
    public ConfigData_Text Reset_Text;
    public List<ConfigData_Text> Reset_OptionTexts = new List<ConfigData_Text>();
}
