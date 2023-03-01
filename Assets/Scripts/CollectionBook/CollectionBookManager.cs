using System.Collections;
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
    Success,
    Restart
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
    public List<GameObject> main_LangObjs_TC = new List<GameObject>();
    public List<GameObject> main_LangObjs_SC = new List<GameObject>();
    public List<GameObject> main_LangObjs_EN = new List<GameObject>();
    public List<CollectionBookBossObject> main_BossObjs = new List<CollectionBookBossObject>();
    public List<CollectionBookNPCObject> main_NPCObjs = new List<CollectionBookNPCObject>();
    public RectTransform main_Scroll_ContentRect;
    public GameObject main_CloseIpadObj;
    public GameObject main_CloseIpadFrameIdleObj;
    public GameObject main_CloseIpadFrameSelectedObj;
    public GameObject main_RestartGameObj;
    public List<GameObject> main_RestartGameFrameObjs = new List<GameObject>();
    public Image main_ArrowImgL;
    public Image main_ArrowImgR;
    public Sprite main_ArrowSprite_OnIdle;
    public Sprite main_ArrowSprite_OnSelected;
    public Sprite main_ArrowSprite_OffIdle;
    public Sprite main_ArrowSprite_OffSelected;
    float main_Scroll_PosGap = 170;
    public int main_Scroll_VisibleIndex = 0;

    [Header("Detail")]
    public GameObject detail_RootObj;
    public GameObject detail_Title_Boss_TC;
    public GameObject detail_Title_Boss_SC;
    public GameObject detail_Title_Boss_EN;
    public GameObject detail_Title_NPC_TC;
    public GameObject detail_Title_NPC_SC;
    public GameObject detail_Title_NPC_EN;
    public List<CollectionBookBossObject> detail_BossObjs = new List<CollectionBookBossObject>();
    public List<CollectionBookNPCObject> detail_NPCObjs = new List<CollectionBookNPCObject>();
    public TextMeshProUGUI detail_Info_TC;
    public TextMeshProUGUI detail_Info_SC;
    public TextMeshProUGUI detail_Info_EN;
    public TextMeshProUGUI detail_Feature_Title_TC;
    public TextMeshProUGUI detail_Feature_Title_SC;
    public TextMeshProUGUI detail_Feature_Title_EN;
    public GameObject detail_FeatureObjsRoot_TC;
    public GameObject detail_FeatureObjsRoot_SC;
    public GameObject detail_FeatureObjsRoot_EN;
    public List<CollectionBookFeatureObject> detail_FeatureObjs_TC = new List<CollectionBookFeatureObject>();
    public List<CollectionBookFeatureObject> detail_FeatureObjs_SC = new List<CollectionBookFeatureObject>();
    public List<CollectionBookFeatureObject> detail_FeatureObjs_EN = new List<CollectionBookFeatureObject>();
    public GameObject detail_ExitFrameObj_TC;
    public GameObject detail_ExitFrameObj_SC;
    public GameObject detail_ExitFrameObj_EN;
    public GameObject detail_ExitText_TC;
    public GameObject detail_ExitText_SC;
    public GameObject detail_ExitText_EN;
    public Image detail_ArrowL;
    public Image detail_ArrowR;
    public Sprite detail_ArrowSprite_On;
    public Sprite detail_ArrowSprite_Off;
    public int detail_CurrPage = 0;

    [Header("Success")]
    public GameObject success_RootObj;
    public List<CollectionBookBossObject> success_BossObjs = new List<CollectionBookBossObject>();
    public TextMeshProUGUI success_Text_TC;
    public TextMeshProUGUI success_Text_SC;
    public TextMeshProUGUI success_Text_EN;

    [Header("Restart")]
    public CanvasGroup restart_CanvasGrp;
    public List<GameObject> restart_ArrowObjs = new List<GameObject>();
    public int restart_CurrIndex;

    [Header("Confirm btn")]
    public RectTransform confirmBtnRect_Common;
    public RectTransform confirmBtnRect_Restart;
    float confirmBtnPosX_TCSC = -37f;
    float confirmBtnPosX_EN = -108f;

    CommonUtils commonUtils;
    InputManager inputManager;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("CollectionBookManager Awake");
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

        inputManager = InputManager.instance;
        inputManager.onValueChanged_VerticalCallback += InputManager_OnValueChanged_Vertical;
        inputManager.onValueChanged_HorizontalCallback += InputManager_OnValueChanged_Horizontal;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;

        canvasGroup.alpha = 0;
        currRow = -1;
        currIndex_Boss = -1;
        currIndex_NPC = -1;
        restart_CurrIndex = 0;

        for (int i = 0; i < commonUtils.bosses.Count; i++)
        {
            if (commonUtils.bosses[i].Id == main_BossObjs[i].id.ToString())
            {
                main_BossObjs[i].Setup(commonUtils.bossCards[i], commonUtils.currLang);
            }
            if (commonUtils.bosses[i].Id == detail_BossObjs[i].id.ToString())
            {
                detail_BossObjs[i].Setup(commonUtils.bossCards[i], commonUtils.currLang);
            }
            if (commonUtils.bosses[i].Id == success_BossObjs[i].id.ToString())
            {
                success_BossObjs[i].Setup(commonUtils.bossCards[i], commonUtils.currLang);
            }
        }

        for (int i = 0; i < commonUtils.NPC_Carboniferous.Count; i++)
        {
            for (int j = 0; j < main_NPCObjs.Count; j++)
            {
                if (commonUtils.NPC_Carboniferous[i].Id == main_NPCObjs[j].characterID.ToString())
                {
                    main_NPCObjs[j].Setup(MapID.Carboniferous, i, (j + 1).ToString("000"));
                    if (commonUtils.NPC_Carboniferous[i].Id == detail_NPCObjs[j].characterID.ToString())
                    {
                        detail_NPCObjs[j].Setup(MapID.Carboniferous, i, (j + 1).ToString("000"));
                    }
                    break;
                }
                
            }
        }

        for (int i = 0; i < commonUtils.NPC_Permian.Count; i++)
        {
            for (int j = 0; j < main_NPCObjs.Count; j++)
            {
                if (commonUtils.NPC_Permian[i].Id == main_NPCObjs[j].characterID.ToString())
                {
                    main_NPCObjs[j].Setup(MapID.Permian, i, (j + 1).ToString("000"));
                    if (commonUtils.NPC_Permian[i].Id == detail_NPCObjs[j].characterID.ToString())
                    {
                        detail_NPCObjs[j].Setup(MapID.Permian, i, (j + 1).ToString("000"));
                    }
                    break;
                }
            }
        }

        restart_CanvasGrp.alpha = 0;
    }

    private void CommonUtils_OnChangeLang()
    {
        ChangeLanguage();
        for (int i = 0; i < main_BossObjs.Count; i++)
        {
            main_BossObjs[i].ChangeLanguage(commonUtils.currLang);
            detail_BossObjs[i].ChangeLanguage(commonUtils.currLang);
            success_BossObjs[i].ChangeLanguage(commonUtils.currLang);
        }
    }

    private void InputManager_OnValueChanged_Vertical(int val)
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
        {
            if (OptionManager.instance.currStage == OptionStage.None)
            {
                if (currStage == CollectionBookStage.Main)
                {

                    if (currRow == 0)
                    {
                        SoundManager.instance.Play_Input(0);
                        if (val == -1)
                        {
                            currRow = 1;
                            main_BossObjs[currIndex_Boss].SetSelection(false);
                            main_NPCObjs[currIndex_NPC].SetSelection(true);
                        }
                    }
                    else if (currRow == 1)
                    {
                        SoundManager.instance.Play_Input(0);
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
                            if (commonUtils.currMapId == MapID.Lab)
                            {
                                for (int i = 0; i < main_RestartGameFrameObjs.Count; i++)
                                {
                                    main_RestartGameFrameObjs[i].SetActive(true);
                                }
                            }
                            else
                            {
                                main_CloseIpadFrameIdleObj.SetActive(false);
                                main_CloseIpadFrameSelectedObj.SetActive(true);
                            }
                        }
                    }
                    else if (currRow == 2)
                    {
                        SoundManager.instance.Play_Input(0);
                        if (val == 1)
                        {
                            currRow = 1;
                            if (commonUtils.currMapId == MapID.Lab)
                            {
                                for (int i = 0; i < main_RestartGameFrameObjs.Count; i++)
                                {
                                    main_RestartGameFrameObjs[i].SetActive(false);
                                }
                            }
                            else
                            {
                                main_CloseIpadFrameIdleObj.SetActive(true);
                                main_CloseIpadFrameSelectedObj.SetActive(false);
                            }
                            main_NPCObjs[currIndex_NPC].SetSelection(true);
                        }
                    }
                }
                else if (currStage == CollectionBookStage.Restart)
                {
                    if (restart_CurrIndex == 0)
                    {
                        SoundManager.instance.Play_Input(0);
                        if (val == -1)
                        {

                            restart_ArrowObjs[restart_CurrIndex].SetActive(false);
                            restart_CurrIndex = 1;
                            restart_ArrowObjs[restart_CurrIndex].SetActive(true);
                        }
                    }
                    else if (restart_CurrIndex == 1)
                    {
                        SoundManager.instance.Play_Input(0);
                        if (val == 1)
                        {
                            restart_ArrowObjs[restart_CurrIndex].SetActive(false);
                            restart_CurrIndex = 0;
                            restart_ArrowObjs[restart_CurrIndex].SetActive(true);
                        }
                    }
                }
            }
        }

        
    }

    private void InputManager_OnValueChanged_Horizontal(int val)
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
        {
            if (OptionManager.instance.currStage == OptionStage.None)
            {
                if (currStage == CollectionBookStage.Main)
                {

                    if (currRow == 0)
                    {
                        SoundManager.instance.Play_Input(0);
                        if (val == -1)
                        {
                            if (currIndex_Boss != 0)
                            {
                                main_BossObjs[currIndex_Boss].SetSelection(false);
                                currIndex_Boss--;
                                main_BossObjs[currIndex_Boss].SetSelection(true);
                            }
                        }
                        else if (val == 1)
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
                        SoundManager.instance.Play_Input(0);
                        if (val == -1)
                        {
                            if (currIndex_NPC != 0)
                            {
                                main_Scroll_VisibleIndex--;
                                if (main_Scroll_VisibleIndex < 0)
                                {
                                    main_Scroll_VisibleIndex = 0;
                                    main_Scroll_ContentRect.anchoredPosition = new Vector2(main_Scroll_ContentRect.anchoredPosition.x + main_Scroll_PosGap, 0);
                                }
                                main_NPCObjs[currIndex_NPC].SetSelection(false);
                                currIndex_NPC--;
                                main_NPCObjs[currIndex_NPC].SetSelection(true);
                                if (currIndex_NPC == 0)
                                {
                                    main_ArrowImgL.sprite = main_ArrowSprite_OffIdle;
                                }
                                if (currIndex_NPC < 8)
                                {
                                    main_ArrowImgR.sprite = main_ArrowSprite_OnIdle;
                                }
                            }
                        }
                        else if (val == 1)
                        {
                            if (currIndex_NPC < main_NPCObjs.Count - 1)
                            {
                                main_Scroll_VisibleIndex++;
                                if (main_Scroll_VisibleIndex > 5)
                                {
                                    main_Scroll_VisibleIndex = 5;
                                    main_Scroll_ContentRect.anchoredPosition = new Vector2(main_Scroll_ContentRect.anchoredPosition.x - main_Scroll_PosGap, 0);
                                }
                                main_NPCObjs[currIndex_NPC].SetSelection(false);
                                currIndex_NPC++;
                                main_NPCObjs[currIndex_NPC].SetSelection(true);
                                if (currIndex_NPC > 5)
                                {
                                    main_ArrowImgL.sprite = main_ArrowSprite_OnIdle;
                                }
                                if (currIndex_NPC == (main_NPCObjs.Count - 1))
                                {
                                    main_ArrowImgR.sprite = main_ArrowSprite_OffIdle;
                                }
                            }
                        }
                    }
                }
                else if (currStage == CollectionBookStage.Detail)
                {
                    SoundManager.instance.Play_Input(0);
                    if (val == -1)
                    {
                        if (detail_CurrPage == 1)
                        {
                            detail_CurrPage = 0;
                            //use change lang function, to switch detail info and feature page
                            ChangeLanguage();
                        }
                    }
                    else if (val == 1)
                    {
                        if (detail_CurrPage == 0)
                        {
                            detail_CurrPage = 1;
                            ChangeLanguage();
                        }
                    }
                }
            }
        }

        
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
        {
            if (OptionManager.instance.currStage == OptionStage.None)
            {
                if (currStage == CollectionBookStage.Main)
                {
                    if (currRow == 0)
                    {
                        if (commonUtils.bosses[currIndex_Boss].IsSuccessCollectDone)
                        {
                            SoundManager.instance.Play_SFX(0);
                            Show_Detail(commonUtils.bosses[currIndex_Boss]);
                        }
                        else
                        {
                            main_BossObjs[currIndex_Boss].ShakeAni();
                        }
                    }
                    else if (currRow == 1)
                    {
                        if (main_NPCObjs[currIndex_NPC].mapID == MapID.Carboniferous)
                        {
                            SoundManager.instance.Play_SFX(0);
                            Show_Detail(commonUtils.NPC_Carboniferous[main_NPCObjs[currIndex_NPC].configDataIndex]);
                        }
                        else if (main_NPCObjs[currIndex_NPC].mapID == MapID.Permian)
                        {
                            SoundManager.instance.Play_SFX(0);
                            Show_Detail(commonUtils.NPC_Permian[main_NPCObjs[currIndex_NPC].configDataIndex]);
                        }
                    }
                    else if (currRow == 2)
                    {
                        SoundManager.instance.Play_Input(1);
                        if (commonUtils.currMapId == MapID.Lab)
                        {
                            Show_Restart();
                        }
                        else
                        {
                            Hide_Main(0.5f);
                        }
                    }
                }
                else if (currStage == CollectionBookStage.Detail)
                {
                    SoundManager.instance.Play_Input(1);
                    Hide_Detail();
                }
                else if (currStage == CollectionBookStage.Restart)
                {
                    SoundManager.instance.Play_Input(2);
                    if (restart_CurrIndex == 0)
                    {
                        //TODO Reset game
                        Hide_Restart();
                        commonUtils.ResetGame();
                    }
                    else if (restart_CurrIndex == 1)
                    {
                        Hide_Restart();
                    }
                }
            }
        }

        
    }

    //----- Main -----

    public void Show_Main()
    {
        canvasGroup.gameObject.SetActive(true);
        main_RootObj.SetActive(true);
        detail_RootObj.SetActive(false);
        success_RootObj.SetActive(false);

        confirmBtnRect_Common.gameObject.SetActive(true);
        confirmBtnRect_Restart.gameObject.SetActive(false);

        //----

        currRow = 0;
        currIndex_Boss = 0;
        currIndex_NPC = 0;
        main_Scroll_ContentRect.anchoredPosition = new Vector2(0f, 0f);
        main_Scroll_VisibleIndex = 0;

        ChangeLanguage();

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
        main_Scroll_ContentRect.anchoredPosition = new Vector2(0, 0);

        if (commonUtils.currMapId == MapID.Lab)
        {
            for (int i = 0; i < main_RestartGameFrameObjs.Count; i++)
            {
                main_RestartGameFrameObjs[i].SetActive(false);
            }
            main_RestartGameObj.SetActive(true);
            main_CloseIpadObj.SetActive(false);
        }
        else
        {
            main_CloseIpadFrameIdleObj.SetActive(true);
            main_CloseIpadFrameSelectedObj.SetActive(false);
            main_CloseIpadObj.SetActive(true);
            main_RestartGameObj.SetActive(false);
        }

        main_ArrowImgL.sprite = main_ArrowSprite_OffIdle;
        main_ArrowImgR.sprite = main_ArrowSprite_OnIdle;

        //----

        SoundManager.instance.Play_BGM(4, 1);
        SoundManager.instance.Play_SFX(1);
        canvasGroup.DOFade(1f, 0.5f).OnComplete(() => currStage = CollectionBookStage.Main);
    }

    //----- Detail -----

    public void Show_Detail(ConfigData_Character character)
    {
        confirmBtnRect_Common.gameObject.SetActive(true);
        confirmBtnRect_Restart.gameObject.SetActive(false);

        if (character.Id == CharacterID.M01.ToString() || character.Id == CharacterID.M02.ToString() || character.Id == CharacterID.M03.ToString())
        {
            for (int i = 0; i < detail_NPCObjs.Count; i++)
            {
                detail_NPCObjs[i].gameObject.SetActive(false);
            }
            if (character.Id == CharacterID.M01.ToString())
            {
                detail_BossObjs[0].UnlockDirect();
                detail_BossObjs[0].gameObject.SetActive(true);
                detail_BossObjs[1].gameObject.SetActive(false);
                detail_BossObjs[2].gameObject.SetActive(false);
            }
            else if (character.Id == CharacterID.M02.ToString())
            {
                detail_BossObjs[1].UnlockDirect();
                detail_BossObjs[0].gameObject.SetActive(false);
                detail_BossObjs[1].gameObject.SetActive(true);
                detail_BossObjs[2].gameObject.SetActive(false);
            }
            else if (character.Id == CharacterID.M03.ToString())
            {
                detail_BossObjs[2].UnlockDirect();
                detail_BossObjs[0].gameObject.SetActive(false);
                detail_BossObjs[1].gameObject.SetActive(false);
                detail_BossObjs[2].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < detail_BossObjs.Count; i++)
            {
                detail_BossObjs[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < detail_NPCObjs.Count; i++)
            {
                if (character.Id == detail_NPCObjs[i].characterID.ToString())
                {
                    //NPC selection false, caz now is select Exit
                    detail_NPCObjs[i].gameObject.SetActive(true);
                    detail_NPCObjs[i].SetSelection(false);
                }
                else
                {
                    detail_NPCObjs[i].SetSelection(false);
                    detail_NPCObjs[i].gameObject.SetActive(false);
                }
            }
        }
        
        detail_Info_TC.text = character.InfoText.Text_TC;
        detail_Info_SC.text = character.InfoText.Text_SC;
        detail_Info_EN.text = character.InfoText.Text_EN;
        for (int i = 0; i < detail_FeatureObjs_TC.Count; i++)
        {
            if (i < character.FeatureTexts.Count)
            {
                detail_FeatureObjs_TC[i].Setup(character.FeatureTexts[i].Text_TC, 10);
            }
            else
            {
                detail_FeatureObjs_TC[i].Setup("", 0);
            }
        }
        for (int i = 0; i < detail_FeatureObjs_SC.Count; i++)
        {
            if (i < character.FeatureTexts.Count)
            {
                detail_FeatureObjs_SC[i].Setup(character.FeatureTexts[i].Text_SC, 10);
            }
            else
            {
                detail_FeatureObjs_SC[i].Setup("", 0);
            }
        }
        for (int i = 0; i < detail_FeatureObjs_EN.Count; i++)
        {
            if (i < character.FeatureTexts.Count)
            {
                detail_FeatureObjs_EN[i].Setup(character.FeatureTexts[i].Text_EN, 10);
            }
            else
            {
                detail_FeatureObjs_EN[i].Setup("", 0);
            }
        }
        detail_CurrPage = 0;
        ChangeLanguage();
        
        detail_ExitFrameObj_TC.SetActive(true);
        detail_ExitFrameObj_SC.SetActive(true);
        detail_ExitFrameObj_EN.SetActive(true);

        //----
        main_RootObj.gameObject.SetActive(false);
        detail_RootObj.gameObject.SetActive(true);
        currStage = CollectionBookStage.Detail; 
        
    }

    public void Hide_Detail()
    {
        main_RootObj.SetActive(true);
        detail_RootObj.SetActive(false);
        currStage = CollectionBookStage.Main;
    }

    //----- Success -----

    public void Show_Success(ConfigData_Text successText, ConfigData_Character character, float aniTime)
    {
        currStage = CollectionBookStage.Success;
        canvasGroup.gameObject.SetActive(true);
        main_RootObj.SetActive(false);
        detail_RootObj.SetActive(false);
        success_RootObj.SetActive(true);

        confirmBtnRect_Common.gameObject.SetActive(false);
        confirmBtnRect_Restart.gameObject.SetActive(false);

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
                if (commonUtils.currLang == Language.TC)
                {
                    success_Text_TC.DOFade(1f, 1f);
                }
                else if (commonUtils.currLang == Language.SC)
                {
                    success_Text_SC.DOFade(1f, 1f);
                }
                else if (commonUtils.currLang == Language.EN)
                {
                    success_Text_EN.DOFade(1f, 1f);
                }
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
                if (commonUtils.currLang == Language.TC)
                {
                    success_Text_TC.DOFade(1f, 1f);
                }
                else if (commonUtils.currLang == Language.SC)
                {
                    success_Text_SC.DOFade(1f, 1f);
                }
                else if (commonUtils.currLang == Language.EN)
                {
                    success_Text_EN.DOFade(1f, 1f);
                }
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
                if (commonUtils.currLang == Language.TC)
                {
                    success_Text_TC.DOFade(1f, 1f);
                }
                else if (commonUtils.currLang == Language.SC)
                {
                    success_Text_SC.DOFade(1f, 1f);
                }
                else if (commonUtils.currLang == Language.EN)
                {
                    success_Text_EN.DOFade(1f, 1f);
                }
            }
        }

    }

    //--- Restart ---

    void Show_Restart()
    {
        currStage = CollectionBookStage.Restart;
        restart_CurrIndex = 0;
        for (int i = 0; i < restart_ArrowObjs.Count; i++)
        {
            if (i == restart_CurrIndex)
            {
                restart_ArrowObjs[i].SetActive(true);
            }
            else
            {
                restart_ArrowObjs[i].SetActive(false);
            }
        }
        restart_CanvasGrp.DOFade(1f, 0.5f);

        confirmBtnRect_Common.gameObject.SetActive(false);
        confirmBtnRect_Restart.gameObject.SetActive(true);
    }

    void Hide_Restart()
    {
        restart_CanvasGrp.DOFade(0f, 0.5f).OnComplete(() => currStage = CollectionBookStage.Main);
    }

    //--- Hide ---

    void Hide_Main(float aniTime)
    {
        canvasGroup.DOFade(0, aniTime).OnComplete(HideComplete_Main);
    }

    public void Hide_Succuss(float aniTime)
    {
        canvasGroup.DOFade(0, aniTime).OnComplete(HideComplete_Succuss);
    }
    void HideComplete_Main()
    {
        if (commonUtils.currMapId == MapID.Carboniferous)
        {
            SoundManager.instance.Play_BGM(2, 1);
        }
        else if (commonUtils.currMapId == MapID.Permian)
        {
            SoundManager.instance.Play_BGM(3, 1);
        }
        canvasGroup.gameObject.SetActive(false);
        currStage = CollectionBookStage.None;
        DroneController.instance.ShowTalkHint();
        DroneController.instance.canShowTalkHint = true;
        GameManager.instance.dialogActive = false;
    }
    void HideComplete_Succuss()
    {
        canvasGroup.gameObject.SetActive(false);
        currStage = CollectionBookStage.None;
        DroneController.instance.ShowTalkHint();
        DroneController.instance.canShowTalkHint = true;
    }

    //--- Language ---

    void ChangeLanguage()
    {
        if (commonUtils.currLang == Language.TC)
        {
            //main
            for (int i = 0; i < main_LangObjs_TC.Count; i++)
            {
                main_LangObjs_TC[i].SetActive(true);
            }
            for (int i = 0; i < main_LangObjs_SC.Count; i++)
            {
                main_LangObjs_SC[i].SetActive(false);
            }
            for (int i = 0; i < main_LangObjs_EN.Count; i++)
            {
                main_LangObjs_EN[i].SetActive(false);
            }

            //detail
            if (currRow == 0)
            {
                detail_Title_Boss_TC.SetActive(true);
                detail_Title_NPC_TC.SetActive(false);
            }
            else if (currRow == 1)
            {
                detail_Title_Boss_TC.SetActive(false);
                detail_Title_NPC_TC.SetActive(true);
            }
            detail_Title_Boss_SC.SetActive(false);
            detail_Title_Boss_EN.SetActive(false);
            detail_Title_NPC_SC.SetActive(false);
            detail_Title_NPC_EN.SetActive(false);
            if (detail_CurrPage == 0)
            {
                detail_Info_TC.gameObject.SetActive(true);
                detail_Info_SC.gameObject.SetActive(false);
                detail_Info_EN.gameObject.SetActive(false);
                detail_Feature_Title_TC.gameObject.SetActive(false);
                detail_Feature_Title_SC.gameObject.SetActive(false);
                detail_Feature_Title_EN.gameObject.SetActive(false);
                detail_FeatureObjsRoot_TC.gameObject.SetActive(false);
                detail_FeatureObjsRoot_SC.gameObject.SetActive(false);
                detail_FeatureObjsRoot_EN.gameObject.SetActive(false);
            }
            else if (detail_CurrPage == 1)
            {
                detail_Info_TC.gameObject.SetActive(false);
                detail_Info_SC.gameObject.SetActive(false);
                detail_Info_EN.gameObject.SetActive(false);
                detail_Feature_Title_TC.gameObject.SetActive(true);
                detail_Feature_Title_SC.gameObject.SetActive(false);
                detail_Feature_Title_EN.gameObject.SetActive(false);
                detail_FeatureObjsRoot_TC.gameObject.SetActive(true);
                detail_FeatureObjsRoot_SC.gameObject.SetActive(false);
                detail_FeatureObjsRoot_EN.gameObject.SetActive(false);
            }
            detail_ExitText_TC.SetActive(true);
            detail_ExitText_SC.SetActive(false);
            detail_ExitText_EN.SetActive(false);

            confirmBtnRect_Restart.anchoredPosition = new Vector2(confirmBtnPosX_TCSC, confirmBtnRect_Restart.anchoredPosition.y);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            //main
            for (int i = 0; i < main_LangObjs_TC.Count; i++)
            {
                main_LangObjs_TC[i].SetActive(false);
            }
            for (int i = 0; i < main_LangObjs_SC.Count; i++)
            {
                main_LangObjs_SC[i].SetActive(true);
            }
            for (int i = 0; i < main_LangObjs_EN.Count; i++)
            {
                main_LangObjs_EN[i].SetActive(false);
            }

            //detail
            if (currRow == 0)
            {
                detail_Title_Boss_SC.SetActive(true);
                detail_Title_NPC_SC.SetActive(false);
            }
            else if(currRow == 1)
            {
                detail_Title_Boss_SC.SetActive(false);
                detail_Title_NPC_SC.SetActive(true);
            }
            detail_Title_Boss_TC.SetActive(false);
            detail_Title_Boss_EN.SetActive(false);
            detail_Title_NPC_TC.SetActive(false);
            detail_Title_NPC_EN.SetActive(false);
            if (detail_CurrPage == 0)
            {
                detail_Info_TC.gameObject.SetActive(false);
                detail_Info_SC.gameObject.SetActive(true);
                detail_Info_EN.gameObject.SetActive(false);
                detail_Feature_Title_TC.gameObject.SetActive(false);
                detail_Feature_Title_SC.gameObject.SetActive(false);
                detail_Feature_Title_EN.gameObject.SetActive(false);
                detail_FeatureObjsRoot_TC.gameObject.SetActive(false);
                detail_FeatureObjsRoot_SC.gameObject.SetActive(false);
                detail_FeatureObjsRoot_EN.gameObject.SetActive(false);
            }
            else if (detail_CurrPage == 1)
            {
                detail_Info_TC.gameObject.SetActive(false);
                detail_Info_SC.gameObject.SetActive(false);
                detail_Info_EN.gameObject.SetActive(false);
                detail_Feature_Title_TC.gameObject.SetActive(false);
                detail_Feature_Title_SC.gameObject.SetActive(true);
                detail_Feature_Title_EN.gameObject.SetActive(false);
                detail_FeatureObjsRoot_TC.gameObject.SetActive(false);
                detail_FeatureObjsRoot_SC.gameObject.SetActive(true);
                detail_FeatureObjsRoot_EN.gameObject.SetActive(false);
            }
            detail_ExitText_TC.SetActive(false);
            detail_ExitText_SC.SetActive(true);
            detail_ExitText_EN.SetActive(false);

            confirmBtnRect_Restart.anchoredPosition = new Vector2(confirmBtnPosX_TCSC, confirmBtnRect_Restart.anchoredPosition.y);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            //main
            for (int i = 0; i < main_LangObjs_TC.Count; i++)
            {
                main_LangObjs_TC[i].SetActive(false);
            }
            for (int i = 0; i < main_LangObjs_SC.Count; i++)
            {
                main_LangObjs_SC[i].SetActive(false);
            }
            for (int i = 0; i < main_LangObjs_EN.Count; i++)
            {
                main_LangObjs_EN[i].SetActive(true);
            }

            //detail
            if (currRow == 0)
            {
                detail_Title_Boss_EN.SetActive(true);
                detail_Title_NPC_EN.SetActive(false);
            }
            else if (currRow == 1)
            {
                detail_Title_Boss_EN.SetActive(false);
                detail_Title_NPC_EN.SetActive(true);
            }
            detail_Title_Boss_TC.SetActive(false);
            detail_Title_Boss_SC.SetActive(false);
            detail_Title_NPC_TC.SetActive(false);
            detail_Title_NPC_SC.SetActive(false);
            if (detail_CurrPage == 0)
            {
                detail_Info_TC.gameObject.SetActive(false);
                detail_Info_SC.gameObject.SetActive(false);
                detail_Info_EN.gameObject.SetActive(true);
                detail_Feature_Title_TC.gameObject.SetActive(false);
                detail_Feature_Title_SC.gameObject.SetActive(false);
                detail_Feature_Title_EN.gameObject.SetActive(false);
                detail_FeatureObjsRoot_TC.gameObject.SetActive(false);
                detail_FeatureObjsRoot_SC.gameObject.SetActive(false);
                detail_FeatureObjsRoot_EN.gameObject.SetActive(false);
            }
            else if (detail_CurrPage == 1)
            {
                detail_Info_TC.gameObject.SetActive(false);
                detail_Info_SC.gameObject.SetActive(false);
                detail_Info_EN.gameObject.SetActive(false);
                detail_Feature_Title_TC.gameObject.SetActive(false);
                detail_Feature_Title_SC.gameObject.SetActive(false);
                detail_Feature_Title_EN.gameObject.SetActive(true);
                detail_FeatureObjsRoot_TC.gameObject.SetActive(false);
                detail_FeatureObjsRoot_SC.gameObject.SetActive(false);
                detail_FeatureObjsRoot_EN.gameObject.SetActive(true);
            }
            detail_ExitText_TC.SetActive(false);
            detail_ExitText_SC.SetActive(false);
            detail_ExitText_EN.SetActive(true);

            confirmBtnRect_Restart.anchoredPosition = new Vector2(confirmBtnPosX_EN, confirmBtnRect_Restart.anchoredPosition.y);
        }

        //detail page arrow 
        if (detail_CurrPage == 0)
        {
            detail_ArrowL.sprite = detail_ArrowSprite_Off;
            detail_ArrowR.sprite = detail_ArrowSprite_On;
        }
        else if (detail_CurrPage == 1)
        {
            detail_ArrowL.sprite = detail_ArrowSprite_On;
            detail_ArrowR.sprite = detail_ArrowSprite_Off;
        }
    }

    private void OnDestroy()
    {
        if (commonUtils != null)
        {
            commonUtils.onChangeLangCallback -= CommonUtils_OnChangeLang;
        }
        if (inputManager != null)
        {
            inputManager.onValueChanged_VerticalCallback -= InputManager_OnValueChanged_Vertical;
            inputManager.onValueChanged_HorizontalCallback -= InputManager_OnValueChanged_Horizontal;
            inputManager.onValueChanged_ConfirmCallback -= InputManager_OnValueChanged_Confirm;
        }
    }
}
