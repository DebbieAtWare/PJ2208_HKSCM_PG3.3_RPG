using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;

public class StatusBarManager : MonoBehaviour
{
    public static StatusBarManager instance;

    [Header("Carbon")]
    public CanvasGroup canvasGrp_Carbon;
    public List<Image> lockImgs_CarbonGrp = new List<Image>();
    public List<Image> badgeImgs_CarbonGrp = new List<Image>();
    public RectTransform rectL_CarbonGrp;
    public HorizontalLayoutGroup horiLayoutL_CarbonGrp;
    public RectTransform rectR_CarbonGrp;


    [Header("Permian")]
    public CanvasGroup canvasGrp_Permian;
    public List<Image> lockImgs_PermianGrp = new List<Image>();
    public List<Image> badgeImgs_PermianGrp = new List<Image>();
    public RectTransform rectL_PermianGrp;
    public RectTransform rectR_PermianGrp;

    [Header("Lang")]
    public List<GameObject> langObjs_TC = new List<GameObject>();
    public List<GameObject> langObjs_SC = new List<GameObject>();
    public List<GameObject> langObjs_EN = new List<GameObject>();

    CommonUtils commonUtils;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("StatusBarManager Awake");
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

        for (int i = 0; i < lockImgs_CarbonGrp.Count; i++)
        {
            lockImgs_CarbonGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
            lockImgs_CarbonGrp[i].rectTransform.eulerAngles = new Vector3(0, 0, 0);
            lockImgs_CarbonGrp[i].gameObject.SetActive(true);
            badgeImgs_CarbonGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
            badgeImgs_CarbonGrp[i].rectTransform.eulerAngles = new Vector3(0, 90, 0);
            badgeImgs_CarbonGrp[i].gameObject.SetActive(false);
            lockImgs_PermianGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
            lockImgs_PermianGrp[i].rectTransform.eulerAngles = new Vector3(0, 0, 0);
            lockImgs_PermianGrp[i].gameObject.SetActive(true);
            badgeImgs_PermianGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
            badgeImgs_PermianGrp[i].rectTransform.eulerAngles = new Vector3(0, 90, 0);
            badgeImgs_PermianGrp[i].gameObject.SetActive(false);
        }

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
            for (int i = 0; i < langObjs_TC.Count; i++)
            {
                langObjs_TC[i].SetActive(true);
            }
            for (int i = 0; i < langObjs_SC.Count; i++)
            {
                langObjs_SC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_EN.Count; i++)
            {
                langObjs_EN[i].SetActive(false);
            }
            RebuildLayout();
        }
        else if (commonUtils.currLang == Language.SC)
        {
            for (int i = 0; i < langObjs_TC.Count; i++)
            {
                langObjs_TC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_SC.Count; i++)
            {
                langObjs_SC[i].SetActive(true);
            }
            for (int i = 0; i < langObjs_EN.Count; i++)
            {
                langObjs_EN[i].SetActive(false);
            }
            RebuildLayout();
        }
        else if (commonUtils.currLang == Language.EN)
        {
            for (int i = 0; i < langObjs_TC.Count; i++)
            {
                langObjs_TC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_SC.Count; i++)
            {
                langObjs_SC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_EN.Count; i++)
            {
                langObjs_EN[i].SetActive(true);
            }
            RebuildLayout();
        }
    }

    void RebuildLayout()
    {
        //each call twice
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectL_CarbonGrp);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectL_CarbonGrp);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectR_CarbonGrp);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectR_CarbonGrp);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectL_PermianGrp);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectL_PermianGrp);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectR_PermianGrp);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectR_PermianGrp);
        rectR_CarbonGrp.anchoredPosition = new Vector2(rectL_CarbonGrp.sizeDelta.x, 0);
        rectL_PermianGrp.anchoredPosition = new Vector2(rectL_PermianGrp.sizeDelta.x, 0);
        rectR_PermianGrp.anchoredPosition = new Vector2(rectL_PermianGrp.sizeDelta.x + rectR_PermianGrp.sizeDelta.x, 0);
    }

    public void Show_Carbon(float aniTime)
    {
        for (int i = 0; i < commonUtils.bosses.Count; i++)
        {
            if (commonUtils.bosses[i].IsSuccessCollectDone)
            {
                lockImgs_CarbonGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
                lockImgs_CarbonGrp[i].rectTransform.eulerAngles = new Vector3(0, 0, 0);
                lockImgs_CarbonGrp[i].gameObject.SetActive(false);
                badgeImgs_CarbonGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
                badgeImgs_CarbonGrp[i].rectTransform.eulerAngles = new Vector3(0, 0, 0);
                badgeImgs_CarbonGrp[i].gameObject.SetActive(true);
            }
            else
            {
                lockImgs_CarbonGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
                lockImgs_CarbonGrp[i].rectTransform.eulerAngles = new Vector3(0, 0, 0);
                lockImgs_CarbonGrp[i].gameObject.SetActive(true);
                badgeImgs_CarbonGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
                badgeImgs_CarbonGrp[i].rectTransform.eulerAngles = new Vector3(0, 90, 0);
                badgeImgs_CarbonGrp[i].gameObject.SetActive(false);
            }
        }
        canvasGrp_Carbon.DOFade(1, aniTime);
    }

    public void Hide_Carbon(float aniTime)
    {
        canvasGrp_Carbon.DOFade(0, aniTime);
    }
    
    public void BadgeAni_Carbon(float waitTime)
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            yield return new WaitForSeconds(waitTime);
            lockImgs_CarbonGrp[0].rectTransform.DOScale(new Vector3(2, 2, 2), 0.5f);
            yield return new WaitForSeconds(0.5f);
            SoundManager.instance.Play_SFX(0);
            lockImgs_CarbonGrp[0].rectTransform.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
            yield return new WaitForSeconds(0.2f);
            lockImgs_CarbonGrp[0].gameObject.SetActive(false);
            badgeImgs_CarbonGrp[0].gameObject.SetActive(true);
            badgeImgs_CarbonGrp[0].rectTransform.localScale = new Vector3(2, 2, 2);
            badgeImgs_CarbonGrp[0].rectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            yield return new WaitForSeconds(1f);
            badgeImgs_CarbonGrp[0].rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f);
        }
    }


    public void Show_Permian(float aniTime)
    {
        for (int i = 0; i < commonUtils.bosses.Count; i++)
        {
            if (commonUtils.bosses[i].IsSuccessCollectDone)
            {
                lockImgs_PermianGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
                lockImgs_PermianGrp[i].rectTransform.eulerAngles = new Vector3(0, 0, 0);
                lockImgs_PermianGrp[i].gameObject.SetActive(false);
                badgeImgs_PermianGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
                badgeImgs_PermianGrp[i].rectTransform.eulerAngles = new Vector3(0, 0, 0);
                badgeImgs_PermianGrp[i].gameObject.SetActive(true);
            }
            else
            {
                lockImgs_PermianGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
                lockImgs_PermianGrp[i].rectTransform.eulerAngles = new Vector3(0, 0, 0);
                lockImgs_PermianGrp[i].gameObject.SetActive(true);
                badgeImgs_PermianGrp[i].rectTransform.localScale = new Vector3(1, 1, 1);
                badgeImgs_PermianGrp[i].rectTransform.eulerAngles = new Vector3(0, 90, 0);
                badgeImgs_PermianGrp[i].gameObject.SetActive(false);
            }
        }
        canvasGrp_Permian.DOFade(1, aniTime);
    }

    public void Hide_Permian(float aniTime)
    {
        canvasGrp_Permian.DOFade(0, aniTime);
    }

    public void BadgeAni_Permian1(float waitTime)
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            yield return new WaitForSeconds(waitTime);
            lockImgs_PermianGrp[1].rectTransform.DOScale(new Vector3(2, 2, 2), 0.5f);
            yield return new WaitForSeconds(0.5f);
            SoundManager.instance.Play_SFX(0);
            lockImgs_PermianGrp[1].rectTransform.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
            yield return new WaitForSeconds(0.2f);
            lockImgs_PermianGrp[1].gameObject.SetActive(false);
            badgeImgs_PermianGrp[1].gameObject.SetActive(true);
            badgeImgs_PermianGrp[1].rectTransform.localScale = new Vector3(2, 2, 2);
            badgeImgs_PermianGrp[1].rectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            yield return new WaitForSeconds(1f);
            badgeImgs_PermianGrp[1].rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f);
        }
    }

    public void BadgeAni_Permian2(float waitTime)
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            yield return new WaitForSeconds(waitTime);
            lockImgs_PermianGrp[2].rectTransform.DOScale(new Vector3(2, 2, 2), 0.5f);
            yield return new WaitForSeconds(0.5f);
            SoundManager.instance.Play_SFX(0);
            lockImgs_PermianGrp[2].rectTransform.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
            yield return new WaitForSeconds(0.2f);
            lockImgs_PermianGrp[2].gameObject.SetActive(false);
            badgeImgs_PermianGrp[2].gameObject.SetActive(true);
            badgeImgs_PermianGrp[2].rectTransform.localScale = new Vector3(2, 2, 2);
            badgeImgs_PermianGrp[2].rectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            yield return new WaitForSeconds(1f);
            badgeImgs_PermianGrp[2].rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f);
        }
    }

    private void OnDestroy()
    {
        if (commonUtils != null)
        {
            commonUtils.onChangeLangCallback -= CommonUtils_OnChangeLang;
        }
    }
}
