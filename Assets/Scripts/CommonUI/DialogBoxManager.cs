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
    
    [Header("Text")]
    public TextMeshProUGUI text_TC;
    public List<TextMeshProUGUI> optionTexts_TC;
    public List<GameObject> arrowObjs;
    public DialogWriter.DialogWriterSingle dialogWriterSingle;

    [Header("ZoomImg")]
    public CanvasGroup zoomImgCanvasGrp;
    public GameObject zoomObj_NPC_P11;
    public GameObject zoomObj_NPC_P12;

    CommonUtils commonUtils;
    Vector2 supportImgSizeTarget = new Vector2(720, 480);

    void Start()
    {
        instance = this;
        commonUtils = CommonUtils.instance;
        for (int i = 0; i < optionTexts_TC.Count; i++)
        {
            optionTexts_TC[i].gameObject.SetActive(false);
            arrowObjs[i].SetActive(false);
        }
    }

    public void FinishCurrentDialog()
    {
        dialogWriterSingle.WriteAllAndDestroy();
    }

    public void ShowDialog(ConfigData_DialogBox dialogBox)
    {
        dialogBoxGrp.SetActive(true);

        if (!string.IsNullOrEmpty(dialogBox.ByWhom) && !string.IsNullOrWhiteSpace(dialogBox.ByWhom))
        {
            if (dialogBox.ByWhom == CharacterID.AVA.ToString())
            {
                SoundManager.instance.Play_Dialog(0);
            }
            else if (dialogBox.ByWhom == CharacterID.DRO.ToString())
            {
                SoundManager.instance.Play_Dialog(1);
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
            
        dialogWriterSingle = DialogWriter.AddWriter_Static(text_TC, dialogBox.Text_TC, 0.05f, true, OnDialogLineEnd);

        if (dialogBox.OptionTexts_TC != null && dialogBox.OptionTexts_TC.Count != 0)
        {
            for (int i = 0; i < optionTexts_TC.Count; i++)
            {
                if (i < dialogBox.OptionTexts_TC.Count)
                {
                    optionTexts_TC[i].gameObject.SetActive(true);
                    optionTexts_TC[i].text = dialogBox.OptionTexts_TC[i];
                }
                else
                {
                    optionTexts_TC[i].text = "";
                    optionTexts_TC[i].gameObject.SetActive(false);
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
                optionTexts_TC[i].gameObject.SetActive(false);
                arrowObjs[i].SetActive(false);
            }
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
            ShowSupportImg(dialogBox.ImagePath);
        }
        else
        {
            HideSupportImg();
        }
    }

    void OnDialogLineEnd()
    {
        SoundManager.instance.FadeOutStop_Dialog(0.3f);
    }

    public void HideDialog()
    {
        dialogBoxGrp.SetActive(false);
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

    public void ShowSupportImg(string imgPath)
    {
        supportImgCanvasGrp.gameObject.SetActive(true);
        //tmp only load from resouces folder
        Texture2D texture = Resources.Load<Texture2D>(imgPath);
        supportImg.texture = texture;
        if (((float)texture.width / texture.height) >= (float)(supportImgSizeTarget.x / supportImgSizeTarget.y))
        {
            supportImg.rectTransform.sizeDelta = new Vector2(supportImgSizeTarget.x, (supportImgSizeTarget.x * texture.height) / texture.width);
        }
        else
        {
            supportImg.rectTransform.sizeDelta = new Vector2((supportImgSizeTarget.y * texture.width) / texture.height, supportImgSizeTarget.y);
        }
        supportImgCanvasGrp.DOFade(1, 0.5f);
    }

    public void HideSupportImg()
    {
        supportImgCanvasGrp.DOFade(0, 0.5f).OnComplete(() => supportImgCanvasGrp.gameObject.SetActive(false));
    }

    //------

    public void ShowZoomImg(CharacterID id, float aniTime)
    {
        zoomImgCanvasGrp.gameObject.SetActive(true);
        if (id == CharacterID.NPC_P11)
        {
            zoomObj_NPC_P11.SetActive(true);
            zoomObj_NPC_P12.SetActive(false);
        }
        else if (id == CharacterID.NPC_P12)
        {
            zoomObj_NPC_P11.SetActive(false);
            zoomObj_NPC_P12.SetActive(true);
        }
        zoomImgCanvasGrp.DOFade(1, aniTime);
    }

    public void HideZoomImg(float aniTime)
    {
        zoomImgCanvasGrp.DOFade(0, aniTime).OnComplete(() => zoomImgCanvasGrp.gameObject.SetActive(false));
    }
}
