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
    public TextMeshProUGUI text_TC;
    public CanvasGroup supportImgCanvasGrp;
    public RawImage supportImg;

    CommonUtils commonUtils;
    Vector2 supportImgSizeTarget = new Vector2(720, 480);

    void Start()
    {
        instance = this;
        commonUtils = CommonUtils.instance;
    }

    public void ShowDialog(ConfigData_DialogBox dialogBox)
    {
        dialogBoxGrp.SetActive(true);
        text_TC.text = dialogBox.Text_TC;

        if (dialogBox.ByWhom == CharacterID.AVA.ToString())
        {
            profilePic.gameObject.SetActive(true);
            profilePic.sprite = commonUtils.profilePicSprite_Avatar;
        }
        else if (dialogBox.ByWhom == CharacterID.DRO.ToString())
        {
            profilePic.gameObject.SetActive(true);
            profilePic.sprite = commonUtils.profilePicSprite_Drone;
        }
        else if (dialogBox.ByWhom == CharacterID.M01.ToString())
        {
            profilePic.gameObject.SetActive(true);
            profilePic.sprite = commonUtils.profilePicSprite_Boss01;
        }
        else if (dialogBox.ByWhom == CharacterID.M02.ToString())
        {
            profilePic.gameObject.SetActive(true);
            profilePic.sprite = commonUtils.profilePicSprite_Boss02;
        }
        else if (dialogBox.ByWhom == CharacterID.M03.ToString())
        {
            profilePic.gameObject.SetActive(true);
            profilePic.sprite = commonUtils.profilePicSprite_Boss03;
        }
        else
        {
            profilePic.gameObject.SetActive(false);
        }

        if (!String.IsNullOrEmpty(dialogBox.ImagePath) && !string.IsNullOrEmpty(dialogBox.ImagePath))
        {
            ShowSupportImg(dialogBox.ImagePath);
        }
        else
        {
            HideSupportImg();
        }
    }

    public void HideDialog()
    {
        dialogBoxGrp.SetActive(false);
    }

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
}
