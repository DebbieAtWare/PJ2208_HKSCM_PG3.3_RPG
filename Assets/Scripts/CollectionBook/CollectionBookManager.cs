using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CollectionBookManager : MonoBehaviour
{
    public static CollectionBookManager instance;

    [Header("Main")]
    public CanvasGroup canvasGroup;
    public List<CollectionBookBossObject> bossObjs = new List<CollectionBookBossObject>();
    public List<CollectionBookNPCObject> npcObjs = new List<CollectionBookNPCObject>();
    public ScrollRect npcScrollRect;
    public GameObject exitFrameObj;

    [Header("SuccessCollectGroup")]
    
    public Image successCollect_Img;
    public TextMeshProUGUI successCollect_Text_TC;

    CommonUtils commonUtils;

    void Awake()
    {
        Debug.Log("CollectionBookManager Awake");
        if (instance != null)
        {
            Debug.Log("More than one instance of CollectionBookManager");
            return;
        }
        instance = this;
    }

    public void Setup()
    {
        Debug.Log("CollectionBookManager Setup");
        commonUtils = CommonUtils.instance;

        canvasGroup.alpha = 0;

        for (int i = 0; i < commonUtils.bosses.Count; i++)
        {
            if (commonUtils.bosses[i].Id == bossObjs[i].id.ToString())
            {
                bossObjs[i].Setup(commonUtils.bosses[i].Name_TC);
            }
        }

        for (int i = 0; i < commonUtils.NPC_Carboniferous.Count; i++)
        {
            for (int j = 0; j < npcObjs.Count; j++)
            {
                if (commonUtils.NPC_Carboniferous[i].Id == npcObjs[j].id.ToString())
                {
                    npcObjs[j].Setup((j + 1).ToString("000"));
                    break;
                }
            }
        }

        for (int i = 0; i < commonUtils.NPC_Permian.Count; i++)
        {
            for (int j = 0; j < npcObjs.Count; j++)
            {
                if (commonUtils.NPC_Permian[i].Id == npcObjs[j].id.ToString())
                {
                    npcObjs[j].Setup((j + 1).ToString("000"));
                    break;
                }
            }
        }
    }

    public void ShowSuccessCollect(string name_TC, Sprite sprite)
    {
        successCollect_Text_TC.text = "成功收集" + name_TC + "！";
        successCollect_Img.sprite = sprite;
    }

    public void HideSuccessCollect(float aniTime)
    {
        //successCollect_CanvasGrp.DOFade(0, aniTime).OnComplete(() => successCollect_CanvasGrp.gameObject.SetActive(false));
    }
}
