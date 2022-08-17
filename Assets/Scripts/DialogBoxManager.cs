using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxManager : MonoBehaviour
{
    public static DialogBoxManager instance;

    [Header("UI")]
    public GameObject dialogBoxGrp;
    public Image profilePic;
    public TextMeshProUGUI text_TC;
    public GameObject supportImgGrp;
    public Image supportImg;

    CommonUtils commonUtils;

    void Start()
    {
        instance = this;
        commonUtils = CommonUtils.instance;
    }

    public void ShowDialog(string line, string byWhom)
    {
        dialogBoxGrp.SetActive(true);
        text_TC.text = line;

        if (byWhom == CharacterID.AVA.ToString())
        {
            profilePic.gameObject.SetActive(true);
            profilePic.sprite = commonUtils.profilePicSprite_Avatar;
        }
        else if (byWhom == CharacterID.DRO.ToString())
        {
            profilePic.gameObject.SetActive(true);
            profilePic.sprite = commonUtils.profilePicSprite_Drone;
        }
        else if (byWhom == CharacterID.M01.ToString())
        {
            profilePic.gameObject.SetActive(true);
            profilePic.sprite = commonUtils.profilePicSprite_Boss01;
        }
        else if (byWhom == CharacterID.M02.ToString())
        {
            profilePic.gameObject.SetActive(true);
            profilePic.sprite = commonUtils.profilePicSprite_Boss02;
        }
        else if (byWhom == CharacterID.M03.ToString())
        {
            profilePic.gameObject.SetActive(true);
            profilePic.sprite = commonUtils.profilePicSprite_Boss03;
        }
        else
        {
            profilePic.gameObject.SetActive(false);
        }
    }

    public void HideDialog()
    {
        dialogBoxGrp.SetActive(false);
    }

    public void ShowSupportImg(Image img)
    {
        supportImg = img;
        supportImgGrp.SetActive(true);
    }

    public void HideSupportImg()
    {
        supportImgGrp.SetActive(false);
    }
}
