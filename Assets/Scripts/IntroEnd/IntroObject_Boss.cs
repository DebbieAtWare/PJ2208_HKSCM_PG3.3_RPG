using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroObject_Boss : MonoBehaviour
{
    public CanvasGroup canvasGrp;
    public GameObject blackObj;
    public GameObject arrowGrp1;
    public GameObject arrowGrp2;
    public List<TextMeshProUGUI> texts_TC;
    public List<TextMeshProUGUI> texts_SC;
    public List<TextMeshProUGUI> texts_EN;

    IEnumerator aniCoroutine;

    public void Setup()
    {
        AlphaAni(0, 0);
        arrowGrp1.SetActive(false);
        arrowGrp2.SetActive(false);

        aniCoroutine = Ani();
    }

    public void AlphaAni(float val, float aniTime)
    {
        if (val == 1)
        {
            blackObj.SetActive(true);
            canvasGrp.DOFade(val, aniTime);
        }
        else if (val == 0)
        {
            canvasGrp.DOFade(val, aniTime).OnComplete(()=> blackObj.SetActive(false));
        }
    }

    public void Play()
    {
        if (CommonUtils.instance.currLang == Language.TC)
        {
            for (int i = 0; i < texts_TC.Count; i++)
            {
                texts_TC[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < texts_SC.Count; i++)
            {
                texts_SC[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < texts_EN.Count; i++)
            {
                texts_EN[i].gameObject.SetActive(false);
            }
        }
        else if (CommonUtils.instance.currLang == Language.SC)
        {
            for (int i = 0; i < texts_TC.Count; i++)
            {
                texts_TC[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < texts_SC.Count; i++)
            {
                texts_SC[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < texts_EN.Count; i++)
            {
                texts_EN[i].gameObject.SetActive(false);
            }
        }
        else if (CommonUtils.instance.currLang == Language.EN)
        {
            for (int i = 0; i < texts_TC.Count; i++)
            {
                texts_TC[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < texts_SC.Count; i++)
            {
                texts_SC[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < texts_EN.Count; i++)
            {
                texts_EN[i].gameObject.SetActive(true);
            }
        }
        aniCoroutine = Ani();
        StartCoroutine(aniCoroutine);
    }

    IEnumerator Ani()
    {
        yield return new WaitForSeconds(0.7f);
        arrowGrp1.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        arrowGrp2.SetActive(true);
    }

    public void DirectShowAllText()
    {
        StopCoroutine(aniCoroutine);
        arrowGrp1.SetActive(true);
        arrowGrp2.SetActive(true);
    }

    public void ResetAll()
    {
        AlphaAni(0, 0);
        arrowGrp1.SetActive(false);
        arrowGrp2.SetActive(false);
    }
}
