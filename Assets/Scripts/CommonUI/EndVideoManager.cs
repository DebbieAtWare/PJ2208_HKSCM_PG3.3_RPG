using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Play()
    {

    }

    public void ResetAll()
    {

    }
}
