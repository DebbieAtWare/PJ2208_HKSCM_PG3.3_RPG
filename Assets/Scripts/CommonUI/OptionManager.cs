using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public enum OptionStage
{
    None,
    Main,
    Language,
    Control,
    Restart
}

public class OptionManager : MonoBehaviour
{
    public static OptionManager instance;

    [Header("Root")]
    public GameObject rootObj;

    [Header("Top Btn")]
    public GameObject topBtnObj;
    public TextMeshProUGUI topBtn_OptionText_TC;
    public TextMeshProUGUI topBtn_OptionText_SC;
    public TextMeshProUGUI topBtn_OptionText_EN;

    [Header("Popup")]
    public GameObject popupObj_Root;
    public TextMeshProUGUI popupObj_CloseText_TC;
    public TextMeshProUGUI popupObj_CloseText_SC;
    public TextMeshProUGUI popupObj_CloseText_EN;

    [Header("Main Group")]
    public GameObject mainGrp_Root;
    public List<TextMeshProUGUI> mainGrp_OptionTexts_TC = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> mainGrp_OptionTexts_SC = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> mainGrp_OptionTexts_EN = new List<TextMeshProUGUI>();
    public List<GameObject> mainGrp_ArrowObjs = new List<GameObject>();
    public int mainGrp_CurrIndex = 0;

    [Header("Lang Group")]
    public GameObject langGrp_Root;
    public List<TextMeshProUGUI> langGrp_OptionTexts_TC = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> langGrp_OptionTexts_SC = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> langGrp_OptionTexts_EN = new List<TextMeshProUGUI>();
    public List<GameObject> langGrp_ArrowObjs = new List<GameObject>();
    public int langGrp_CurrIndex = 0;

    [Header("Control Group")]
    public GameObject controlGrp_Root;
    public List<TextMeshProUGUI> controlGrp_OptionTexts_TC = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> controlGrp_OptionTexts_SC = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> controlGrp_OptionTexts_EN = new List<TextMeshProUGUI>();
    public List<GameObject> controlGrp_ArrowObjs = new List<GameObject>();
    public int controlGrp_CurrIndex = 0;

    [Header("Reset Group")]
    public GameObject resetGrp_Root;
    public List<GameObject> resetGrp_ArrowObjs = new List<GameObject>();
    public int resetGrp_CurrIndex = 0;

    [Header("Language")]
    public List<GameObject> langObjs_TC;
    public List<GameObject> langObjs_SC;
    public List<GameObject> langObjs_EN;

    [Header("Curr")]
    public OptionStage currStage;
    public bool isPreviouDialogActive;

    CommonUtils commonUtils;
    InputManager inputManager;

    bool canDroneShowTalkHint = false;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("OptionManager Awake");
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
        commonUtils = CommonUtils.instance;
        commonUtils.onChangeLangCallback += CommonUtils_OnChangeLang;

        inputManager = InputManager.instance;
        inputManager.onValueChanged_OptionCallback += InputManager_OnValueChanged_Option;
        inputManager.onValueChanged_VerticalCallback += InputManager_OnValueChanged_Vertical;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;
        inputManager.canInput_Option = true;

        ChangeLanguage();

        mainGrp_Root.SetActive(true);
        langGrp_Root.SetActive(false);
        controlGrp_Root.SetActive(false);
        resetGrp_Root.SetActive(false);
        popupObj_Root.SetActive(false);
        mainGrp_CurrIndex = 0;

        SetActive(false);

