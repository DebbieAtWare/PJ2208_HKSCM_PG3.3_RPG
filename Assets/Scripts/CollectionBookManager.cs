using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionBookManager : MonoBehaviour
{
    public static CollectionBookManager instance;

    [Header("SuccessCollectGroup")]
    public GameObject successCollectGrp;
    public Image successCollect_Img;
    public TextMeshProUGUI successCollect_Text_TC;

    void Start()
    {
        instance = this;
    }

    public void ShowSuccessCollect(string name_TC)
    {
        successCollectGrp.SetActive(true);
        successCollect_Text_TC.text = "成功收集" + name_TC + "！";
    }

    public void HideSuccessCollect()
    {
        successCollectGrp.SetActive(false);
    }
}
