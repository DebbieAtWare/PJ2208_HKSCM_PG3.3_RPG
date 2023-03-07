using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionBookNPCObject : MonoBehaviour
{
    public CharacterID characterID;
    public MapID mapID { get; private set; }
    public int configDataIndex { get; private set; }
    public GameObject frame_In;
    public Image frame_Out;
    public bool isBlink = false;
    public TextMeshProUGUI numText_In;
    public TextMeshProUGUI numText_Out;

    public void Setup(MapID _mapID, int _configDataIndex, string numText)
    {
        mapID = _mapID;
        configDataIndex = _configDataIndex;
        numText_In.text = numText;
        numText_Out.text = numText;
        if (isBlink)
        {
            frame_Out.DOFade(0.4f, 0f);
            BlinkFrameAni();
        }
    }

    private void OnEnable()
    {
        if (isBlink)
        {
            frame_Out.DOFade(0.4f, 0f);
            BlinkFrameAni();
        }
    }

    void BlinkFrameAni()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            frame_Out.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(0.8f);
            frame_Out.DOFade(0.4f, 0.5f);
            yield return new WaitForSeconds(0.8f);
            BlinkFrameAni();
        }
    }

    public void SetSelection(bool val)
    {
        if (val)
        {
            frame_In.SetActive(false);
            frame_Out.gameObject.SetActive(true);
            numText_In.gameObject.SetActive(false);
            numText_Out.gameObject.SetActive(true);
        }
        else
        {
            frame_In.SetActive(true);
            frame_Out.gameObject.SetActive(false);
            numText_In.gameObject.SetActive(true);
            numText_Out.gameObject.SetActive(false);
        }
    }
}
