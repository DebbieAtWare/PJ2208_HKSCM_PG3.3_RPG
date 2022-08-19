using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConversationModeManager : MonoBehaviour
{
    public static ConversationModeManager instance;

    [Header("Root")]
    public CanvasGroup conversationModeCanvasGrp;

    [Header("bkg")]
    public Image bkg1;
    public Image bkg2;

    [Header("Boss")]
    public Image bossImg;
    public AspectRatioFitter bossAspectFitter;
    float bossWidthTarget = 885;
    Vector2 bossPosTarget_Center = new Vector2(25, 100);
    Vector3 bossScaleTarget_Center = new Vector3(1, 1, 1);
    Vector2 bossPosTarget_Side = new Vector2(429, 138);
    Vector3 bossScaleTarget_Side = new Vector3(0.84f, 0.84f, 0.84f);

    [Header("Avatar")]
    public Image avatarImg;
    Vector2 avatarPosTarget_Off = new Vector2(-1126, -120);
    Vector2 avatarPosTarget_On = new Vector2(-635, -120);

    [Header("Tag")]
    public CanvasGroup tagCanvasGrp;
    public TextMeshProUGUI tag_NameText_TC;
    public TextMeshProUGUI tag_DescriptionText_TC;

    void Start()
    {
        instance = this;
    }

    public void Show1(string name_TC, string tag_TC, Sprite sprite)
    {
        conversationModeCanvasGrp.gameObject.SetActive(true);
        bkg1.DOFade(1, 0);
        bkg2.DOFade(1, 0);
        tag_NameText_TC.text = name_TC;
        tag_DescriptionText_TC.text = tag_TC;
        tagCanvasGrp.alpha = 0;
        bossImg.sprite = sprite;
        //bossImg.rectTransform.sizeDelta = new Vector2(bossWidthTarget, ((bossImg.sprite.rect.height * bossWidthTarget) / bossImg.sprite.rect.width));
        bossImg.rectTransform.anchoredPosition = bossPosTarget_Center;
        bossImg.rectTransform.localScale = bossScaleTarget_Center;
        avatarImg.rectTransform.anchoredPosition = avatarPosTarget_Off;
        conversationModeCanvasGrp.DOFade(1, 0.5f);
    }

    public void Show2()
    {
        bkg2.DOFade(0, 0.5f);
        bossImg.rectTransform.DOAnchorPos(bossPosTarget_Side, 0.5f);
        bossImg.rectTransform.DOScale(bossScaleTarget_Side, 0.5f);
        avatarImg.rectTransform.DOAnchorPos(avatarPosTarget_On, 0.5f);
        tagCanvasGrp.DOFade(1, 0.5f);
    }

    public void Show3()
    {
        avatarImg.rectTransform.DOAnchorPos(avatarPosTarget_Off, 0.5f);
    }

    public void HideFade(float aniTime)
    {
        conversationModeCanvasGrp.DOFade(0, aniTime).OnComplete(() => conversationModeCanvasGrp.gameObject.SetActive(false));
    }

    public void HideCut()
    {
        conversationModeCanvasGrp.alpha = 0;
        conversationModeCanvasGrp.gameObject.SetActive(false);
    }
}
