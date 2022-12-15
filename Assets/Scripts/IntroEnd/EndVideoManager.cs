using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum EndVideoStage
{
    None,
    Page1_Playing,
    Page1_FastIn,
    Page1_EndWaiting,
    Page2_Playing,
    Page2_FastIn,
    Page2_EndWaiting,
    Page3_Playing,
    Page3_FastIn,
    Page3_EndWaiting,
    Page4_Playing,
    Page4_FastIn,
    Page4_EndWaiting,
    Page5
}

public class EndVideoManager : MonoBehaviour
{
    public static EndVideoManager instance;

    public delegate void OnVideoFinished();
    public OnVideoFinished onVideoFinishedCallback;

    [Header("Main")]
    public RectTransform blackBkgRect;
    public ConfirmButtonControl confirmBtnControl;

    [Header("Page1")]
    public RectTransform page1_bkgRect_Carbon;
    public CanvasGroup page1_bkgCanvasGrp_Carbon;
    public RectTransform page1_bkgRect_Permian;
    public CanvasGroup page1_bkgCanvasGrp_Permian;
    public RectTransform page1_textRect_TC;
    public RectTransform page1_textRect_SC;
    public RectTransform page1_textRect_EN;
    public RectTransform page1_imgRect;
    public CollectionBookBossObject page1_bossObj1;
    public CollectionBookBossObject page1_bossObj2;
    public CollectionBookBossObject page1_bossObj3;

    [Header("Page2")]
    public RectTransform page2_bkgRect;
    public CanvasGroup page2_bkgCanvasGrp;
    public RectTransform page2_textRect_TC;
    public RectTransform page2_textRect_SC;
    public RectTransform page2_textRect_EN;
    public RectTransform page2_imgRect;
    public ConversationModeBossObject page2_bossObj;

    [Header("Page3")]
    public RectTransform page3_bkgRect;
    public CanvasGroup page3_bkgCanvasGrp;
    public RectTransform page3_textRect_TC;
    public RectTransform page3_textRect_SC;
    public RectTransform page3_textRect_EN;
    public RectTransform page3_imgRect;
    public ConversationModeBossObject page3_bossObj;

    [Header("Page4")]
    public RectTransform page4_bkgRect;
    public CanvasGroup page4_bkgCanvasGrp;
    public RectTransform page4_textRect_TC;
    public RectTransform page4_textRect_SC;
    public RectTransform page4_textRect_EN;
    public RectTransform page4_imgRect;
    public ConversationModeBossObject page4_bossObj;

    [Header("Curr")]
    public EndVideoStage currStage = EndVideoStage.None;

    float bkgPosYTarget_Top = 0;
    float bkgPosYTarget_Bottom_Carbon = 2568;
    float bkgPosYTarget_Bottom_Permian = 4314;

    Vector2 textPosTarget_Up = new Vector2(0, 540f);
    Vector2 textPosTarget_Down = new Vector2(0, -480f);
    Vector2 textPosTarget_On = new Vector2(0, 0);

    Vector2 imgPosTarget_On = new Vector2(0, 0);
    Vector2 imgPosTarget_Off = new Vector2(970, 0);

    float aniTime_BkgSpeed = 0.3f;
    float aniTime_BkgFadeIn = 0.5f;

    float aniTime_Text_SlowIn = 6f;
    float aniTime_Text_FastIn = 0.3f;
    float aniTime_Text_Out = 0.3f;

    float aniTime_Img_SlowIn = 2f;
    float aniTime_Img_FastIn = 0.3f;
    float aniTime_Img_Out = 0.3f;

    IEnumerator page1_Coroutine_Play;
    IEnumerator page1_Coroutine_FastIn;
    IEnumerator page2_Coroutine_Play;
    IEnumerator page2_Coroutine_FastIn;
    IEnumerator page3_Coroutine_Play;
    IEnumerator page3_Coroutine_FastIn;
    IEnumerator page4_Coroutine_Play;
    IEnumerator page4_Coroutine_FastIn;

