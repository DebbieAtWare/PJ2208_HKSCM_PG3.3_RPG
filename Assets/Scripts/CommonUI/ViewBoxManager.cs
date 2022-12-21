using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ViewBoxManager : MonoBehaviour
{
    public static ViewBoxManager instance;

    [Header("NPC")]
    public GameObject npcObj;
    public TextMeshProUGUI npc_Text1_TC;
    public TextMeshProUGUI npc_Text1_SC;
    public TextMeshProUGUI npc_Text1_EN;
    public TextMeshProUGUI npc_Text2_TC;
    public TextMeshProUGUI npc_Text2_SC;
    public TextMeshProUGUI npc_Text2_EN;

    [Header("Drone")]
    public GameObject droneObj;
    public TextMeshProUGUI drone_Text1_TC;
    public TextMeshProUGUI drone_Text1_SC;
    public TextMeshProUGUI drone_Text1_EN;
    public TextMeshProUGUI drone_Text2_TC;
    public TextMeshProUGUI drone_Text2_SC;
    public TextMeshProUGUI drone_Text2_EN;

    [Header("Language")]
    public List<GameObject> langObjs_TC;
    public List<GameObject> langObjs_SC;
    public List<GameObject> langObjs_EN;

    CommonUtils commonUtils;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("ViewBoxManager Awake");
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

        char[] charSeparators = new char[] { '<', '>' };
        string[] splitArray_TC = commonUtils.generalInteraction_Drone.Text_TC.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splitArray_TC.Length; i++)
        {
            if (i == 0)
            {
                drone_Text1_TC.text = splitArray_TC[i];
                drone_Text1_TC.rectTransform.sizeDelta = new Vector2(drone_Text1_TC.preferredWidth, drone_Text1_TC.rectTransform.sizeDelta.y);
            }
            else if (i == 2)
            {
                drone_Text2_TC.text = splitArray_TC[i];
                drone_Text2_TC.rectTransform.sizeDelta = new Vector2(drone_Text2_TC.preferredWidth, drone_Text2_TC.rectTransform.sizeDelta.y);
            }
        }
        string[] splitArray_SC = commonUtils.generalInteraction_Drone.Text_SC.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splitArray_SC.Length; i++)
        {
            if (i == 0)
            {
                drone_Text1_SC.text = splitArray_SC[i];
                drone_Text1_SC.rectTransform.sizeDelta = new Vector2(drone_Text1_SC.preferredWidth, drone_Text1_SC.rectTransform.sizeDelta.y);
            }
            else if (i == 2)
            {
                drone_Text2_SC.text = splitArray_SC[i];
                drone_Text2_SC.rectTransform.sizeDelta = new Vector2(drone_Text2_SC.preferredWidth, drone_Text2_SC.rectTransform.sizeDelta.y);
            }
        }
        string[] splitArray_EN = commonUtils.generalInteraction_Drone.Text_EN.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splitArray_EN.Length; i++)
        {
            if (i == 0)
            {
                drone_Text1_EN.text = splitArray_EN[i];
                drone_Text1_EN.rectTransform.sizeDelta = new Vector2(drone_Text1_EN.preferredWidth, drone_Text1_EN.rectTransform.sizeDelta.y);
            }
            else if (i == 2)
            {
                drone_Text2_EN.text = splitArray_EN[i];
                drone_Text2_EN.rectTransform.sizeDelta = new Vector2(drone_Text2_EN.preferredWidth, drone_Text2_EN.rectTransform.sizeDelta.y);
            }
        }

        //----

        string[] splitArray_TC2 = commonUtils.generalInteraction_NPC.Text_TC.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splitArray_TC2.Length; i++)
        {
            if (i == 0)
            {
                npc_Text1_TC.text = splitArray_TC2[i];
                npc_Text1_TC.rectTransform.sizeDelta = new Vector2(npc_Text1_TC.preferredWidth, npc_Text1_TC.rectTransform.sizeDelta.y);
            }
            else if (i == 2)
            {
                npc_Text2_TC.text = splitArray_TC2[i];
                npc_Text2_TC.rectTransform.sizeDelta = new Vector2(npc_Text2_TC.preferredWidth, npc_Text2_TC.rectTransform.sizeDelta.y);
            }
        }
        string[] splitArray_SC2 = commonUtils.generalInteraction_NPC.Text_SC.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splitArray_SC2.Length; i++)
        {
            if (i == 0)
            {
                npc_Text1_SC.text = splitArray_SC2[i];
                npc_Text1_SC.rectTransform.sizeDelta = new Vector2(npc_Text1_SC.preferredWidth, npc_Text1_SC.rectTransform.sizeDelta.y);
            }
            else if (i == 2)
            {
                npc_Text2_SC.text = splitArray_SC2[i];
                npc_Text2_SC.rectTransform.sizeDelta = new Vector2(npc_Text2_SC.preferredWidth, npc_Text2_SC.rectTransform.sizeDelta.y);
            }
        }
        string[] splitArray_EN2 = commonUtils.generalInteraction_NPC.Text_EN.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splitArray_EN2.Length; i++)
        {
            if (i == 0)
            {
                npc_Text1_EN.text = splitArray_EN2[i];
                npc_Text1_EN.rectTransform.sizeDelta = new Vector2(npc_Text1_EN.preferredWidth, npc_Text1_EN.rectTransform.sizeDelta.y);
            }
            else if (i == 2)
            {
                npc_Text2_EN.text = splitArray_EN2[i];
                npc_Text2_EN.rectTransform.sizeDelta = new Vector2(npc_Text2_EN.preferredWidth, npc_Text2_EN.rectTransform.sizeDelta.y);
            }
        }

        ChangeLanguage();
        HideViewBox_NPC();
        HideViewBox_Drone();
    }

    public void ShowViewBox_NPC()
    {
        npcObj.SetActive(true);
    }

    public void HideViewBox_NPC()
    {
        npcObj.SetActive(false);
    }

    public void ShowViewBox_Drone()
    {
        droneObj.SetActive(true);
    }

    public void HideViewBox_Drone()
    {
        droneObj.SetActive(false);
    }

    private void CommonUtils_OnChangeLang()
    {
        ChangeLanguage();
    }

    public void ChangeLanguage()
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
        }
    }

    private void OnDestroy()
    {
        commonUtils.onChangeLangCallback -= CommonUtils_OnChangeLang;
    }
}
