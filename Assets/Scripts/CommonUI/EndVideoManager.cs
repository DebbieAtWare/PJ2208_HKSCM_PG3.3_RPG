using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EndVideoStage
{
    None,
    Page1_Playing,
    Page1_FastIn,
    Page1_EndWaiting,
    Page2_Playing,
    Page2_FastIn,
    Page2_EndWaiting
}

public class EndVideoManager : MonoBehaviour
{
    public static EndVideoManager instance;

    public delegate void OnVideoFinished();
    public OnVideoFinished onVideoFinishedCallback;

    [Header("Main")]
    public RectTransform blackBkgRect;

    [Header("Page1")]
    public RectTransform page1_textRect_TC;
    public RectTransform page1_textRect_SC;
    public RectTransform page1_textRect_EN;
    public RectTransform page1_imgRect;
    public CollectionBookBossObject page1_bossObj1;
    public CollectionBookBossObject page1_bossObj2;
    public CollectionBookBossObject page1_bossObj3;

    [Header("Page2")]
    public RectTransform page2_textRect_TC;
    public RectTransform page2_textRect_SC;
    public RectTransform page2_textRect_EN;
    public RectTransform page2_imgRect;
    public ConversationModeBossObject page2_bossObj;

    [Header("Page3")]
    public RectTransform page3_textRect_TC;
    public RectTransform page3_textRect_SC;
    public RectTransform page3_textRect_EN;
    public RectTransform page3_imgRect;
    public ConversationModeBossObject page3_bossObj;

    [Header("Page4")]
    public RectTransform page4_textRect_TC;
    public RectTransform page4_textRect_SC;
    public RectTransform page4_textRect_EN;
    public RectTransform page4_imgRect;
    public ConversationModeBossObject page4_bossObj;

    [Header("Curr")]
    public EndVideoStage currStage = EndVideoStage.None;

    Vector2 textPosTarget_Up = new Vector2(0, 540f);
    Vector2 textPosTarget_Down = new Vector2(0, -540f);
    Vector2 textPosTarget_On = new Vector2(0, 0);

    Vector2 imgPosTarget_On = new Vector2(0, 0);
    Vector2 imgPosTarget_Off = new Vector2(970, 0);

    float aniTime_Text_SlowIn = 3f;
    float aniTime_Text_FastIn = 0.3f;
    float aniTime_Text_Out = 0.3f;

    float aniTime_Img_SlowIn = 2f;
    float aniTime_Img_FastIn = 0.3f;
    float aniTime_Img_Out = 0.3f;

    IEnumerator page1_Coroutine_Play;
    IEnumerator page1_Coroutine_FastIn;
    IEnumerator page2_Coroutine_Play;
    IEnumerator page2_Coroutine_FastIn;

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

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ResetAll();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Play();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (currStage == EndVideoStage.Page1_Playing)
            {
                page1_Coroutine_FastIn = Page1_Ani_FaseIn();
                StartCoroutine(page1_Coroutine_FastIn);
            }
            else if (currStage == EndVideoStage.Page1_EndWaiting)
            {
                page2_Coroutine_Play = Page2_Ani_Play();
                StartCoroutine(page2_Coroutine_Play);
            }
            else if (currStage == EndVideoStage.Page2_Playing)
            {
                page2_Coroutine_FastIn = Page2_Ani_FastIn();
                StartCoroutine(page2_Coroutine_FastIn);
            }
        }
    }

    public void Setup()
    {
        page1_Coroutine_Play = Page1_Ani_Play();
        page1_Coroutine_FastIn = Page1_Ani_FaseIn();
        page2_Coroutine_Play = Page2_Ani_Play();
        page2_Coroutine_FastIn = Page2_Ani_FastIn();
        ResetAll();
    }

    public void Play()
    {
        page1_Coroutine_Play = Page1_Ani_Play();
        StartCoroutine(page1_Coroutine_Play);
    }

    IEnumerator Page1_Ani_Play()
    {
        currStage = EndVideoStage.Page1_Playing;
        blackBkgRect.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.7f);
        page1_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        page1_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_SlowIn).SetEase(Ease.Linear);
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
        page1_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        page1_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_FastIn).SetEase(Ease.Linear);
        yield return new WaitForSeconds(aniTime_Img_FastIn);
        currStage = EndVideoStage.Page1_EndWaiting;
    }

    IEnumerator Page2_Ani_Play()
    {
        StopCoroutine(page1_Coroutine_Play);
        StopCoroutine(page1_Coroutine_FastIn);
        page1_textRect_TC.DOAnchorPos(textPosTarget_Up, aniTime_Text_Out).SetEase(Ease.Linear);
        page1_imgRect.DOAnchorPos(imgPosTarget_Off, aniTime_Img_Out).SetEase(Ease.Linear);
        yield return new WaitForSeconds(aniTime_Text_Out);
        currStage = EndVideoStage.Page2_Playing;
        page2_bossObj.ChangeAni_Idle();
        page2_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_SlowIn).SetEase(Ease.Linear);
        page2_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_SlowIn).SetEase(Ease.Linear);
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
        page2_textRect_TC.DOAnchorPos(textPosTarget_On, aniTime_Text_FastIn).SetEase(Ease.Linear);
        page2_imgRect.DOAnchorPos(imgPosTarget_On, aniTime_Img_FastIn).SetEase(Ease.Linear);
        yield return new WaitForSeconds(aniTime_Img_FastIn);
        currStage = EndVideoStage.Page2_EndWaiting;
    }

    public void ResetAll()
    {
        currStage = EndVideoStage.None;
        StopCoroutine(page1_Coroutine_Play);
        StopCoroutine(page1_Coroutine_FastIn);
        StopCoroutine(page2_Coroutine_Play);
        StopCoroutine(page2_Coroutine_FastIn);
        DOTween.Kill(blackBkgRect);
        DOTween.Kill(page1_textRect_TC);
        DOTween.Kill(page1_textRect_SC);
        DOTween.Kill(page1_textRect_EN);
        DOTween.Kill(page1_imgRect);
        DOTween.Kill(page2_textRect_TC);
        DOTween.Kill(page2_textRect_SC);
        DOTween.Kill(page2_textRect_EN);
        DOTween.Kill(page2_imgRect);
        blackBkgRect.localScale = new Vector3(1, 0, 1);
        page1_textRect_TC.anchoredPosition = textPosTarget_Down;
        page1_textRect_SC.anchoredPosition = textPosTarget_Down;
        page1_textRect_EN.anchoredPosition = textPosTarget_Down;
        page1_imgRect.anchoredPosition = imgPosTarget_Off;
        page2_textRect_TC.anchoredPosition = textPosTarget_Down;
        page2_textRect_SC.anchoredPosition = textPosTarget_Down;
        page2_textRect_EN.anchoredPosition = textPosTarget_Down;
        page2_imgRect.anchoredPosition = imgPosTarget_Off;
        page2_bossObj.ResetAll();
    }
}
