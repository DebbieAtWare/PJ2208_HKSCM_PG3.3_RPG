using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoManager : MonoBehaviour
{
    public static VideoManager instance;

    public delegate void OnVideoFinished_Intro();
    public OnVideoFinished_Intro onVideoFinishedCallback_Intro;

    public delegate void OnVideoStarted_Intro();
    public OnVideoStarted_Intro onVideoStartedCallback_Intro;

    public delegate void OnVideoFinished_Ending();
    public OnVideoFinished_Ending onVideoFinishedCallback_Ending;

    [Header("Intro")]
    public CanvasGroup introGrp_CanvasGrp;

    [Header("Ending")]
    public CanvasGroup endingGrp_CanvasGrp;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("VideoManager Awake");
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
        introGrp_CanvasGrp.alpha = 0;
        endingGrp_CanvasGrp.alpha = 0;
    }

    public void Play_Intro()
    {
        //TODO play intro video

        //tmp
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            introGrp_CanvasGrp.DOFade(1f, 1f);
            yield return new WaitForSeconds(1f);
            if (onVideoStartedCallback_Intro != null)
            {
                onVideoStartedCallback_Intro.Invoke();
            }
            yield return new WaitForSeconds(2f);
            introGrp_CanvasGrp.DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);
            if (onVideoFinishedCallback_Intro != null)
            {
                onVideoFinishedCallback_Intro.Invoke();
            }
        }
    }

    public void Play_Ending()
    {
        //TODO play Ending video

        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            endingGrp_CanvasGrp.DOFade(1f, 1f);
            yield return new WaitForSeconds(3f);
            //call before fade out
            if (onVideoFinishedCallback_Ending != null)
            {
                onVideoFinishedCallback_Ending.Invoke();
            }
            endingGrp_CanvasGrp.DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);
            
        }
    }
}
