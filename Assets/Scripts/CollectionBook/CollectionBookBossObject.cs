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
    public Image img_Glow;
    public Image img_Boss;
    public Image img_Lock;
    public CanvasGroup img_Gray;

    [Header("Text")]
    public TextMeshProUGUI titleText_TC;
    public TextMeshProUGUI titleText_SC;
    public TextMeshProUGUI titleText_EN;
    public TextMeshProUGUI nameText_TC;
    public TextMeshProUGUI nameText_SC;
    public TextMeshProUGUI nameText_EN;

    [Header("Frame")]
    public GameObject frameObj_Idle;
    public Image frameObj_Selected;
    IEnumerator blinkCoroutine;

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
        originalScale = rect.localScale;
        rect.localScale = originalScale;
        img_Lock.rectTransform.eulerAngles = new Vector3(0, 0, 0);
        img_Lock.color = new Color(1, 1, 1, 1);
        img_Glow.DOFade(0, 0);
        img_Lock.gameObject.SetActive(true);
        img_Boss.rectTransform.eulerAngles = new Vector3(0, 90, 0);
        img_Boss.color = new Color(1, 1, 1, 1);
        img_Boss.gameObject.SetActive(false);
        img_Gray.DOFade(0, 0);
        ChangeLanguage(lang);

        blinkCoroutine = BlinkFrameAni();
    }

    private void OnEnable()
    {
        if (frameObj_Selected.gameObject.activeInHierarchy)
        {
            frameObj_Selected.DOFade(1f, 0f);
            blinkCoroutine = BlinkFrameAni();
            StartCoroutine(blinkCoroutine);
        }
    }

    private void OnDisable()
    {
        DOTween.Kill(frameObj_Selected);
    }

    IEnumerator BlinkFrameAni()
    {
        yield return new WaitForSeconds(0.6f);
        frameObj_Selected.DOFade(0.4f, 0.4f);
        yield return new WaitForSeconds(0.6f);
        frameObj_Selected.DOFade(1f, 0.4f);
        blinkCoroutine = BlinkFrameAni();
        StartCoroutine(blinkCoroutine);
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
            nameText_TC.rectTransform.sizeDelta = new Vector2(nameText_TC.preferredWidth, nameText_TC.rectTransform.sizeDelta.y);
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
            nameText_SC.rectTransform.sizeDelta = new Vector2(nameText_SC.preferredWidth, nameText_SC.rectTransform.sizeDelta.y);
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
            nameText_EN.rectTransform.sizeDelta = new Vector2(nameText_EN.preferredWidth, nameText_EN.rectTransform.sizeDelta.y);
        }
    }

    public void UnlockAni()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            img_Gray.DOFade(0, 0);
            rect.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.5f);
            img_Glow.DOFade(1, 1);
            yield return new WaitForSeconds(0.8f);
            img_Lock.rectTransform.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
            img_Glow.rectTransform.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
            yield return new WaitForSeconds(0.2f);
            img_Lock.gameObject.SetActive(false);
            img_Boss.gameObject.SetActive(true);
            img_Boss.rectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            img_Glow.rectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
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
        img_Lock.rectTransform.DOShakePosition(0.1f, 5f, 20, 0f, false);
    }

    public void SetSelection(bool val)
    {
        if (val)
        {
            frameObj_Idle.SetActive(false);
            frameObj_Selected.gameObject.SetActive(true);
            frameObj_Selected.DOFade(1f, 0f);
            StartCoroutine(blinkCoroutine);
        }
        else
        {
            frameObj_Idle.SetActive(true);
            frameObj_Selected.gameObject.SetActive(false);
            StopCoroutine(blinkCoroutine);
            frameObj_Selected.DOFade(0f, 0f);
        }
    }

    public void ResetAll()
    {
        rect.localScale = originalScale;
        img_Lock.rectTransform.eulerAngles = new Vector3(0, 0, 0);
        img_Lock.color = new Color(1, 1, 1, 1);
        img_Glow.DOFade(0, 0);
        img_Lock.gameObject.SetActive(true);
        img_Boss.rectTransform.eulerAngles = new Vector3(0, 90, 0);
        img_Boss.color = new Color(1, 1, 1, 1);
        img_Boss.gameObject.SetActive(false);
        img_Gray.DOFade(0, 0);
    }
}
