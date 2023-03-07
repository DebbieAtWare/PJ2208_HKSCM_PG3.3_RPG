using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    IEnumerator blinkCoroutine;

    public void Setup()
    {
        img_TC.sprite = sprite_Idle;
        img_SC.sprite = sprite_Idle;
        img_EN.sprite = sprite_Idle;
        img_TC.DOFade(0.2f, 0f);
        img_SC.DOFade(0.2f, 0f);
        img_EN.DOFade(0.2f, 0f);
        blinkCoroutine = BlinkFrameAni();
    }

    private void OnEnable()
    {
        if (img_TC.sprite == sprite_Selected)
        {
            img_TC.DOFade(1f, 0f);
            img_SC.DOFade(1f, 0f);
            img_EN.DOFade(1f, 0f);
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
            img_TC.DOFade(1f, 0f);
            img_SC.DOFade(1f, 0f);
            img_EN.DOFade(1f, 0f);
            StartCoroutine(blinkCoroutine);
        }
        else
        {
            img_TC.sprite = sprite_Idle;
            img_SC.sprite = sprite_Idle;
            img_EN.sprite = sprite_Idle;
            StopCoroutine(blinkCoroutine);
            img_TC.DOFade(0.2f, 0f);
            img_SC.DOFade(0.2f, 0f);
            img_EN.DOFade(0.2f, 0f);
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