        currStage = OptionStage.None;
    }

    public void SetActive(bool val)
    {
        rootObj.SetActive(val);
    }

    private void InputManager_OnValueChanged_Option()
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
        {
            if (MainManger.instance.currStage == MainStage.StartLab || MainManger.instance.currStage == MainStage.InGame || MainManger.instance.currStage == MainStage.EndLab_CollectionBookUpdate)
            {
                if (currStage == OptionStage.None)
                {
                    SoundManager.instance.Play_SFX(12);
                    //save a record when option is not yet open
                    DroneController.instance.HideTalkHint();
                    canDroneShowTalkHint = DroneController.instance.canShowTalkHint;
                    DroneController.instance.canShowTalkHint = false;
                    isPreviouDialogActive = GameManager.instance.dialogActive;
                    GameManager.instance.dialogActive = true;
                    ChangeControl_Main();
                }
                else
                {
                    SoundManager.instance.Play_Input(1);
                    Close_ResetAll();
                }
            }

        }

    }

    private void InputManager_OnValueChanged_Vertical(int val)
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
        {
            if (currStage == OptionStage.Main)
            {
                SoundManager.instance.Play_Input(0);
                if (mainGrp_CurrIndex == 0)
                {
                    if (val == -1)
                    {
                        mainGrp_ArrowObjs[mainGrp_CurrIndex].SetActive(false);
                        mainGrp_CurrIndex = 1;
                        mainGrp_ArrowObjs[mainGrp_CurrIndex].SetActive(true);
                    }
                }
                else if (mainGrp_CurrIndex == 1)
                {
                    if (val == 1)
                    {
                        mainGrp_ArrowObjs[mainGrp_CurrIndex].SetActive(false);
                        mainGrp_CurrIndex = 0;
                        mainGrp_ArrowObjs[mainGrp_CurrIndex].SetActive(true);
                    }
                    else if (val == -1)
                    {
                        mainGrp_ArrowObjs[mainGrp_CurrIndex].SetActive(false);
                        mainGrp_CurrIndex = 2;
                        mainGrp_ArrowObjs[mainGrp_CurrIndex].SetActive(true);
                    }
                }
                else if (mainGrp_CurrIndex == 2)
                {
                    if (val == 1)
                    {
                        mainGrp_ArrowObjs[mainGrp_CurrIndex].SetActive(false);
                        mainGrp_CurrIndex = 1;
                        mainGrp_ArrowObjs[mainGrp_CurrIndex].SetActive(true);
                    }
                }
            }
            else if (currStage == OptionStage.Language)
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
                    else if (val == -1)
                    {
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(false);
                        langGrp_CurrIndex = 3;
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(true);
                    }
                }
                else if (langGrp_CurrIndex == 3)
                {
                    if (val == 1)
                    {
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(false);
                        langGrp_CurrIndex = 2;
                        langGrp_ArrowObjs[langGrp_CurrIndex].SetActive(true);
                    }
                }
            }
            else if (currStage == OptionStage.Restart)
            {
                SoundManager.instance.Play_Input(0);
                if (resetGrp_CurrIndex == 0)
                {
                    if (val == -1)
                    {
                        resetGrp_ArrowObjs[resetGrp_CurrIndex].SetActive(false);
                        resetGrp_CurrIndex = 1;
                        resetGrp_ArrowObjs[resetGrp_CurrIndex].SetActive(true);
                    }
                }
                else if (resetGrp_CurrIndex == 1)
                {
                    if (val == 1)
                    {
                        resetGrp_ArrowObjs[resetGrp_CurrIndex].SetActive(false);
                        resetGrp_CurrIndex = 0;
                        resetGrp_ArrowObjs[resetGrp_CurrIndex].SetActive(true);
                    }
                }
            }
        }


    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
        {
            if (currStage == OptionStage.Main)
            {
                SoundManager.instance.Play_Input(2);
                if (mainGrp_CurrIndex == 0)
                {
                    ChangeControl_Language();
                }
                else if (mainGrp_CurrIndex == 1)
                {
                    ChangeControl_Control();
                }
                else if (mainGrp_CurrIndex == 2)
                {
                    ChangeControl_Reset();
                }
            }
            else if (currStage == OptionStage.Language)
            {
                SoundManager.instance.Play_Input(2);
                if (langGrp_CurrIndex == 0)
                {
                    ChangeControl_Main();
                }
                else if (langGrp_CurrIndex == 1)
                {
                    commonUtils.ChangeLanguage(Language.TC);
                    Close_ResetAll();
                }
                else if (langGrp_CurrIndex == 2)
                {
                    commonUtils.ChangeLanguage(Language.SC);
                    Close_ResetAll();
                }
                else if (langGrp_CurrIndex == 3)
                {
                    commonUtils.ChangeLanguage(Language.EN);
                    Close_ResetAll();
                }
            }
            else if (currStage == OptionStage.Control)
            {
                SoundManager.instance.Play_Input(2);
                if (controlGrp_CurrIndex == 0)
                {
                    ChangeControl_Main();
                }
            }
            else if (currStage == OptionStage.Restart)
            {
                SoundManager.instance.Play_Input(2);
                if (resetGrp_CurrIndex == 0)
                {
                    //TODO reset game !!!!!
                    Close_ResetAll();
                    commonUtils.ResetGame();
                }
                else if (resetGrp_CurrIndex == 1)
                {
                    Close_ResetAll();
                }
            }
        }


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

    void ChangeControl_Main()
    {
        currStage = OptionStage.Main;
        topBtnObj.SetActive(false);
        mainGrp_Root.SetActive(true);
        langGrp_Root.SetActive(false);
        langGrp_CurrIndex = 0;
        controlGrp_Root.SetActive(false);
        controlGrp_CurrIndex = 0;
        resetGrp_Root.SetActive(false);
        resetGrp_CurrIndex = 1;
        resetGrp_ArrowObjs[0].SetActive(false);
        resetGrp_ArrowObjs[1].SetActive(true);
        for (int i = 0; i < mainGrp_ArrowObjs.Count; i++)
        {
            if (i == mainGrp_CurrIndex)
            {
                mainGrp_ArrowObjs[i].SetActive(true);
            }
            else
            {
                mainGrp_ArrowObjs[i].SetActive(false);
            }
        }
        popupObj_Root.SetActive(true);
    }

    void ChangeControl_Language()
    {
        currStage = OptionStage.Language;
        topBtnObj.SetActive(false);
        mainGrp_Root.SetActive(false);
        langGrp_Root.SetActive(true);
        controlGrp_Root.SetActive(false);
        resetGrp_Root.SetActive(false);
        for (int i = 0; i < langGrp_ArrowObjs.Count; i++)
        {
            if (i == langGrp_CurrIndex)
            {
                langGrp_ArrowObjs[i].SetActive(true);
            }
            else
            {
                langGrp_ArrowObjs[i].SetActive(false);
            }
        }
    }

    void ChangeControl_Control()
    {
        currStage = OptionStage.Control;
        topBtnObj.SetActive(false);
        mainGrp_Root.SetActive(false);
        langGrp_Root.SetActive(false);
        controlGrp_Root.SetActive(true);
        resetGrp_Root.SetActive(false);
        for (int i = 0; i < langGrp_ArrowObjs.Count; i++)
        {
            if (i == langGrp_CurrIndex)
            {
                langGrp_ArrowObjs[i].SetActive(true);
            }
            else
            {
                langGrp_ArrowObjs[i].SetActive(false);
            }
        }
    }

    void ChangeControl_Reset()
    {
        currStage = OptionStage.Restart;
        topBtnObj.SetActive(false);
        mainGrp_Root.SetActive(false);
        langGrp_Root.SetActive(false);
        controlGrp_Root.SetActive(false);
        resetGrp_Root.SetActive(true);
        for (int i = 0; i < resetGrp_ArrowObjs.Count; i++)
        {
            if (i == resetGrp_CurrIndex)
            {
                resetGrp_ArrowObjs[i].SetActive(true);
            }
            else
            {
                resetGrp_ArrowObjs[i].SetActive(false);
            }
        }
    }

    void Close_ResetAll()
    {
        //close and wait to reset currStage to prevent press A to trigger other
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            topBtnObj.SetActive(true);
            popupObj_Root.SetActive(false);
            mainGrp_CurrIndex = 0;
            langGrp_CurrIndex = 0;
            controlGrp_CurrIndex = 0;
            yield return new WaitForSeconds(0.2f);
            currStage = OptionStage.None;
            //revert the setting before the option open
            //eg. when in greeting msg after change map, drone is at side of player
            if (canDroneShowTalkHint)
            {
                DroneController.instance.ShowTalkHint();
                DroneController.instance.canShowTalkHint = true;
            }
            else
            {
                DroneController.instance.HideTalkHint();
                DroneController.instance.canShowTalkHint = false;
            }
            if (MainManger.instance.currStage == MainStage.InGame)
            {
                GameManager.instance.dialogActive = isPreviouDialogActive;
            }
        }

    }

    private void OnDestroy()
    {
        commonUtils.onChangeLangCallback -= CommonUtils_OnChangeLang;

        inputManager.onValueChanged_OptionCallback -= InputManager_OnValueChanged_Option;
        inputManager.onValueChanged_VerticalCallback -= InputManager_OnValueChanged_Vertical;
        inputManager.onValueChanged_ConfirmCallback -= InputManager_OnValueChanged_Confirm;
    }
}
