using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum IntroVideoStage
{
    None,
    Transition_Start,
    Page1,
    Page2,
    Transition_Page2To3,
    Page3
}

public class IntroVideoManager : MonoBehaviour
{
    public static IntroVideoManager instance;

    public delegate void OnVideoStarted();
    public OnVideoStarted onVideoStartedCallback;
    public delegate void OnVideoFinished();
    public OnVideoFinished onVideoFinishedCallback;

    [Header("Lab")]
    public IntroObject_Lab labObj;

    [Header("Map")]
    public IntroObject_Map mapObj;
    public Texture mapFirstFrameTexture;

    [Header("Pixelate")]
    public RawImage pixelateImg1;
    public RawImage pixelateImg2;

    [Header("Curr")]
    public IntroVideoStage currStage;

    CommonUtils commonUtils;
    InputManager inputManager;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("IntroVideoManager Awake");
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

        labObj.onTransitionStartDoneCallback += Lab_OnTransitionStartDone;
        labObj.Setup();
        mapObj.onTransitionStartDoneCallback += Map_OnTransitionStartDone;
        mapObj.Setup();

        currStage = IntroVideoStage.None;
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (currStage == IntroVideoStage.Page1)
        {
            if (DialogBoxManager.instance.dialogWriterSingle.IsActive())
            {
                DialogBoxManager.instance.FinishCurrentDialog();
            }
            else
            {
                currStage = IntroVideoStage.Page2;
                DialogBoxManager.instance.ShowDialog(commonUtils.introVideoDialogs[1]);
            }
        }
        else if (currStage == IntroVideoStage.Page2)
        {
            if (DialogBoxManager.instance.dialogWriterSingle.IsActive())
            {
                DialogBoxManager.instance.FinishCurrentDialog();
            }
            else
            {
                TransitionAni_Page2To3();
            }
        }
    }

    public void Play()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            currStage = IntroVideoStage.Transition_Start;
            SoundManager.instance.FadeOutStop_BGM(1f);
            labObj.AlphaAni(1f, 1f);
            yield return new WaitForSeconds(1f);
            labObj.Play();
            if (onVideoStartedCallback != null)
            {
                onVideoStartedCallback.Invoke();
            }
        }
    }

    private void Lab_OnTransitionStartDone()
    {
        currStage = IntroVideoStage.Page1;
        DialogBoxManager.instance.ShowDialog(commonUtils.introVideoDialogs[0]);
    }

    void TransitionAni_Page2To3()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            currStage = IntroVideoStage.Transition_Page2To3;
            DialogBoxManager.instance.HideDialog();
            yield return new WaitForEndOfFrame();
            //screen cap lab and pixelate
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();
            pixelateImg1.texture = tex;
            pixelateImg1.DOFade(1f, 1f);
            pixelateImg1.material.DOFloat(50f, "_PixelateSize", 1f).From(512f).SetEase(Ease.Linear);
            SoundManager.instance.Play_SFX(11);
            yield return new WaitForSeconds(1f);
            //fade in pixelate map
            pixelateImg2.texture = mapFirstFrameTexture;
            pixelateImg2.material.SetFloat("_PixelateSize", 50f);
            pixelateImg2.DOFade(1f, 1f);
            yield return new WaitForSeconds(1f);
            //depixelate map
            mapObj.AlphaAni(1f, 0f);
            pixelateImg1.DOFade(0f, 0f);
            pixelateImg2.material.DOFloat(512f, "_PixelateSize", 1f).From(50f).SetEase(Ease.Linear);
            pixelateImg2.DOFade(0f, 1f);
            yield return new WaitForSeconds(1.4f);
            mapObj.Play();
        }
    }

    private void Map_OnTransitionStartDone()
    {
        currStage = IntroVideoStage.Page3;
    }
}
