using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConversationModeManager : MonoBehaviour
{
    public static ConversationModeManager instance;

    [Header("Root")]
    public GameObject conversationModeGrp;

    [Header("bkg")]
    public Image bkg;

    [Header("Boss")]
    public Image bossImg;

    [Header("Tag")]
    public GameObject tagObj;
    public TextMeshProUGUI tag_NameText_TC;
    public TextMeshProUGUI tag_DescriptionText_TC;

    void Start()
    {
        instance = this;
    }

    public void Show(string name_TC, string tag_TC)
    {
        conversationModeGrp.SetActive(true);
        tag_NameText_TC.text = name_TC;
        tag_DescriptionText_TC.text = tag_TC;
    }

    public void Hide()
    {
        conversationModeGrp.SetActive(false);
    }
}
