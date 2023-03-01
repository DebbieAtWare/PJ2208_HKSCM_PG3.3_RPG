using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ConversationModeManager : MonoBehaviour
{
    public static ConversationModeManager instance;

    [Header("Root")]
    public CanvasGroup conversationModeCanvasGrp;

    [Header("bkg")]
    public Image bkg;
    public Image bkgTop;
    public List<Sprite> bkgSprites = new List<Sprite>();
    public List<Sprite> bkgTopSprites = new List<Sprite>();
    //48 frame, 12 fps, 4 sec. 4/48 = 0.083
    public float bkgAniTime = 0.083f;
    public int currBkgIndex;
    //16 frame, 12 fps, 
    public float bkgTopAniTime = 0.03125f;
    public int currBkgTopIndex;

    [Header("Boss")]
    public List<ConversationModeBossObject> bossObjs = new List<ConversationModeBossObject>();
    List<Vector2> bossPosTargets_Center = new List<Vector2>();
    List<Vector2> bossPosTargets_Side = new List<Vector2>();
    List<Vector2> bossPosTargets_Off = new List<Vector2>();
    List<Vector3> bossScaleTargets_Center = new List<Vector3>();
    List<Vector3> bossScaleTargets_Side = new List<Vector3>();
    List<Vector3> bossScaleTargets_Off = new List<Vector3>();

    [Header("Avatar")]
    public RectTransform avatarGrpRect;
    public ConversationModeAvatarObject avatarObj;
    public ConversationModeDroneObject droneObj;
    Vector2 avatarGrpPosTarget_On = new Vector2(0f, 0f);
    Vector2 avatarGrpPosTarget_Off = new Vector2(-570f, 0f);

    [Header("Tag")]
    public CanvasGroup tagCanvasGrp;
    public TextMeshProUGUI tag_NameText_TC;
    public TextMeshProUGUI tag_NameText_SC;
    public TextMeshProUGUI tag_NameText_EN;
    public Image tag_BadgeImg;
    public List<Sprite> tag_BadgeSprites = new List<Sprite>();

    int currBossIndex;

    CommonUtils commonUtils;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("ConversationModeManager Awake");
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
        commonUtils = CommonUtils.instance;
        commonUtils.onChangeLangCallback += CommonUtils_OnChangeLang;

        bossPosTargets_Center.Add(new Vector2(0f, 110f));
        bossPosTargets_Center.Add(new Vector2(0f, 65f));
        bossPosTargets_Center.Add(new Vector2(0f, 110f));
        bossPosTargets_Side.Add(new Vector2(446f, 204f));
        bossPosTargets_Side.Add(new Vector2(446f, 204f));
        bossPosTargets_Side.Add(new Vector2(446f, 204f));
        bossPosTargets_Off.Add(new Vector2(1000f, 110f));
        bossPosTargets_Off.Add(new Vector2(1000f, 65f));
        bossPosTargets_Off.Add(new Vector2(1000f, 110f));
        bossScaleTargets_Center.Add(new Vector3(1.1f, 1.1f, 1.1f));
        bossScaleTargets_Center.Add(new Vector3(1.1f, 1.1f, 1.1f));
        bossScaleTargets_Center.Add(new Vector3(1.1f, 1.1f, 1.1f));
        bossScaleTargets_Side.Add(new Vector3(1f, 1f, 1f));
        bossScaleTargets_Side.Add(new Vector3(1f, 1f, 1f));
        bossScaleTargets_Side.Add(new Vector3(1f, 1f, 1f));
        bossScaleTargets_Off.Add(new Vector3(1.1f, 1.1f, 1.1f));
        bossScaleTargets_Off.Add(new Vector3(1.1f, 1.1f, 1.1f));
        bossScaleTargets_Off.Add(new Vector3(1.1f, 1.1f, 1.1f));
        ChangeLanguage();
    }

    private void CommonUtils_OnChangeLang()
    {
        ChangeLanguage();
    }

    void ChangeLanguage()
    {
        if (commonUtils.currLang == Language.TC)
        {
            tag_NameText_TC.gameObject.SetActive(true);
            tag_NameText_SC.gameObject.SetActive(false);
            tag_NameText_EN.gameObject.SetActive(false);
            tag_NameText_TC.rectTransform.sizeDelta = new Vector2(tag_NameText_TC.preferredWidth, tag_NameText_TC.rectTransform.sizeDelta.y);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            tag_NameText_TC.gameObject.SetActive(false);
            tag_NameText_SC.gameObject.SetActive(true);
            tag_NameText_EN.gameObject.SetActive(false);
            tag_NameText_SC.rectTransform.sizeDelta = new Vector2(tag_NameText_SC.preferredWidth, tag_NameText_SC.rectTransform.sizeDelta.y);
        }
        if (commonUtils.currLang == Language.EN)
        {
            tag_NameText_TC.gameObject.SetActive(false);
            tag_NameText_SC.gameObject.SetActive(false);
            tag_NameText_EN.gameObject.SetActive(true);
            tag_NameText_EN.rectTransform.sizeDelta = new Vector2(tag_NameText_EN.preferredWidth, tag_NameText_EN.rectTransform.sizeDelta.y);
        }
    }

    void BkgLoopAni()
    {
        currBkgIndex++;

        if (currBkgIndex == bkgSprites.Count)
        {
            currBkgIndex = 0;
        }

        bkg.sprite = bkgSprites[currBkgIndex];
        Invoke("BkgLoopAni", bkgAniTime);
    }

    void BkgTopAni()
    {
        currBkgTopIndex++;

        if (currBkgTopIndex == bkgTopSprites.Count)
        {
            currBkgTopIndex = 0;
        }
        else
        {
            bkgTop.sprite = bkgTopSprites[currBkgTopIndex];
            Invoke("BkgTopAni", bkgTopAniTime);
        }
    }

    void BkgTopAni_Reverse()
    {
        currBkgTopIndex--;

        if (currBkgTopIndex != 0)
        {
            bkgTop.sprite = bkgTopSprites[currBkgTopIndex];
            Invoke("BkgTopAni_Reverse", bkgTopAniTime);
        }
    }


    public void Ani_Start(CharacterID id, ConfigData_Character info)
    {
        //setup
        conversationModeCanvasGrp.gameObject.SetActive(true);
        bkg.sprite = bkgSprites[0];
        bkg.DOFade(1, 0);
        BkgLoopAni();
        currBkgTopIndex = bkgTopSprites.Count - 1;
        bkgTop.sprite = bkgTopSprites[currBkgTopIndex];
        bkgTop.DOFade(1, 0);
        tag_NameText_TC.text = info.Name_TC;
        tag_NameText_SC.text = info.Name_SC;
        tag_NameText_EN.text = info.Name_EN;
        ChangeLanguage();
        tagCanvasGrp.alpha = 0;
        avatarGrpRect.anchoredPosition = avatarGrpPosTarget_Off;
        if (id == CharacterID.M01)
        {
            currBossIndex = 0;
            bossObjs[0].gameObject.SetActive(true);
            bossObjs[0].rectTrans.anchoredPosition = bossPosTargets_Off[0];
            bossObjs[0].rectTrans.localScale = bossScaleTargets_Off[0];
            bossObjs[0].canvasGrp.alpha = 0;
            tag_BadgeImg.sprite = tag_BadgeSprites[0];
            bossObjs[1].gameObject.SetActive(false);
            bossObjs[2].gameObject.SetActive(false);
        }
        else if (id == CharacterID.M02)
        {
            currBossIndex = 1;
            bossObjs[1].gameObject.SetActive(true);
            bossObjs[1].rectTrans.anchoredPosition = bossPosTargets_Off[1];
            bossObjs[1].rectTrans.localScale = bossScaleTargets_Off[1];
            bossObjs[1].canvasGrp.alpha = 0;
            tag_BadgeImg.sprite = tag_BadgeSprites[1];
            bossObjs[0].gameObject.SetActive(false);
            bossObjs[2].gameObject.SetActive(false);
        }
        else if (id == CharacterID.M03)
        {
            currBossIndex = 2;
            bossObjs[2].gameObject.SetActive(true);
            bossObjs[2].rectTrans.anchoredPosition = bossPosTargets_Off[2];
            bossObjs[2].rectTrans.localScale = bossScaleTargets_Off[2];
            bossObjs[2].canvasGrp.alpha = 0;
            tag_BadgeImg.sprite = tag_BadgeSprites[2];
            bossObjs[0].gameObject.SetActive(false);
            bossObjs[1].gameObject.SetActive(false);
        }
        //ani
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            conversationModeCanvasGrp.DOFade(1, 1f);
            yield return new WaitForSeconds(0.8f);
            BkgTopAni_Reverse();
            bossObjs[currBossIndex].rectTrans.DOAnchorPos(bossPosTargets_Center[currBossIndex], 1.5f);
            bossObjs[currBossIndex].canvasGrp.DOFade(1f, 0.5f);
        }
        
    }

    public void Ani_AvatarIn()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            BkgTopAni();
            bossObjs[currBossIndex].rectTrans.DOAnchorPos(bossPosTargets_Side[currBossIndex], 1f);
            bossObjs[currBossIndex].rectTrans.DOScale(bossScaleTargets_Side[currBossIndex], 1f);
            avatarGrpRect.DOAnchorPos(avatarGrpPosTarget_On, 1f);
            avatarObj.ChangeAni_Walk_R();
            droneObj.ChangeAni();
            tagCanvasGrp.DOFade(1, 1f);
            yield return new WaitForSeconds(1f);
            avatarObj.ChangeAni_Idle();
        }
    }

    public void Ani_AvatarOut()
    {
        avatarGrpRect.DOAnchorPos(avatarGrpPosTarget_Off, 1f);
        //avatarObj.ChangeAni_Walk_L();
    }

    public void Ani_BossCenter()
    {
        tagCanvasGrp.DOFade(0, 1f);
        bossObjs[currBossIndex].rectTrans.DOAnchorPos(bossPosTargets_Center[currBossIndex], 1f);
        bossObjs[currBossIndex].rectTrans.DOScale(bossScaleTargets_Center[currBossIndex], 1f);
    }

    public void BossAni_Idle()
    {
        bossObjs[currBossIndex].ChangeAni_Idle();
    }

    public void BossAni_Talk()
    {
        bossObjs[currBossIndex].ChangeAni_Talk();
    }

    public void HideFade(float aniTime)
    {
        conversationModeCanvasGrp.DOFade(0, aniTime).OnComplete(HideFadeOnCompleted);
    }
    void HideFadeOnCompleted()
    {
        conversationModeCanvasGrp.gameObject.SetActive(false);
        CancelInvoke("BkgLoopAni");
        CancelInvoke("BkgTopAni");
        CancelInvoke("BkgTopAni_Reverse");
        avatarObj.ResetAll();
        droneObj.ResetAll();
        for (int i = 0; i < bossObjs.Count; i++)
        {
            bossObjs[i].ResetAll();
        }
        bkg.sprite = bkgSprites[0];
        currBkgIndex = 0;
        bkgTop.sprite = bkgTopSprites[0];
        currBkgTopIndex = 0;
    }

    public void HideCut()
    {
        conversationModeCanvasGrp.alpha = 0;
        conversationModeCanvasGrp.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (commonUtils != null)
        {
            commonUtils.onChangeLangCallback -= CommonUtils_OnChangeLang;
        }
    }
}
