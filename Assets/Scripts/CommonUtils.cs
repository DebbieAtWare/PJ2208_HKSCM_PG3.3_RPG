using FlexFramework.Excel;
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

    //-----
    [Header("ConfigData - Boss card")]
    public List<ConfigData_BossCard> bossCards = new List<ConfigData_BossCard>();

    [Header("Json")]
    public ConfigData data;

    [Header("Curr")]
    public MapID currMapId;
    public Language currLang;
    public EndingCheckStage currEndingCheck;
    public bool isAtHomePage;
    public bool isSetupExcelDone = false;
    public bool isSetupJsonDone = false;

    //for CMS excel
    public delegate void DownloadHandler(byte[] bytes);

    //local config json
    string localJsonPath = Application.streamingAssetsPath + "/Config.json";
    string jsonString;
    

    InputManager inputManager;
    EndVideoManager endVideoManager;

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

        LoadCMSExcel();
        LoadLocalJson();
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
        Debug.Log("Change Lang  " + currLang.ToString() + "  to  " + lang.ToString());
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


            #region Boss and NPC
            int conversation_IDCol = 0;
            int conversation_NameTCCol = 1;
            int conversation_NameSCCol = 2;
            int conversation_NameENCol = 3;
            int conversation_DescriptionTCCol = 4;
            int conversation_DescriptionSCCol = 5;
            int conversation_DescriptionENCol = 6;
            int conversation_Dialog_ByWhomFirstCol = 7;
            int conversation_Dialog_TCFirstCol = 8;
            int conversation_Dialog_SCFirstCol = 9;
            int conversation_Dialog_ENFirstCol = 10;
            int conversation_Dialog_ImgFirstCol = 11;
            int conversation_Dialog_Increasement = 5;
            int conversation_Dialog_Max = 11;
            //conversation
            for (int i = 0; i < 50; i++)
            {
                if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "M01")
                {
                    ConfigData_Character boss = new ConfigData_Character();
                    boss.Id = "M01";
                    boss.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    boss.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    boss.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    boss.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    boss.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    boss.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    boss.IsCollectable = true;
                    boss.IsFirstMeetDone = false;
                    boss.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            boss.DialogBoxes.Add(dialog);
                        }
                    }
                    bosses.Add(boss);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "M02")
                {
                    ConfigData_Character boss = new ConfigData_Character();
                    boss.Id = "M02";
                    boss.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    boss.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    boss.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    boss.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    boss.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    boss.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    boss.IsCollectable = true;
                    boss.IsFirstMeetDone = false;
                    boss.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            boss.DialogBoxes.Add(dialog);
                        }
                    }
                    bosses.Add(boss);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "M03")
                {
                    ConfigData_Character boss = new ConfigData_Character();
                    boss.Id = "M03";
                    boss.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    boss.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    boss.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    boss.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    boss.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    boss.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    boss.IsCollectable = true;
                    boss.IsFirstMeetDone = false;
                    boss.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            boss.DialogBoxes.Add(dialog);
                        }
                    }
                    bosses.Add(boss);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_C01")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_C01";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Carboniferous.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_C02")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_C02";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Carboniferous.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_C03")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_C03";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Carboniferous.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_C04")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_C04";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Carboniferous.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_C05")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_C05";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Carboniferous.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_C06")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_C06";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Carboniferous.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_P01")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_P01";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Permian.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_P02")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_P02";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Permian.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_P03")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_P03";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Permian.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_P04")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_P04";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Permian.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_P05")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_P05";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Permian.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_P06")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_P06";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Permian.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_P07")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_P07";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Permian.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_P08")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_P08";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Permian.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_P09")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_P09";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Permian.Add(npc);
                }
                else if (GetSingleLineString(book[2].Rows[i][conversation_IDCol].Text) == "NPC_P10")
                {
                    ConfigData_Character npc = new ConfigData_Character();
                    npc.Id = "NPC_P10";
                    npc.Name_TC = GetSingleLineString(book[2].Rows[i][conversation_NameTCCol].Text);
                    npc.Name_SC = GetSingleLineString(book[2].Rows[i][conversation_NameSCCol].Text);
                    npc.Name_EN = GetSingleLineString(book[2].Rows[i][conversation_NameENCol].Text);
                    npc.DescriptionTag_TC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionTCCol].Text);
                    npc.DescriptionTag_SC = GetSingleLineString(book[2].Rows[i][conversation_DescriptionSCCol].Text);
                    npc.DescriptionTag_EN = GetSingleLineString(book[2].Rows[i][conversation_DescriptionENCol].Text);
                    npc.IsCollectable = false;
                    npc.IsFirstMeetDone = false;
                    npc.IsSuccessCollectDone = false;
                    for (int j = 0; j < conversation_Dialog_Max; j++)
                    {
                        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text)))
                        {
                            dialog.ByWhom = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ByWhomFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.ImagePath = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ImgFirstCol + conversation_Dialog_Increasement * j].Text);
                            if (!string.IsNullOrEmpty(dialog.ImagePath))
                            {
                                dialog.ImageTexture = GetImageTexture(dialog.ImagePath);
                            }
                            dialog.Text_TC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_TCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_SC = GetSingleLineString(book[2].Rows[i][conversation_Dialog_SCFirstCol + conversation_Dialog_Increasement * j].Text);
                            dialog.Text_EN = GetSingleLineString(book[2].Rows[i][conversation_Dialog_ENFirstCol + conversation_Dialog_Increasement * j].Text);
                            npc.DialogBoxes.Add(dialog);
                        }
                    }
                    NPC_Permian.Add(npc);
                }
            }
            int collection_IDCol = 0;
            int collection_InfoTCCol = 3;
            int collection_InfoSCCol = 4;
            int collection_InfoENCol = 5;
            //collection book
            for (int i = 0; i < 100; i++)
            {
                if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == bosses[0].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            bosses[0].FeatureTexts.Add(feature);
                        }
                    }
                    bosses[0].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == bosses[1].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            bosses[1].FeatureTexts.Add(feature);
                        }
                    }
                    bosses[1].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == bosses[2].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            bosses[2].FeatureTexts.Add(feature);
                        }
                    }
                    bosses[2].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Carboniferous[0].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Carboniferous[0].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Carboniferous[0].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Carboniferous[1].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Carboniferous[1].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Carboniferous[1].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Carboniferous[2].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Carboniferous[2].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Carboniferous[2].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Carboniferous[3].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Carboniferous[3].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Carboniferous[3].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Carboniferous[4].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Carboniferous[4].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Carboniferous[4].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Carboniferous[5].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Carboniferous[5].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Carboniferous[5].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Permian[0].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Permian[0].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Permian[0].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Permian[1].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Permian[1].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Permian[1].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Permian[2].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Permian[2].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Permian[2].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Permian[3].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Permian[3].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Permian[3].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Permian[4].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Permian[4].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Permian[4].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Permian[5].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Permian[5].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Permian[5].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Permian[6].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Permian[6].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Permian[6].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Permian[7].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Permian[7].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Permian[7].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Permian[8].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Permian[8].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Permian[8].InfoText = info;
                }
                else if (GetSingleLineString(book[1].Rows[i][collection_IDCol].Text) == NPC_Permian[9].Id)
                {
                    ConfigData_Text info = new ConfigData_Text();
                    info.Text_TC = GetSingleLineString(book[1].Rows[i][collection_InfoTCCol].Text);
                    info.Text_SC = GetSingleLineString(book[1].Rows[i][collection_InfoSCCol].Text);
                    info.Text_EN = GetSingleLineString(book[1].Rows[i][collection_InfoENCol].Text);
                    for (int j = 0; j < 4; j++)
                    {
                        if (!string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text)) &&
                            !string.IsNullOrEmpty(GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text)))
                        {
                            ConfigData_Text feature = new ConfigData_Text();
                            feature.Text_TC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoTCCol].Text);
                            feature.Text_SC = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoSCCol].Text);
                            feature.Text_EN = GetSingleLineString(book[1].Rows[i + 1 + j][collection_InfoENCol].Text);
                            NPC_Permian[9].FeatureTexts.Add(feature);
                        }
                    }
                    NPC_Permian[9].InfoText = info;
                }
            }

            #endregion


            #region Boss card
            int bossCard_IDCol = 2;
            int bossCard_InfoTCCol = 5;
            int bossCard_InfoSCCol = 6;
            int bossCard_InfoENCol = 7;
            //Debug.Log(book[3].Rows.Count);
            for (int i = 0; i < book[3].Rows.Count; i++)
            {
                //Debug.Log(i + "   " + book[3].Rows[i].Count);
                if (book[3].Rows[i].Count > bossCard_InfoENCol)
                {
                    if (GetSingleLineString(book[3].Rows[i][bossCard_IDCol].Text) == "M01" ||
                        GetSingleLineString(book[3].Rows[i][bossCard_IDCol].Text) == "M02" ||
                        GetSingleLineString(book[3].Rows[i][bossCard_IDCol].Text) == "M03")
                    {
                        ConfigData_BossCard boss = new ConfigData_BossCard();
                        ConfigData_Text text1 = new ConfigData_Text();
                        text1.Text_TC = GetSingleLineString(book[3].Rows[i][bossCard_InfoTCCol].Text);
                        text1.Text_SC = GetSingleLineString(book[3].Rows[i][bossCard_InfoSCCol].Text);
                        text1.Text_EN = GetSingleLineString(book[3].Rows[i][bossCard_InfoENCol].Text);
                        boss.Title = text1;
                        ConfigData_Text text2 = new ConfigData_Text();
                        text2.Text_TC = GetSingleLineString(book[3].Rows[i + 1][bossCard_InfoTCCol].Text);
                        text2.Text_SC = GetSingleLineString(book[3].Rows[i + 1][bossCard_InfoSCCol].Text);
                        text2.Text_EN = GetSingleLineString(book[3].Rows[i + 1][bossCard_InfoENCol].Text);
                        boss.Name = text2;
                        ConfigData_Text text3 = new ConfigData_Text();
                        text3.Text_TC = GetSingleLineString(book[3].Rows[i + 2][bossCard_InfoTCCol].Text);
                        text3.Text_SC = GetSingleLineString(book[3].Rows[i + 2][bossCard_InfoSCCol].Text);
                        text3.Text_EN = GetSingleLineString(book[3].Rows[i + 2][bossCard_InfoENCol].Text);
                        boss.Row1 = text3;
                        ConfigData_Text text4 = new ConfigData_Text();
                        text4.Text_TC = GetSingleLineString(book[3].Rows[i + 3][bossCard_InfoTCCol].Text);
                        text4.Text_SC = GetSingleLineString(book[3].Rows[i + 3][bossCard_InfoSCCol].Text);
                        text4.Text_EN = GetSingleLineString(book[3].Rows[i + 3][bossCard_InfoENCol].Text);
                        boss.Row2 = text4;
                        ConfigData_Text text5 = new ConfigData_Text();
                        text5.Text_TC = GetSingleLineString(book[3].Rows[i + 4][bossCard_InfoTCCol].Text);
                        text5.Text_SC = GetSingleLineString(book[3].Rows[i + 4][bossCard_InfoSCCol].Text);
                        text5.Text_EN = GetSingleLineString(book[3].Rows[i + 4][bossCard_InfoENCol].Text);
                        boss.Row3 = text5;
                        bossCards.Add(boss);
                    }
                }
            }
            #endregion

            isSetupExcelDone = true;

            if (isSetupExcelDone && isSetupJsonDone)
            {
                if (onSetupDoneCallback != null)
                {
                    onSetupDoneCallback.Invoke();
                }
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
    Texture2D GetImageTexture(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath + "/SupportImage/" + fileName);
        Texture2D tex = new Texture2D(2, 2);
        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            tex.LoadImage(fileData);
            //tex.filterMode = FilterMode.Point;
            tex.wrapMode = TextureWrapMode.Clamp;
            return tex;
        }
        else
        {
            Debug.Log(path + " not exits");
            return tex;
        }
    }


    //-----

    void LoadLocalJson()
    {
        if (File.Exists(localJsonPath))
        {
            jsonString = File.ReadAllText(localJsonPath);
            data = JsonUtility.FromJson<ConfigData>(jsonString);
            if (data != null)
            {
                isSetupJsonDone = true;
                if (isSetupExcelDone && isSetupJsonDone)
                {
                    if (onSetupDoneCallback != null)
                    {
                        onSetupDoneCallback.Invoke();
                    }
                }
            }
        }
        else
        {
            Debug.Log("Config.json not exists");
        }
    }

    //-----

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
        //SceneManager.LoadScene("ResetScene");
        TransitionManager.instance.ResetGame();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (inputManager != null)
        {
            inputManager.onValueChanged_ConfirmCallback -= InputManager_OnValueChanged_Confirm;
        }
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

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            bosses[0].IsSuccessCollectDone = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            bosses[1].IsSuccessCollectDone = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            bosses[2].IsSuccessCollectDone = true;
        }
    }
}

