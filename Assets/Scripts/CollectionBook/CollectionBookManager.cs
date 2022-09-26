﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public enum CollectionBookStage
{
    None,
    Main,
    Detail,
    Success
}

public class CollectionBookManager : MonoBehaviour
{
    public static CollectionBookManager instance;

    [Header("Common")]
    public CanvasGroup canvasGroup;
    public CollectionBookStage currStage;
    public int currRow = -1;
    public int currIndex_Boss = -1;
    public int currIndex_NPC = -1;

    [Header("Main")]
    public GameObject main_RootObj;
    public List<CollectionBookBossObject> main_BossObjs = new List<CollectionBookBossObject>();
    public List<CollectionBookNPCObject> main_NPCObjs = new List<CollectionBookNPCObject>();
    public RectTransform main_Scroll_ContentRect;
    public GameObject main_ExitFrameObj;
    float main_Scroll_PosGap_Small = 73;
    float main_Scroll_PosGap_Full = 185;
    public int main_Scroll_VisibleIndex = 0;
    public bool main_Scroll_PosGap_IsSmall = true;

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
    InputManager inputManager;

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
        commonUtils = CommonUtils.instance;

        inputManager = InputManager.instance;
        inputManager.onValueChanged_VerticalCallback += InputManager_OnValueChanged_Vertical;
        inputManager.onValueChanged_HorizontalCallback += InputManager_OnValueChanged_Horizontal;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;

        canvasGroup.alpha = 0;
        currRow = -1;
        currIndex_Boss = -1;
        currIndex_NPC = -1;

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

    private void InputManager_OnValueChanged_Vertical(int val)
    {
        if (currRow == 0)
        {
            if (val == -1)
            {
                currRow = 1;
                main_BossObjs[currIndex_Boss].SetSelection(false);
                main_NPCObjs[currIndex_NPC].SetSelection(true);
            }
        }
        else if (currRow == 1)
        {
            if (val == 1)
            {
                currRow = 0;
                main_NPCObjs[currIndex_NPC].SetSelection(false);
                main_BossObjs[currIndex_Boss].SetSelection(true);
            }
            else if (val == -1)
            {
                currRow = 2;
                main_NPCObjs[currIndex_NPC].SetSelection(false);
                main_ExitFrameObj.SetActive(true);
            }
        }
        else if (currRow == 2)
        {
            if (val == 1)
            {
                currRow = 1;
                main_ExitFrameObj.SetActive(false);
                main_NPCObjs[currIndex_NPC].SetSelection(true);
            }
        } 
    }

    private void InputManager_OnValueChanged_Horizontal(int val)
    {
        if (currRow == 0)
        {
            if (val == -1)
            {
                if (currIndex_Boss != 0)
                {
                    main_BossObjs[currIndex_Boss].SetSelection(false);
                    currIndex_Boss--;
                    main_BossObjs[currIndex_Boss].SetSelection(true);
                }
            }
            else if(val == 1)
            {
                if (currIndex_Boss < main_BossObjs.Count - 1)
                {
                    main_BossObjs[currIndex_Boss].SetSelection(false);
                    currIndex_Boss++;
                    main_BossObjs[currIndex_Boss].SetSelection(true);
                }
            }
        }
        else if (currRow == 1)
        {
            if (val == -1)
            {
                if (currIndex_NPC != 0)
                {
                    main_Scroll_VisibleIndex--;
                    if (main_Scroll_VisibleIndex < 0)
                    {
                        main_Scroll_VisibleIndex = 0;

                        if (main_Scroll_PosGap_IsSmall)
                        {
                            main_Scroll_ContentRect.anchoredPosition = new Vector2(main_Scroll_ContentRect.anchoredPosition.x + main_Scroll_PosGap_Small, 0f);
                        }
                        else
                        {
                            main_Scroll_ContentRect.anchoredPosition = new Vector2(main_Scroll_ContentRect.anchoredPosition.x + main_Scroll_PosGap_Full, 0);
                        }
                    }
                    main_NPCObjs[currIndex_NPC].SetSelection(false);
                    currIndex_NPC--;
                    main_NPCObjs[currIndex_NPC].SetSelection(true);
                }
            }
            else if (val == 1)
            {
                if (currIndex_NPC < main_NPCObjs.Count - 1)
                {
                    main_Scroll_VisibleIndex++;
                    if (main_Scroll_VisibleIndex > 3)
                    {
                        main_Scroll_VisibleIndex = 3;

                        if (main_Scroll_PosGap_IsSmall)
                        {
                            main_Scroll_PosGap_IsSmall = false;
                            main_Scroll_ContentRect.anchoredPosition = new Vector2(main_Scroll_ContentRect.anchoredPosition.x - main_Scroll_PosGap_Small, 0f);
                        }
                        else
                        {
                            main_Scroll_ContentRect.anchoredPosition = new Vector2(main_Scroll_ContentRect.anchoredPosition.x - main_Scroll_PosGap_Full, 0);
                        }
                    }
                    main_NPCObjs[currIndex_NPC].SetSelection(false);
                    currIndex_NPC++;
                    main_NPCObjs[currIndex_NPC].SetSelection(true);

                }
            }
        }
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        
    }

    //----- Main -----

    public void Show_Main()
    {
        currStage = CollectionBookStage.Main;
        canvasGroup.gameObject.SetActive(true);
        main_RootObj.SetActive(true);
        detail_RootObj.SetActive(false);
        success_RootObj.SetActive(false);

        //----

        currRow = 0;
        currIndex_Boss = 0;
        currIndex_NPC = 0;

        for (int i = 0; i < main_BossObjs.Count; i++)
        {
            if (commonUtils.bosses[i].IsSuccessCollectDone)
            {
                main_BossObjs[i].UnlockDirect();
            }
            if (i == 0)
            {
                main_BossObjs[i].SetSelection(true);
            }
            else
            {
                main_BossObjs[i].SetSelection(false);
            }
        }
        
        for (int i = 0; i < main_NPCObjs.Count; i++)
        {
            main_NPCObjs[i].SetSelection(false);
        }

        //----

        canvasGroup.DOFade(1f, 0.5f);
    }

    //----- Detail -----

    //----- Success -----

    public void Show_Success(ConfigData_Text successText, ConfigData_Character character, float aniTime)
    {
        currStage = CollectionBookStage.Success;
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

    }

    public void Hide_Success(float aniTime)
    {
        canvasGroup.DOFade(0, aniTime).OnComplete(HideSuccessComplete);
    }
    void HideSuccessComplete()
    {
        canvasGroup.gameObject.SetActive(false);
        currStage = CollectionBookStage.None;
    }
}
