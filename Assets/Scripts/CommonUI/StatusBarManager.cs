using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class StatusBarManager : MonoBehaviour
{
    public static StatusBarManager instance;

    [Header("Carbon")]
    public CanvasGroup canvasGrp_Carbon;
    public List<Image> lockImgs_CarbonGrp = new List<Image>();
    public List<Image> badgeImgs_CarbonGrp = new List<Image>();

    [Header("Permian")]
    public CanvasGroup canvasGrp_Permian;
    public List<Image> lockImgs_PermianGrp = new List<Image>();
    public List<Image> badgeImgs_PermianGrp = new List<Image>();

    CommonUtils commonUtils;

    void Awake()
    {
        Debug.Log("StatusBarManager Awake");
        if (instance != null)
        {
            Debug.Log("More than one instance of StatusBarManager");
            return;
        }
        instance = this;
    }

    public void Setup()
    {
        commonUtils = CommonUtils.instance;

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

}
