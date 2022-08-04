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

    void Start()
    {
        instance = this;
    }

    public void ShowDialog(string line)
    {
        dialogBoxGrp.SetActive(true);
        text_TC.text = line;
        GameManager.instance.dialogActive = true;
    }

    public void HideDialog()
    {
        dialogBoxGrp.SetActive(false);
        GameManager.instance.dialogActive = false;
    }
}
