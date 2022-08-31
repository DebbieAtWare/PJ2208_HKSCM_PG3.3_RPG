using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CollectionBookManager : MonoBehaviour
{
    public static CollectionBookManager instance;

    [Header("SuccessCollectGroup")]
    public CanvasGroup successCollect_CanvasGrp;
    public Image successCollect_Img;
    public TextMeshProUGUI successCollect_Text_TC;

    CommonUtils commonUtils;

    void Start()
    {
        instance = this;
        commonUtils = CommonUtils.instance;
    }

    public void ShowSuccessCollect(string name_TC, Sprite sprite)
    {
        successCollect_CanvasGrp.gameObject.SetActive(true);
        successCollect_CanvasGrp.DOFade(1, 0.5f);
        successCollect_Text_TC.text = "成功收集" + name_TC + "！";
        successCollect_Img.sprite = sprite;
    }

    public void HideSuccessCollect(float aniTime)
    {
        successCollect_CanvasGrp.DOFade(0, aniTime).OnComplete(() => successCollect_CanvasGrp.gameObject.SetActive(false));
    }
}
