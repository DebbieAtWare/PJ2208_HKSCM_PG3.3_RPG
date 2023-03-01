using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class DialogBoxManager : MonoBehaviour
{
    public static DialogBoxManager instance;

    [Header("UI")]
    public GameObject dialogBoxGrp;
    public Image profilePic;
    public CanvasGroup supportImgCanvasGrp;
    public RawImage supportImg;
    public ConfirmButtonControl confirmBtnControl;

    [Header("NormapGrp")]
    public GameObject normalGrpObj; 
    public TextMeshProUGUI text_TC;
    public TextMeshProUGUI text_SC;
    public TextMeshProUGUI text_EN;
    public TextMeshProUGUI refText_TC;
    public TextMeshProUGUI refText_SC;
    public TextMeshProUGUI refText_EN;
    public RectTransform optionGrpRect;
    public List<TextMeshProUGUI> optionTexts_TC;
    public List<TextMeshProUGUI> optionTexts_SC;
    public List<TextMeshProUGUI> optionTexts_EN;
    public List<GameObject> arrowObjs;
    public DialogWriter.DialogWriterSingle dialogWriterSingle;
    Vector2 optionGrpPosTarget_OneLine = new Vector2(-12.85f, -20f);
    Vector2 optionGrpPosTarget_TwoLine = new Vector2(-12.85f, -57f);

    [Header("ControlGrp")]
    public GameObject controlGrpObj;

    [Header("ZoomImg")]
    public CanvasGroup zoomImgCanvasGrp;
    public GameObject zoomObj_NPC_P09;
    public GameObject zoomObj_NPC_P10;

    [Header("Lang")]
    public List<GameObject> langObjs_TC = new List<GameObject>();
    public List<GameObject> langObjs_SC = new List<GameObject>();
    public List<GameObject> langObjs_EN = new List<GameObject>();

    public delegate void OnDialogEnd();
    public OnDialogEnd onDialogEndCallback;

    CommonUtils commonUtils;
    Vector2 supportImgSizeTarget = new Vector2(720, 480);

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("DialogBoxManager Awake");
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
        instance = this;
        
        commonUtils = CommonUtils.instance;
        commonUtils.onChangeLangCallback += CommonUtils_OnChangeLang;

        for (int i = 0; i < optionTexts_TC.Count; i++)
        {
            optionTexts_TC[i].gameObject.SetActive(false);
            optionTexts_SC[i].gameObject.SetActive(false);
            optionTexts_EN[i].gameObject.SetActive(false);
            arrowObjs[i].SetActive(false);
        }
        normalGrpObj.SetActive(false);
        controlGrpObj.SetActive(false);
        dialogBoxGrp.SetActive(false);

        ChangeLanguage();
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
            if (text_TC.preferredHeight < 80)
            {
                optionGrpRect.anchoredPosition = optionGrpPosTarget_OneLine;
            }
            else
            {
                optionGrpRect.anchoredPosition = optionGrpPosTarget_TwoLine;
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
            if (text_SC.preferredHeight < 80)
            {
                optionGrpRect.anchoredPosition = optionGrpPosTarget_OneLine;
            }
            else
            {
                optionGrpRect.anchoredPosition = optionGrpPosTarget_TwoLine;
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
            if (text_EN.preferredHeight < 80)
            {
                optionGrpRect.anchoredPosition = optionGrpPosTarget_OneLine;
            }
            else
            {
                optionGrpRect.anchoredPosition = optionGrpPosTarget_TwoLine;
            }
        }
    }

    public void FinishCurrentDialog()
    {
        dialogWriterSingle.WriteAllAndDestroy();
    }

    public void ShowDialog(ConfigData_DialogBox dialogBox)
    {
        dialogBoxGrp.SetActive(true);
        normalGrpObj.SetActive(true);
        controlGrpObj.SetActive(false);

        //directly show the confirm btn ani
        confirmBtnControl.SetAlpha(1, 0);

        if (!string.IsNullOrEmpty(dialogBox.ByWhom) && !string.IsNullOrWhiteSpace(dialogBox.ByWhom))
        {
            if (dialogBox.ByWhom == CharacterID.AVA.ToString())
            {
                SoundManager.instance.Play_Dialog(0);
            }
            else if (dialogBox.ByWhom == CharacterID.DRO.ToString())
            {
                SoundManager.instance.Play_Dialog_Drone();
            }
            else if (dialogBox.ByWhom == CharacterID.M01.ToString() || dialogBox.ByWhom == CharacterID.M02.ToString() || dialogBox.ByWhom == CharacterID.M03.ToString())
            {
                SoundManager.instance.Play_Dialog(2);
            }
            else
            {
                SoundManager.instance.Play_Dialog(3);
            }
        }

        if (commonUtils.currLang == Language.TC)
        {
            text_TC.text = "";
            dialogWriterSingle = DialogWriter.AddWriter_Static(text_TC, dialogBox.Text_TC, commonUtils.data.DialogBox_TimePerCharacter_TC, true, OnDialogLineEnd);
            text_SC.text = dialogBox.Text_SC;
            text_EN.text = dialogBox.Text_EN;
        }
        else if (commonUtils.currLang == Language.SC)
        {
            text_TC.text = dialogBox.Text_TC;
            text_SC.text = "";
            dialogWriterSingle = DialogWriter.AddWriter_Static(text_SC, dialogBox.Text_SC, commonUtils.data.DialogBox_TimePerCharacter_SC, true, OnDialogLineEnd);
            text_EN.text = dialogBox.Text_EN;
        }
        else if (commonUtils.currLang == Language.EN)
        {
            text_TC.text = dialogBox.Text_TC;
            text_SC.text = dialogBox.Text_SC;
            text_EN.text = "";
            dialogWriterSingle = DialogWriter.AddWriter_Static(text_EN, dialogBox.Text_EN, commonUtils.data.DialogBox_TimePerCharacter_EN, true, OnDialogLineEnd);
        }

        //option
        for (int i = 0; i < optionTexts_TC.Count; i++)
        {
            optionTexts_TC[i].text = "";
            optionTexts_SC[i].text = "";
            optionTexts_EN[i].text = "";
            optionTexts_TC[i].gameObject.SetActive(false);
            optionTexts_SC[i].gameObject.SetActive(false);
            optionTexts_EN[i].gameObject.SetActive(false);
            arrowObjs[i].SetActive(false);
        }

        //profile pic
        if (string.IsNullOrEmpty(dialogBox.ByWhom) || string.IsNullOrWhiteSpace(dialogBox.ByWhom))
        {
            profilePic.gameObject.SetActive(false);
        }
        else
        {
            if (dialogBox.ByWhom == CharacterID.AVA.ToString())
            {
                profilePic.gameObject.SetActive(true);
                profilePic.sprite = PlayerController.instance.dialogBoxProfileSprite;
            }
            else if (dialogBox.ByWhom == CharacterID.DRO.ToString())
            {
                profilePic.gameObject.SetActive(true);
                profilePic.sprite = DroneController.instance.dialogBoxProfileSprite;
            }
            else if (dialogBox.ByWhom == CharacterID.M01.ToString())
            {
                profilePic.gameObject.SetActive(true);
                profilePic.sprite = CarboniferousManager.instance.bossObj.dialogBoxProfileSprite;
            }
            else if (dialogBox.ByWhom == CharacterID.M02.ToString())
            {
                profilePic.gameObject.SetActive(true);
                profilePic.sprite = PermianManager.instance.bossObj2.dialogBoxProfileSprite;
            }
            else if (dialogBox.ByWhom == CharacterID.M03.ToString())
            {
                profilePic.gameObject.SetActive(true);
                profilePic.sprite = PermianManager.instance.bossObj3.dialogBoxProfileSprite;
            }
            else
            {
                if (commonUtils.currMapId == MapID.Carboniferous)
                {
                    for (int i = 0; i < CarboniferousManager.instance.NPCObjs.Count; i++)
                    {
                        if (CarboniferousManager.instance.NPCObjs[i].id.ToString() == dialogBox.ByWhom &&
                            CarboniferousManager.instance.NPCObjs[i].dialogBoxProfileSprite != null)
                        {
                            profilePic.gameObject.SetActive(true);
                            profilePic.sprite = CarboniferousManager.instance.NPCObjs[i].dialogBoxProfileSprite;
                        }
                    }
                }
                else if (commonUtils.currMapId == MapID.Permian)
                {
                    for (int i = 0; i < PermianManager.instance.NPCObjs.Count; i++)
                    {
                        if (PermianManager.instance.NPCObjs[i].id.ToString() == dialogBox.ByWhom &&
                            PermianManager.instance.NPCObjs[i].dialogBoxProfileSprite != null)
                        {
                            profilePic.gameObject.SetActive(true);
                            profilePic.sprite = PermianManager.instance.NPCObjs[i].dialogBoxProfileSprite;
                        }
                    }
                }
            }
        }

        //support img
        if (!String.IsNullOrEmpty(dialogBox.ImagePath) && !string.IsNullOrEmpty(dialogBox.ImagePath))
        {
            ShowSupportImg(dialogBox.ImageTexture);
        }
        else
        {
            HideSupportImg();
        }
    }

    void OnDialogLineEnd()
    {
        if (onDialogEndCallback != null)
        {
            onDialogEndCallback.Invoke();
        }
        SoundManager.instance.FadeOutStop_Dialog(0.3f);
        SoundManager.instance.FadeOutStop_Dialog_Drone(0.3f);
    }

    public void ShowOption(ConfigData_DialogBox dialogBox)
    {
        if (dialogBox.OptionTexts_TC != null && dialogBox.OptionTexts_TC.Count != 0)
        {
            if (commonUtils.currLang == Language.TC)
            {
                refText_TC.text = dialogBox.Text_TC;
                if (refText_TC.preferredHeight < 80)
                {
                    optionGrpRect.anchoredPosition = optionGrpPosTarget_OneLine;
                }
                else
                {
                    optionGrpRect.anchoredPosition = optionGrpPosTarget_TwoLine;
                }
            }
            else if (commonUtils.currLang == Language.SC)
            {
                refText_SC.text = dialogBox.Text_SC;
                if (refText_SC.preferredHeight < 80)
                {
                    optionGrpRect.anchoredPosition = optionGrpPosTarget_OneLine;
                }
                else
                {
                    optionGrpRect.anchoredPosition = optionGrpPosTarget_TwoLine;
                }
            }
            else if (commonUtils.currLang == Language.EN)
            {
                refText_EN.text = dialogBox.Text_EN;
                if (refText_EN.preferredHeight < 80)
                {
                    optionGrpRect.anchoredPosition = optionGrpPosTarget_OneLine;
                }
                else
                {
                    optionGrpRect.anchoredPosition = optionGrpPosTarget_TwoLine;
                }
            }
            for (int i = 0; i < optionTexts_TC.Count; i++)
            {
                if (i < dialogBox.OptionTexts_TC.Count)
                {
                    optionTexts_TC[i].text = dialogBox.OptionTexts_TC[i];
                    if (dialogBox.OptionTexts_SC != null && dialogBox.OptionTexts_SC.Count != 0)
                    {
                        optionTexts_SC[i].text = dialogBox.OptionTexts_SC[i];
                    }
                    if (dialogBox.OptionTexts_EN != null && dialogBox.OptionTexts_EN.Count != 0)
                    {
                        optionTexts_EN[i].text = dialogBox.OptionTexts_EN[i];
                    }
                    if (commonUtils.currLang == Language.TC)
                    {
                        optionTexts_TC[i].gameObject.SetActive(true);
                        optionTexts_SC[i].gameObject.SetActive(false);
                        optionTexts_EN[i].gameObject.SetActive(false);
                    }
                    else if (commonUtils.currLang == Language.SC)
                    {
                        optionTexts_TC[i].gameObject.SetActive(false);
                        optionTexts_SC[i].gameObject.SetActive(true);
                        optionTexts_EN[i].gameObject.SetActive(false);
                    }
                    else if (commonUtils.currLang == Language.EN)
                    {
                        optionTexts_TC[i].gameObject.SetActive(false);
                        optionTexts_SC[i].gameObject.SetActive(false);
                        optionTexts_EN[i].gameObject.SetActive(true);
                    }
                }
                else
                {
                    optionTexts_TC[i].text = "";
                    optionTexts_SC[i].text = "";
                    optionTexts_EN[i].text = "";
                    optionTexts_TC[i].gameObject.SetActive(false);
                    optionTexts_SC[i].gameObject.SetActive(false);
                    optionTexts_EN[i].gameObject.SetActive(false);
                }

                if (i == 0)
                {
                    arrowObjs[i].SetActive(true);
                }
                else
                {
                    arrowObjs[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < optionTexts_TC.Count; i++)
            {
                optionTexts_TC[i].text = "";
                optionTexts_SC[i].text = "";
                optionTexts_EN[i].text = "";
                optionTexts_TC[i].gameObject.SetActive(false);
                optionTexts_SC[i].gameObject.SetActive(false);
                optionTexts_EN[i].gameObject.SetActive(false);
                arrowObjs[i].SetActive(false);
            }
        }
    }

    public void HideDialog()
    {
        dialogBoxGrp.SetActive(false);
        confirmBtnControl.SetAlpha(0, 0);
    }

    //------

    public void ShowControl()
    {
        profilePic.gameObject.SetActive(true);
        profilePic.sprite = DroneController.instance.dialogBoxProfileSprite;
        dialogBoxGrp.SetActive(true);
        normalGrpObj.SetActive(false);
        controlGrpObj.SetActive(true);
        confirmBtnControl.SetAlpha(1, 0);
    }

    public void HideControl()
    {
        controlGrpObj.SetActive(false);
        confirmBtnControl.SetAlpha(0, 0);
    }

    //------


    public void SetOptionArrow(int val)
    {
        for (int i = 0; i < arrowObjs.Count; i++)
        {
            if (i == val)
            {
                arrowObjs[i].SetActive(true);
            }
            else
            {
                arrowObjs[i].SetActive(false);
            }
        }
    }

    //------

    public void ShowSupportImg(Texture2D texture)
    {
        supportImgCanvasGrp.gameObject.SetActive(true);
        //tmp only load from resouces folder
        //Texture2D texture = Resources.Load<Texture2D>(imgPath);
        supportImg.texture = texture;
        if (((float)texture.width / texture.height) >= (float)(supportImgSizeTarget.x / supportImgSizeTarget.y))
        {
            supportImg.rectTransform.sizeDelta = new Vector2(supportImgSizeTarget.x, (supportImgSizeTarget.x * texture.height) / texture.width);
        }
        else
        {
            supportImg.rectTransform.sizeDelta = new Vector2((supportImgSizeTarget.y * texture.width) / texture.height, supportImgSizeTarget.y);
        }
        supportImgCanvasGrp.DOFade(1, 0f);
    }

    public void HideSupportImg()
    {
        supportImgCanvasGrp.DOFade(0, 0f).OnComplete(() => supportImgCanvasGrp.gameObject.SetActive(false));
    }

    //------

    public void ShowZoomImg(CharacterID id, float aniTime)
    {
        zoomImgCanvasGrp.gameObject.SetActive(true);
        if (id == CharacterID.NPC_P09)
        {
            zoomObj_NPC_P09.SetActive(true);
            zoomObj_NPC_P10.SetActive(false);
        }
        else if (id == CharacterID.NPC_P10)
        {
            zoomObj_NPC_P09.SetActive(false);
            zoomObj_NPC_P10.SetActive(true);
        }
        zoomImgCanvasGrp.DOFade(1, aniTime);
    }

    public void HideZoomImg(float aniTime)
    {
        zoomImgCanvasGrp.DOFade(0, aniTime).OnComplete(() => zoomImgCanvasGrp.gameObject.SetActive(false));
    }

    private void OnDestroy()
    {
        if (commonUtils != null)
        {
            commonUtils.onChangeLangCallback -= CommonUtils_OnChangeLang;
        }
    }
}
