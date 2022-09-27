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

    [Header("Text")]
    public TextMeshProUGUI nameText_TC;
    public TextMeshProUGUI nameText_SC;
    public TextMeshProUGUI nameText_EN;

    [Header("Frame")]
    public GameObject frameObj;

    public void Setup(string name_TC, string name_SC, string name_EN)
    {
        nameText_TC.text = name_TC;
        nameText_SC.text = name_SC;
        nameText_EN.text = name_EN;
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
    }

    public void UnlockAni()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
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
        img_Lock.DOColor(new Color(0.4f, 0.4f, 0.4f, 1), 0.5f);
        img_Boss.DOColor(new Color(0.4f, 0.4f, 0.4f, 1), 0.5f);
    }

    public void UnlockDirect()
    {
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
        frameObj.SetActive(val);
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
    }
}