[Serializable]
public class ConfigData
{
    public float DialogBox_TimePerCharacter_TC;
    public float DialogBox_TimePerCharacter_SC;
    public float DialogBox_TimePerCharacter_EN;
    public float TimeOut_DisplayUITimerTarget;
    public float TimeOut_CountdownUITimerTarget;
    public float IntroVideo_AutoRun_Page3To4;
    public float IntroVideo_AutoRun_Page4To5;
    public float IntroVideo_AutoRun_Page5To6;
    public float IntroVideo_AutoRun_Page6To7;
    public float IntroVideo_AutoRun_Page7To8;
    public float IntroVideo_AutoRun_Page8To9;
    public float IntroVideo_AutoRun_Page9To10;
    public float EndingVideo_AutoRun_Page1To2;
    public float EndingVideo_AutoRun_Page2To3;
    public float EndingVideo_AutoRun_Page3To4;
    public float EndingVideo_AutoRun_Page4To5;
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
    //for excel
    public string ByWhom;
    public string ImagePath;
    public string Text_TC;
    public string Text_SC;
    public string Text_EN;
    public List<string> OptionTexts_TC;
    public List<string> OptionTexts_SC;
    public List<string> OptionTexts_EN;
    //for program
    public Texture2D ImageTexture;
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

[Serializable]
public class ConfigData_BossCard
{
    public ConfigData_Text Title;
    public ConfigData_Text Name;
    public ConfigData_Text Row1;
    public ConfigData_Text Row2;
    public ConfigData_Text Row3;
}
