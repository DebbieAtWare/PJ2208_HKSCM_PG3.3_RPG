using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CollectionBookBossObject : MonoBehaviour
{
    [Header("Main")]
    public CharacterID id;

    [Header("Card")]
    public Image img_Boss;
    public Image img_Lock;

    [Header("Text")]
    public TextMeshProUGUI nameText_TC;

    [Header("Frame")]
    public GameObject frameObj;

    public void Setup(string name_TC)
    {
        nameText_TC.text = name_TC;
        img_Lock.rectTransform.eulerAngles = new Vector3(0, 0, 0);
        img_Lock.color = new Color(1, 1, 1, 1);
        img_Lock.gameObject.SetActive(true);
        img_Boss.rectTransform.eulerAngles = new Vector3(0, 90, 0);
        img_Boss.color = new Color(1, 1, 1, 1);
        img_Boss.gameObject.SetActive(false);
    }

    public void UnlockAni()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            img_Lock.rectTransform.DOLocalRotate(new Vector3(0, 90, 0), 0.15f);
            yield return new WaitForSeconds(0.1f);
            img_Lock.gameObject.SetActive(false);
            img_Boss.gameObject.SetActive(true);
            img_Boss.rectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0.15f);
        }
    }

    public void SetSelection(bool val)
    {
        frameObj.SetActive(val);
    }

    public void GrayOut_On()
    {
        img_Lock.color = new Color(0.4f, 0.4f, 0.4f, 1);
        img_Boss.color = new Color(0.4f, 0.4f, 0.4f, 1);
    }

    public void GrayOut_Off()
    {
        img_Lock.color = new Color(1, 1, 1, 1);
        img_Boss.color = new Color(1, 1, 1, 1);
    }

    public void ResetAll()
    {
        img_Lock.rectTransform.eulerAngles = new Vector3(0, 0, 0);
        img_Lock.color = new Color(1, 1, 1, 1);
        img_Lock.gameObject.SetActive(true);
        img_Boss.rectTransform.eulerAngles = new Vector3(0, 90, 0);
        img_Boss.color = new Color(1, 1, 1, 1);
        img_Boss.gameObject.SetActive(false);
    }
}
