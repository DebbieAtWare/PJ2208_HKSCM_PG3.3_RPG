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
    public List<RectTransform> bossRects = new List<RectTransform>();
    public List<CanvasGroup> bossCanvasGrps = new List<CanvasGroup>();
    List<Vector2> bossPosTargets_Center = new List<Vector2>();
    List<Vector2> bossPosTargets_Side = new List<Vector2>();
    List<Vector2> bossPosTargets_Off = new List<Vector2>();
    List<Vector3> bossScaleTargets_Center = new List<Vector3>();
    List<Vector3> bossScaleTargets_Side = new List<Vector3>();
    List<Vector3> bossScaleTargets_Off = new List<Vector3>();

    [Header("Avatar")]
    public RectTransform avatarGrpRect;
    Vector2 avatarGrpPosTarget_On = new Vector2(0f, 0f);
    Vector2 avatarGrpPosTarget_Off = new Vector2(-570f, 0f);

    [Header("Tag")]
    public CanvasGroup tagCanvasGrp;
    public TextMeshProUGUI tag_NameText_TC;
    public TextMeshProUGUI tag_DescriptionText_TC;
    public Image tag_BadgeImg;
    public List<Sprite> tag_BadgeSprites = new List<Sprite>();

    int currBossIndex;

    void Start()
    {
        instance = this;
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


    public void Ani_Start(CharacterID id, string name_TC, string tag_TC)
    {
        //setup
        conversationModeCanvasGrp.gameObject.SetActive(true);
        bkg.sprite = bkgSprites[0];
        bkg.DOFade(1, 0);
        BkgLoopAni();
        currBkgTopIndex = bkgTopSprites.Count - 1;
        bkgTop.sprite = bkgTopSprites[currBkgTopIndex];
        bkgTop.DOFade(1, 0);
        tag_NameText_TC.text = name_TC;
        tag_DescriptionText_TC.text = tag_TC;
        tagCanvasGrp.alpha = 0;
        avatarGrpRect.anchoredPosition = avatarGrpPosTarget_Off;
        if (id == CharacterID.M01)
        {
            currBossIndex = 0;
            bossRects[0].gameObject.SetActive(true);
            bossRects[0].anchoredPosition = bossPosTargets_Off[0];
            bossRects[0].localScale = bossScaleTargets_Off[0];
            bossCanvasGrps[0].alpha = 0;
            tag_BadgeImg.sprite = tag_BadgeSprites[0];
            bossRects[1].gameObject.SetActive(false);
            bossRects[2].gameObject.SetActive(false);
        }
        else if (id == CharacterID.M02)
        {
            currBossIndex = 1;
            bossRects[1].gameObject.SetActive(true);
            bossRects[1].anchoredPosition = bossPosTargets_Off[1];
            bossRects[1].localScale = bossScaleTargets_Off[1];
            bossCanvasGrps[1].alpha = 0;
            tag_BadgeImg.sprite = tag_BadgeSprites[1];
            bossRects[0].gameObject.SetActive(false);
            bossRects[2].gameObject.SetActive(false);
        }
        else if (id == CharacterID.M03)
        {
            currBossIndex = 2;
            bossRects[2].gameObject.SetActive(true);
            bossRects[2].anchoredPosition = bossPosTargets_Off[2];
            bossRects[2].localScale = bossScaleTargets_Off[2];
            bossCanvasGrps[2].alpha = 0;
            tag_BadgeImg.sprite = tag_BadgeSprites[2];
            bossRects[0].gameObject.SetActive(false);
            bossRects[1].gameObject.SetActive(false);
        }
        //ani
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            conversationModeCanvasGrp.DOFade(1, 1f);
            yield return new WaitForSeconds(0.8f);
            BkgTopAni_Reverse();
            bossRects[currBossIndex].DOAnchorPos(bossPosTargets_Center[currBossIndex], 1.5f);
            bossCanvasGrps[currBossIndex].DOFade(1f, 0.5f);
        }
        
    }

    public void Ani_AvatarIn()
    {
        BkgTopAni();
        bossRects[currBossIndex].DOAnchorPos(bossPosTargets_Side[currBossIndex], 1f);
        bossRects[currBossIndex].DOScale(bossScaleTargets_Side[currBossIndex], 1f);
        avatarGrpRect.DOAnchorPos(avatarGrpPosTarget_On, 1f);
        tagCanvasGrp.DOFade(1, 1f);
    }

    public void Ani_AvatarOut()
    {
        avatarGrpRect.DOAnchorPos(avatarGrpPosTarget_Off, 1f);
    }

    public void Ani_BossCenter()
    {
        tagCanvasGrp.DOFade(0, 1f);
        bossRects[currBossIndex].DOAnchorPos(bossPosTargets_Center[currBossIndex], 1f);
        bossRects[currBossIndex].DOScale(bossScaleTargets_Center[currBossIndex], 1f);
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
}
