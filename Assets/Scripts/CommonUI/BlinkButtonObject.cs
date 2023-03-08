using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlinkButtonObject : MonoBehaviour
{
    public GameObject obj_TC;
    public GameObject obj_SC;
    public GameObject obj_EN;
    public Image img_TC;
    public Image img_SC;
    public Image img_EN;
    public Sprite sprite_Idle;
    public Sprite sprite_Selected;
    public List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    IEnumerator blinkCoroutine;

    public void Setup()
    {
        img_TC.sprite = sprite_Idle;
        img_SC.sprite = sprite_Idle;
        img_EN.sprite = sprite_Idle;
        img_TC.color = new Color(img_TC.color.r, img_TC.color.g, img_TC.color.b, 0.2f);
        img_SC.color = new Color(img_SC.color.r, img_SC.color.g, img_SC.color.b, 0.2f);
        img_EN.color = new Color(img_EN.color.r, img_EN.color.g, img_EN.color.b, 0.2f);
        blinkCoroutine = BlinkFrameAni();
    }

    private void OnEnable()
    {
        if (img_TC.sprite == sprite_Selected)
        {
            img_TC.color = new Color(img_TC.color.r, img_TC.color.g, img_TC.color.b, 1f);
            img_SC.color = new Color(img_SC.color.r, img_SC.color.g, img_SC.color.b, 1f);
            img_EN.color = new Color(img_EN.color.r, img_EN.color.g, img_EN.color.b, 1f);
            blinkCoroutine = BlinkFrameAni();
            StartCoroutine(blinkCoroutine);
        }
    }

    IEnumerator BlinkFrameAni()
    {
        yield return new WaitForSeconds(0.6f);
        img_TC.DOFade(0.4f, 0.4f);
        img_SC.DOFade(0.4f, 0.4f);
        img_EN.DOFade(0.4f, 0.4f);
        yield return new WaitForSeconds(0.6f);
        img_TC.DOFade(1f, 0.4f);
        img_SC.DOFade(1f, 0.4f);
        img_EN.DOFade(1f, 0.4f);
        blinkCoroutine = BlinkFrameAni();
        StartCoroutine(blinkCoroutine);
    }

    public void SetSelection(bool val)
    {
        if (val)
        {
            img_TC.sprite = sprite_Selected;
            img_SC.sprite = sprite_Selected;
            img_EN.sprite = sprite_Selected;
            img_TC.color = new Color(img_TC.color.r, img_TC.color.g, img_TC.color.b, 1f);
            img_SC.color = new Color(img_SC.color.r, img_SC.color.g, img_SC.color.b, 1f);
            img_EN.color = new Color(img_EN.color.r, img_EN.color.g, img_EN.color.b, 1f);
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(blinkCoroutine);
            }
        }
        else
        {
            StopCoroutine(blinkCoroutine);
            DOTween.Kill(img_TC);
            DOTween.Kill(img_SC);
            DOTween.Kill(img_EN);
            img_TC.sprite = sprite_Idle;
            img_SC.sprite = sprite_Idle;
            img_EN.sprite = sprite_Idle;
            img_TC.color = new Color(img_TC.color.r, img_TC.color.g, img_TC.color.b, 0.2f);
            img_SC.color = new Color(img_SC.color.r, img_SC.color.g, img_SC.color.b, 0.2f);
            img_EN.color = new Color(img_EN.color.r, img_EN.color.g, img_EN.color.b, 0.2f);
        }
    }

    public void SetTextAlpha(float alpha)
    {
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, alpha);
        }
    }

    public void ChangeLanguage(Language lang)
    {
        if (lang == Language.TC)
        {
            obj_TC.SetActive(true);
            obj_SC.SetActive(false);
            obj_EN.SetActive(false);
        }
        else if (lang == Language.SC)
        {
            obj_TC.SetActive(false);
            obj_SC.SetActive(true);
            obj_EN.SetActive(false);
        }
        else if (lang == Language.EN)
        {
            obj_TC.SetActive(false);
            obj_SC.SetActive(false);
            obj_EN.SetActive(true);
        }
    }
}