    CommonUtils commonUtils;
    InputManager inputManager;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("EndVideoManager Awake");
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
        inputManager = InputManager.instance;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;

        page1_Coroutine_Play = Page1_Ani_Play();
        page1_Coroutine_FastIn = Page1_Ani_FaseIn();
        page2_Coroutine_Play = Page2_Ani_Play();
        page2_Coroutine_FastIn = Page2_Ani_FastIn();
        page3_Coroutine_Play = Page3_Ani_Play();
        page3_Coroutine_FastIn = Page3_Ani_FastIn();
        page4_Coroutine_Play = Page4_Ani_Play();
        page4_Coroutine_FastIn = Page4_Ani_FastIn();
        ResetAll();
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (currStage == EndVideoStage.Page1_Playing)
        {
            SoundManager.instance.Play_Input(2);
            page1_Coroutine_FastIn = Page1_Ani_FaseIn();
            StartCoroutine(page1_Coroutine_FastIn);
        }
        else if (currStage == EndVideoStage.Page1_EndWaiting)
        {
            SoundManager.instance.Play_Input(2);
            page2_Coroutine_Play = Page2_Ani_Play();
            StartCoroutine(page2_Coroutine_Play);
        }
        else if (currStage == EndVideoStage.Page2_Playing)
        {
            SoundManager.instance.Play_Input(2);
            page2_Coroutine_FastIn = Page2_Ani_FastIn();
            StartCoroutine(page2_Coroutine_FastIn);
        }
        else if (currStage == EndVideoStage.Page2_EndWaiting)
        {
            SoundManager.instance.Play_Input(2);
            page3_Coroutine_Play = Page3_Ani_Play();
            StartCoroutine(page3_Coroutine_Play);
        }
        else if (currStage == EndVideoStage.Page3_Playing)
        {
            SoundManager.instance.Play_Input(2);
            page3_Coroutine_FastIn = Page3_Ani_FastIn();
            StartCoroutine(page3_Coroutine_FastIn);
        }
        else if (currStage == EndVideoStage.Page3_EndWaiting)
        {
            SoundManager.instance.Play_Input(2);
            page4_Coroutine_Play = Page4_Ani_Play();
            StartCoroutine(page4_Coroutine_Play);
        }
        else if (currStage == EndVideoStage.Page4_Playing)
        {
            SoundManager.instance.Play_Input(2);
            page4_Coroutine_FastIn = Page4_Ani_FastIn();
            StartCoroutine(page4_Coroutine_FastIn);
        }
        else if (currStage == EndVideoStage.Page4_EndWaiting)
        {
            SoundManager.instance.Play_Input(2);
            currStage = EndVideoStage.Page5;
            page4_bossObj.ResetAll();
            TransitionManager.instance.EndingVideoToLab();
        }
    }

    private void Update()
    {
        if (currStage == EndVideoStage.Page1_Playing || currStage == EndVideoStage.Page1_FastIn || currStage == EndVideoStage.Page1_EndWaiting)
        {
            if (commonUtils.currMapId == MapID.Carboniferous)
            {
                if (page1_bkgRect_Carbon.anchoredPosition.y <= bkgPosYTarget_Bottom_Carbon)
                {
                    page1_bkgRect_Carbon.anchoredPosition = new Vector2(page1_bkgRect_Carbon.anchoredPosition.x, page1_bkgRect_Carbon.anchoredPosition.y + aniTime_BkgSpeed);
                }
            }
            else if (commonUtils.currMapId == MapID.Permian)
            {
                if (page1_bkgRect_Permian.anchoredPosition.y <= bkgPosYTarget_Bottom_Permian)
                {
                    page1_bkgRect_Permian.anchoredPosition = new Vector2(page1_bkgRect_Permian.anchoredPosition.x, page1_bkgRect_Permian.anchoredPosition.y + aniTime_BkgSpeed);
                }
            }
        }
        else if (currStage == EndVideoStage.Page2_Playing || currStage == EndVideoStage.Page2_FastIn || currStage == EndVideoStage.Page2_EndWaiting)
        {
            if (page2_bkgRect.anchoredPosition.y <= bkgPosYTarget_Bottom_Carbon)
            {
                page2_bkgRect.anchoredPosition = new Vector2(page2_bkgRect.anchoredPosition.x, page2_bkgRect.anchoredPosition.y + aniTime_BkgSpeed);
            }
        }
        else if (currStage == EndVideoStage.Page3_Playing || currStage == EndVideoStage.Page3_FastIn || currStage == EndVideoStage.Page3_EndWaiting)
        {
            if (page3_bkgRect.anchoredPosition.y <= bkgPosYTarget_Bottom_Permian)
            {
                page3_bkgRect.anchoredPosition = new Vector2(page3_bkgRect.anchoredPosition.x, page3_bkgRect.anchoredPosition.y + aniTime_BkgSpeed);
            }
        }
        else if (currStage == EndVideoStage.Page4_Playing || currStage == EndVideoStage.Page4_FastIn || currStage == EndVideoStage.Page4_EndWaiting)
        {
            if (page4_bkgRect.anchoredPosition.y <= bkgPosYTarget_Bottom_Permian)
            {
                page4_bkgRect.anchoredPosition = new Vector2(page4_bkgRect.anchoredPosition.x, page4_bkgRect.anchoredPosition.y + aniTime_BkgSpeed);
            }
        }
    }

    public void Play()
    {
        page1_Coroutine_Play = Page1_Ani_Play();
        StartCoroutine(page1_Coroutine_Play);
    }

    IEnumerator Page1_Ani_Play()
    {
        SoundManager.instance.FadeOutStop_BGM(1f);
        blackBkgRect.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.7f);
        if (commonUtils.currMapId == MapID.Carboniferous)
        {
            page1_bkgCanvasGrp_Carbon.DOFade(1, aniTime_BkgFadeIn).OnComplete(() => currStage = EndVideoStage.Page1_Playing);
        }
        else if (commonUtils.currMapId == MapID.Permian)
        {
            page1_bkgCanvasGrp_Permian.DOFade(1, aniTime_BkgFadeIn).OnComplete(() => currStage = EndVideoStage.Page1_Playing);
        }
        if (commonUtils.currLang == Language.TC)
        {
            page1_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            page1_textRect_SC.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            page1_textRect_EN.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        page1_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_SlowIn).SetEase(Ease.Linear);
        page1_bossObj1.Setup(commonUtils.bosses[0].Name_TC, commonUtils.bosses[0].Name_SC, commonUtils.bosses[0].Name_EN, commonUtils.currLang);
        page1_bossObj2.Setup(commonUtils.bosses[1].Name_TC, commonUtils.bosses[1].Name_SC, commonUtils.bosses[1].Name_EN, commonUtils.currLang);
        page1_bossObj3.Setup(commonUtils.bosses[2].Name_TC, commonUtils.bosses[2].Name_SC, commonUtils.bosses[2].Name_EN, commonUtils.currLang);
        page1_bossObj1.UnlockDirect();
        page1_bossObj2.UnlockDirect();
        page1_bossObj3.UnlockDirect();
        CancelInvoke("InvokeShowConfirmBtn");
        confirmBtnControl.SetAlpha(0, 0);
        Invoke("InvokeShowConfirmBtn", 1f);
        yield return new WaitForSeconds(aniTime_Text_SlowIn);
        currStage = EndVideoStage.Page1_EndWaiting;
    }

    IEnumerator Page1_Ani_FaseIn()
    {
        currStage = EndVideoStage.Page1_FastIn;
        StopCoroutine(page1_Coroutine_Play);
        DOTween.Kill(blackBkgRect);
        DOTween.Kill(page1_textRect_TC);
        DOTween.Kill(page1_textRect_SC);
        DOTween.Kill(page1_textRect_EN);
        DOTween.Kill(page1_imgRect);
        blackBkgRect.DOScale(new Vector3(1, 1, 1), aniTime_Img_FastIn).SetEase(Ease.Linear);
        if (commonUtils.currLang == Language.TC)
        {
            page1_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            page1_textRect_SC.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            page1_textRect_EN.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        page1_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_FastIn).SetEase(Ease.Linear);
        yield return new WaitForSeconds(aniTime_Img_FastIn);
        currStage = EndVideoStage.Page1_EndWaiting;
    }

    //----------

    IEnumerator Page2_Ani_Play()
    {
        StopCoroutine(page1_Coroutine_Play);
        StopCoroutine(page1_Coroutine_FastIn);
        CancelInvoke("InvokeShowConfirmBtn");
        confirmBtnControl.SetAlpha(0, 0);
        if (commonUtils.currLang == Language.TC)
        {
            page1_textRect_TC.DOAnchorPos(textPosTarget_Up, aniTime_Text_Out).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            page1_textRect_SC.DOAnchorPos(textPosTarget_Up, aniTime_Text_Out).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            page1_textRect_EN.DOAnchorPos(textPosTarget_Up, aniTime_Text_Out).SetEase(Ease.Linear);
        }
        page1_imgRect.DOAnchorPos(imgPosTarget_Off, aniTime_Img_Out).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.2f);
        currStage = EndVideoStage.Page2_Playing;
        page2_bkgCanvasGrp.DOFade(1f, aniTime_BkgFadeIn);
        page2_bossObj.ChangeAni_Idle();
        if (commonUtils.currLang == Language.TC)
        {
            page2_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            page2_textRect_SC.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            page2_textRect_EN.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        page2_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_SlowIn).SetEase(Ease.Linear);
        CancelInvoke("InvokeShowConfirmBtn");
        confirmBtnControl.SetAlpha(0, 0);
        Invoke("InvokeShowConfirmBtn", 1f);
        yield return new WaitForSeconds(aniTime_Text_SlowIn);
        currStage = EndVideoStage.Page2_EndWaiting;
    }

    IEnumerator Page2_Ani_FastIn()
    {
        currStage = EndVideoStage.Page2_FastIn;
        StopCoroutine(page2_Coroutine_Play);
        DOTween.Kill(page2_textRect_TC);
        DOTween.Kill(page2_textRect_SC);
        DOTween.Kill(page2_textRect_EN);
        DOTween.Kill(page2_imgRect);
        if (commonUtils.currLang == Language.TC)
        {
            page2_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            page2_textRect_SC.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            page2_textRect_EN.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        page2_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_FastIn).SetEase(Ease.Linear);
        yield return new WaitForSeconds(aniTime_Img_FastIn);
        currStage = EndVideoStage.Page2_EndWaiting;
    }

    //----------

    IEnumerator Page3_Ani_Play()
    {
        StopCoroutine(page2_Coroutine_Play);
        StopCoroutine(page2_Coroutine_FastIn);
        CancelInvoke("InvokeShowConfirmBtn");
        confirmBtnControl.SetAlpha(0, 0);
        if (commonUtils.currLang == Language.TC)
        {
            page2_textRect_TC.DOAnchorPos(textPosTarget_Up, aniTime_Text_Out).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            page2_textRect_SC.DOAnchorPos(textPosTarget_Up, aniTime_Text_Out).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            page2_textRect_EN.DOAnchorPos(textPosTarget_Up, aniTime_Text_Out).SetEase(Ease.Linear);
        }
        page2_imgRect.DOAnchorPos(imgPosTarget_Off, aniTime_Img_Out).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.2f);
        currStage = EndVideoStage.Page3_Playing;
        page3_bkgCanvasGrp.DOFade(1f, aniTime_BkgFadeIn);
        page3_bossObj.ChangeAni_Idle();
        if (commonUtils.currLang == Language.TC)
        {
            page3_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            page3_textRect_SC.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            page3_textRect_EN.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        page3_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_SlowIn).SetEase(Ease.Linear);
        CancelInvoke("InvokeShowConfirmBtn");
        confirmBtnControl.SetAlpha(0, 0);
        Invoke("InvokeShowConfirmBtn", 1f);
        yield return new WaitForSeconds(aniTime_Text_SlowIn);
        currStage = EndVideoStage.Page3_EndWaiting;
    }

    IEnumerator Page3_Ani_FastIn()
    {
        currStage = EndVideoStage.Page3_FastIn;
        StopCoroutine(page3_Coroutine_Play);
        DOTween.Kill(page3_textRect_TC);
        DOTween.Kill(page3_textRect_SC);
        DOTween.Kill(page3_textRect_EN);
        DOTween.Kill(page3_imgRect);
        if (commonUtils.currLang == Language.TC)
        {
            page3_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            page3_textRect_SC.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            page3_textRect_EN.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        page3_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_FastIn).SetEase(Ease.Linear);
        yield return new WaitForSeconds(aniTime_Img_FastIn);
        currStage = EndVideoStage.Page3_EndWaiting;
    }

    //----------

    IEnumerator Page4_Ani_Play()
    {
        StopCoroutine(page3_Coroutine_Play);
        StopCoroutine(page3_Coroutine_FastIn);
        CancelInvoke("InvokeShowConfirmBtn");
        confirmBtnControl.SetAlpha(0, 0);
        if (commonUtils.currLang == Language.TC)
        {
            page3_textRect_TC.DOAnchorPos(textPosTarget_Up, aniTime_Text_Out).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            page3_textRect_SC.DOAnchorPos(textPosTarget_Up, aniTime_Text_Out).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            page3_textRect_EN.DOAnchorPos(textPosTarget_Up, aniTime_Text_Out).SetEase(Ease.Linear);
        }
        page3_imgRect.DOAnchorPos(imgPosTarget_Off, aniTime_Img_Out).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.2f);
        currStage = EndVideoStage.Page4_Playing;
        page4_bkgCanvasGrp.DOFade(1f, aniTime_BkgFadeIn);
        page4_bossObj.ChangeAni_Idle();
        if (commonUtils.currLang == Language.TC)
        {
            page4_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            page4_textRect_SC.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            page4_textRect_EN.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        }
        page4_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_SlowIn).SetEase(Ease.Linear);
        CancelInvoke("InvokeShowConfirmBtn");
        confirmBtnControl.SetAlpha(0, 0);
        Invoke("InvokeShowConfirmBtn", 1f);
        yield return new WaitForSeconds(aniTime_Text_SlowIn);
        currStage = EndVideoStage.Page4_EndWaiting;
    }

    IEnumerator Page4_Ani_FastIn()
    {
        currStage = EndVideoStage.Page4_FastIn;
        StopCoroutine(page4_Coroutine_Play);
        DOTween.Kill(page4_textRect_TC);
        DOTween.Kill(page4_textRect_SC);
        DOTween.Kill(page4_textRect_EN);
        DOTween.Kill(page4_imgRect);
        if (commonUtils.currLang == Language.TC)
        {
            page4_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.SC)
        {
            page4_textRect_SC.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            page4_textRect_EN.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        }
        page4_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_FastIn).SetEase(Ease.Linear);
        yield return new WaitForSeconds(aniTime_Img_FastIn);
        currStage = EndVideoStage.Page4_EndWaiting;
    }

    public void ResetAll()
    {
        currStage = EndVideoStage.None;
        StopCoroutine(page1_Coroutine_Play);
        StopCoroutine(page1_Coroutine_FastIn);
        StopCoroutine(page2_Coroutine_Play);
        StopCoroutine(page2_Coroutine_FastIn);
        StopCoroutine(page3_Coroutine_Play);
        StopCoroutine(page3_Coroutine_FastIn);
        StopCoroutine(page4_Coroutine_Play);
        StopCoroutine(page4_Coroutine_FastIn);
        DOTween.Kill(blackBkgRect);
        DOTween.Kill(page1_textRect_TC);
        DOTween.Kill(page1_textRect_SC);
        DOTween.Kill(page1_textRect_EN);
        DOTween.Kill(page1_imgRect);
        DOTween.Kill(page2_textRect_TC);
        DOTween.Kill(page2_textRect_SC);
        DOTween.Kill(page2_textRect_EN);
        DOTween.Kill(page2_imgRect);
        DOTween.Kill(page3_textRect_TC);
        DOTween.Kill(page3_textRect_SC);
        DOTween.Kill(page3_textRect_EN);
        DOTween.Kill(page3_imgRect);
        DOTween.Kill(page4_textRect_TC);
        DOTween.Kill(page4_textRect_SC);
        DOTween.Kill(page4_textRect_EN);
        DOTween.Kill(page4_imgRect);
        blackBkgRect.localScale = new Vector3(1, 0, 1);
        page1_bkgCanvasGrp_Carbon.alpha = 0;
        page1_bkgRect_Carbon.anchoredPosition = new Vector2(page1_bkgRect_Carbon.anchoredPosition.x, bkgPosYTarget_Top);
        page1_bkgCanvasGrp_Permian.alpha = 0;
        page1_bkgRect_Permian.anchoredPosition = new Vector2(page1_bkgRect_Permian.anchoredPosition.x, bkgPosYTarget_Top);
        page1_textRect_TC.anchoredPosition = textPosTarget_Down;
        page1_textRect_SC.anchoredPosition = textPosTarget_Down;
        page1_textRect_EN.anchoredPosition = textPosTarget_Down;
        page1_imgRect.anchoredPosition = imgPosTarget_Off;
        page2_bkgCanvasGrp.alpha = 0;
        page2_bkgRect.anchoredPosition = new Vector2(page2_bkgRect.anchoredPosition.x, bkgPosYTarget_Top);
        page2_textRect_TC.anchoredPosition = textPosTarget_Down;
        page2_textRect_SC.anchoredPosition = textPosTarget_Down;
        page2_textRect_EN.anchoredPosition = textPosTarget_Down;
        page2_imgRect.anchoredPosition = imgPosTarget_Off;
        page2_bossObj.ResetAll();
        page3_bkgCanvasGrp.alpha = 0;
        page3_bkgRect.anchoredPosition = new Vector2(page3_bkgRect.anchoredPosition.x, bkgPosYTarget_Top);
        page3_textRect_TC.anchoredPosition = textPosTarget_Down;
        page3_textRect_SC.anchoredPosition = textPosTarget_Down;
        page3_textRect_EN.anchoredPosition = textPosTarget_Down;
        page3_imgRect.anchoredPosition = imgPosTarget_Off;
        page3_bossObj.ResetAll();
        page4_bkgCanvasGrp.alpha = 0;
        page4_bkgRect.anchoredPosition = new Vector2(page4_bkgRect.anchoredPosition.x, bkgPosYTarget_Top);
        //TODO textPosTarget_Down should be generate dynamic but not hardcode!!
        page4_textRect_TC.anchoredPosition = new Vector2(0, -515f);
        page4_textRect_SC.anchoredPosition = textPosTarget_Down;
        page4_textRect_EN.anchoredPosition = textPosTarget_Down;
        page4_imgRect.anchoredPosition = imgPosTarget_Off;
        page4_bossObj.ResetAll();
        CancelInvoke("InvokeShowConfirmBtn");
        confirmBtnControl.SetAlpha(0, 0);
    }

    void InvokeShowConfirmBtn()
    {
        confirmBtnControl.SetAlpha(1, 0);
    }
}
