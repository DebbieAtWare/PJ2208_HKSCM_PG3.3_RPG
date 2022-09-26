using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CollectionBookManager : MonoBehaviour
{
    public static CollectionBookManager instance;

    [Header("Common")]
    public CanvasGroup canvasGroup;

    [Header("Main")]
    public GameObject main_RootObj;
    public List<CollectionBookBossObject> main_BossObjs = new List<CollectionBookBossObject>();
    public List<CollectionBookNPCObject> main_NPCObjs = new List<CollectionBookNPCObject>();
    public ScrollRect main_NPCScrollRect;
    public GameObject main_ExitFrameObj;

    [Header("Detail")]
    public GameObject detail_RootObj;
    public List<CollectionBookBossObject> detail_BossObjs = new List<CollectionBookBossObject>();
    public List<CollectionBookNPCObject> detail_NPCObjs = new List<CollectionBookNPCObject>();
    public TextMeshProUGUI detail_Text_L_TC;
    public TextMeshProUGUI detail_Text_L_SC;
    public TextMeshProUGUI detail_Text_L_EN;
    public TextMeshProUGUI detail_Text_R_TC;
    public TextMeshProUGUI detail_Text_R_SC;
    public TextMeshProUGUI detail_Text_R_EN;
    public GameObject detail_ExitFrameObj;

    [Header("Success")]
    public GameObject success_RootObj;
    public List<CollectionBookBossObject> success_BossObjs = new List<CollectionBookBossObject>();
    public TextMeshProUGUI success_Text_TC;
    public TextMeshProUGUI success_Text_SC;
    public TextMeshProUGUI success_Text_EN;

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
            if (commonUtils.bosses[i].Id == main_BossObjs[i].id.ToString())
            {
                main_BossObjs[i].Setup(commonUtils.bosses[i].Name_TC, commonUtils.bosses[i].Name_SC, commonUtils.bosses[i].Name_EN);
            }
            if (commonUtils.bosses[i].Id == detail_BossObjs[i].id.ToString())
            {
                detail_BossObjs[i].Setup(commonUtils.bosses[i].Name_TC, commonUtils.bosses[i].Name_SC, commonUtils.bosses[i].Name_EN);
            }
            if (commonUtils.bosses[i].Id == success_BossObjs[i].id.ToString())
            {
                success_BossObjs[i].Setup(commonUtils.bosses[i].Name_TC, commonUtils.bosses[i].Name_SC, commonUtils.bosses[i].Name_EN);
            }
        }

        for (int i = 0; i < commonUtils.NPC_Carboniferous.Count; i++)
        {
            for (int j = 0; j < main_NPCObjs.Count; j++)
            {
                if (commonUtils.NPC_Carboniferous[i].Id == main_NPCObjs[j].id.ToString())
                {
                    main_NPCObjs[j].Setup((j + 1).ToString("000"));
                    break;
                }
                if (commonUtils.NPC_Carboniferous[i].Id == detail_NPCObjs[j].id.ToString())
                {
                    detail_NPCObjs[j].Setup((j + 1).ToString("000"));
                    break;
                }
            }
        }

        for (int i = 0; i < commonUtils.NPC_Permian.Count; i++)
        {
            for (int j = 0; j < main_NPCObjs.Count; j++)
            {
                if (commonUtils.NPC_Permian[i].Id == main_NPCObjs[j].id.ToString())
                {
                    main_NPCObjs[j].Setup((j + 1).ToString("000"));
                    break;
                }
                if (commonUtils.NPC_Permian[i].Id == detail_NPCObjs[j].id.ToString())
                {
                    detail_NPCObjs[j].Setup((j + 1).ToString("000"));
                    break;
                }
            }
        }
    }

    public void ShowSuccessCollect(ConfigData_Text successText, ConfigData_Character character, float aniTime)
    {
        canvasGroup.gameObject.SetActive(true);
        main_RootObj.SetActive(false);
        detail_RootObj.SetActive(false);
        success_RootObj.SetActive(true);

        for (int i = 0; i < success_BossObjs.Count; i++)
        {
            success_BossObjs[i].ResetAll();
        }

        //----

        success_Text_TC.text = "";
        success_Text_TC.DOFade(0f, 0f);
        success_Text_SC.text = "";
        success_Text_SC.DOFade(0f, 0f);
        success_Text_EN.text = "";
        success_Text_EN.DOFade(0f, 0f);

        char[] charSeparators = new char[] { '<', '>' };
        string[] splitArray_TC = successText.Text_TC.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splitArray_TC.Length; i++)
        {
            if (splitArray_TC[i] == "NPC name")
            {
                success_Text_TC.text += character.Name_TC;
            }
            else
            {
                success_Text_TC.text += splitArray_TC[i];
            }
        }
        string[] splitArray_SC = successText.Text_SC.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splitArray_SC.Length; i++)
        {
            if (splitArray_SC[i] == "NPC name")
            {
                success_Text_SC.text += character.Name_SC;
            }
            else
            {
                success_Text_SC.text += splitArray_SC[i];
            }
        }
        string[] splitArray_EN = successText.Text_EN.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splitArray_EN.Length; i++)
        {
            if (splitArray_EN[i] == "NPC name")
            {
                success_Text_EN.text += character.Name_EN;
            }
            else
            {
                success_Text_EN.text += splitArray_EN[i];
            }
        }
        
        //----

        if (character.Id == CharacterID.M01.ToString())
        {
            if (commonUtils.bosses[1].IsSuccessCollectDone)
            {
                success_BossObjs[1].UnlockDirect();
            }
            if (commonUtils.bosses[2].IsSuccessCollectDone)
            {
                success_BossObjs[2].UnlockDirect();
            }
            StartCoroutine(Ani());
            IEnumerator Ani()
            {
                canvasGroup.DOFade(1f, aniTime);
                yield return new WaitForSeconds(aniTime + 0.2f);
                success_BossObjs[0].UnlockAni();
                success_BossObjs[1].GrayOutAni();
                success_BossObjs[2].GrayOutAni();
                yield return new WaitForSeconds(1f);
                success_Text_TC.DOFade(1f, 1f);
                success_Text_SC.DOFade(1f, 1f);
                success_Text_EN.DOFade(1f, 1f);
            }
        }
        else if (character.Id == CharacterID.M02.ToString())
        {
            if (commonUtils.bosses[0].IsSuccessCollectDone)
            {
                success_BossObjs[0].UnlockDirect();
            }
            if (commonUtils.bosses[2].IsSuccessCollectDone)
            {
                success_BossObjs[2].UnlockDirect();
            }
            StartCoroutine(Ani());
            IEnumerator Ani()
            {
                canvasGroup.DOFade(1f, aniTime);
                yield return new WaitForSeconds(aniTime + 0.2f);
                success_BossObjs[1].UnlockAni();
                success_BossObjs[0].GrayOutAni();
                success_BossObjs[2].GrayOutAni();
                yield return new WaitForSeconds(1f);
                success_Text_TC.DOFade(1f, 1f);
                success_Text_SC.DOFade(1f, 1f);
                success_Text_EN.DOFade(1f, 1f);
            }
        }
        else if (character.Id == CharacterID.M03.ToString())
        {
            if (commonUtils.bosses[0].IsSuccessCollectDone)
            {
                success_BossObjs[0].UnlockDirect();
            }
            if (commonUtils.bosses[1].IsSuccessCollectDone)
            {
                success_BossObjs[1].UnlockDirect();
            }
            StartCoroutine(Ani());
            IEnumerator Ani()
            {
                canvasGroup.DOFade(1f, aniTime);
                yield return new WaitForSeconds(aniTime + 0.2f);
                success_BossObjs[2].UnlockAni();
                success_BossObjs[0].GrayOutAni();
                success_BossObjs[1].GrayOutAni();
                yield return new WaitForSeconds(1f);
                success_Text_TC.DOFade(1f, 1f);
                success_Text_SC.DOFade(1f, 1f);
                success_Text_EN.DOFade(1f, 1f);
            }
        }


        //----




    }

    public void HideSuccessCollect(float aniTime)
    {
        canvasGroup.DOFade(0, aniTime).OnComplete(() => canvasGroup.gameObject.SetActive(false));
    }
}
