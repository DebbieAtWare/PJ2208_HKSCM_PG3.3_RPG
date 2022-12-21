using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIExtensions;
using DG.Tweening;

public class CollectionBookBossObject : MonoBehaviour
{
    [Header("Main")]
    public CharacterID id;
    public RectTransform rect;
    Vector3 originalScale;

    [Header("Card")]
    public Image img_Boss;
    public UIEffect uiEffect_Boss;
    public UIShadow uiShadow_Boss;
    public Image img_Lock;
    public UIEffect uiEffect_Lock;
    public UIShadow uiShadow_Lock;
    public CanvasGroup img_Gray;

    [Header("Text")]
    public TextMeshProUGUI titleText_TC;
    public TextMeshProUGUI titleText_SC;
    public TextMeshProUGUI titleText_EN;
    public TextMeshProUGUI nameText_TC;
    public TextMeshProUGUI nameText_SC;
    public TextMeshProUGUI nameText_EN;
    public TextMeshProUGUI rowText_TC;
    public TextMeshProUGUI rowText_SC;
    public TextMeshProUGUI rowText_EN;

    [Header("Frame")]
    public GameObject frameObj_Idle;
    public GameObject frameObj_Selected;

    [Header("Lang")]
    public List<GameObject> langObjs_TC = new List<GameObject>();
    public List<GameObject> langObjs_SC = new List<GameObject>();
    public List<GameObject> langObjs_EN = new List<GameObject>();

    public void Setup(ConfigData_BossCard bossCard, Language lang)
    {
        titleText_TC.text = bossCard.Title.Text_TC;
        titleText_SC.text = bossCard.Title.Text_SC;
        titleText_EN.text = bossCard.Title.Text_EN;
        nameText_TC.text = bossCard.Name.Text_TC;
        nameText_SC.text = bossCard.Name.Text_SC;
        nameText_EN.text = bossCard.Name.Text_EN;
        rowText_TC.text = bossCard.Row1.Text_TC + "<br>" + bossCard.Row2.Text_TC + "<br>" + bossCard.Row3.Text_TC;
        rowText_SC.text = bossCard.Row1.Text_SC + "<br>" + bossCard.Row2.Text_SC + "<br>" + bossCard.Row3.Text_SC;
        rowText_EN.text = bossCard.Row1.Text_EN + "<br>" + bossCard.Row2.Text_EN + "<br>" + bossCard.Row3.Text_EN;
        originalScale = rect.localScale;
        rect.localScale = originalScale;
        img_Lock.rectTransform.eulerAngles = new Vector3(0, 0, 0);
        img_Lock.color = new Color(1, 1, 1, 1);
        uiShadow_Lock.effectColor = new Color(1, 1, 1, 0);
        uiShadow_Lock.enabled = false;
        uiEffect_Lock.enabled = false;
        img_Lock.gameObject.SetActive(true);
        img_Boss.rectTransform.eulerAngles = new Vector3(0, 90, 0);
        img_Boss.color = new Color(1, 1, 1, 1);
        uiShadow_Boss.effectColor = new Color(1, 1, 1, 0);
        uiShadow_Boss.enabled = false;
        uiEffect_Boss.enabled = false;
        img_Boss.gameObject.SetActive(false);
        img_Gray.DOFade(0, 0);
        ChangeLanguage(lang);
    }

    public void ChangeLanguage(Language lang)
    {
        if (lang == Language.TC)
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
        }
        else if (lang == Language.SC)
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
        }
        else if (lang == Language.EN)
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
        }
    }

    public void UnlockAni()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            img_Gray.DOFade(0, 0);
            rect.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.5f);
            uiEffect_Lock.enabled = true;
            uiShadow_Lock.enabled = true;
            DOTween.To(() => uiShadow_Lock.effectColor, x => uiShadow_Lock.effectColor = x, new Color(1, 1, 1, 0.5f), 1f);
            yield return new WaitForSeconds(0.8f);
            img_Lock.rectTransform.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
            yield return new WaitForSeconds(0.2f);
            img_Lock.gameObject.SetActive(false);
            img_Boss.gameObject.SetActive(true);
            uiEffect_Boss.enabled = true;
            uiShadow_Boss.enabled = true;
            uiShadow_Boss.effectColor = new Color(1, 1, 1, 0.5f);
            img_Boss.rectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            yield return new WaitForSeconds(0.8f);
            rect.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        }
    }

    public void GrayOutAni()
    {
        img_Gray.DOFade(1, 0.5f);
    }

    public void UnlockDirect()
    {
        img_Gray.DOFade(0, 0);
        img_Lock.gameObject.SetActive(false);
        img_Boss.gameObject.SetActive(true);
        img_Boss.rectTransform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void ShakeAni()
    {
        img_Lock.rectTransform.DOShakePosition(0.3f, 10f, 20, 0f);
    }

    public void SetSelection(bool val)
    {
        if (val)
        {
            frameObj_Idle.SetActive(false);
            frameObj_Selected.SetActive(true);
        }
        else
        {
            frameObj_Idle.SetActive(true);
            frameObj_Selected.SetActive(false);
        }
    }

    public void ResetAll()
    {
        rect.localScale = originalScale;
        img_Lock.rectTransform.eulerAngles = new Vector3(0, 0, 0);
        img_Lock.color = new Color(1, 1, 1, 1);
        uiShadow_Lock.effectColor = new Color(1, 1, 1, 0);
        uiShadow_Lock.enabled = false;
        uiEffect_Lock.enabled = false;
        img_Lock.gameObject.SetActive(true);
        img_Boss.rectTransform.eulerAngles = new Vector3(0, 90, 0);
        img_Boss.color = new Color(1, 1, 1, 1);
        uiShadow_Boss.effectColor = new Color(1, 1, 1, 0);
        uiShadow_Boss.enabled = false;
        uiEffect_Boss.enabled = false;
        img_Boss.gameObject.SetActive(false);
        img_Gray.DOFade(0, 0);
    }
}
