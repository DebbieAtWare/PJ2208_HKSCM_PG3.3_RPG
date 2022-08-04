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

    void Start()
    {
        instance = this;
    }

    public void ShowDialog(string line)
    {
        dialogBoxGrp.SetActive(true);
        text_TC.text = line;
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
