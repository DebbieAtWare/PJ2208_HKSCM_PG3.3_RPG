using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBoxManager : MonoBehaviour
{
    public static ViewBoxManager instance;

    [Header("Main")]
    public GameObject npcObj;
    public GameObject droneObj;

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
}
