using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum IntroVideoStage
{
    None,
    Transition_Start,
    Page1,
    Page2,
    Transition_Page2To3,
    Page3,
    Page4,
    Transition_Page4To5,
    Page5,
    Transition_Page5To6,
    Page6,
    Transition_Page6To7,
    Page7,
    Transition_Page7To8,
    Page8,
    Transition_Page8To9,
    Page9,
    Transition_Page9To10
}

public class IntroVideoManager : MonoBehaviour
{
    public static IntroVideoManager instance;

    public delegate void OnVideoStarted();
    public OnVideoStarted onVideoStartedCallback;
    public delegate void OnVideoFinished();
    public OnVideoFinished onVideoFinishedCallback;

    [Header("Root")]
    public GameObject root;

    [Header("Lab")]
    public IntroObject_Lab labObj;

    [Header("Map")]
    public IntroObject_Map mapObj;
    public Texture mapFirstFrameTexture;

    [Header("Egg")]
    public IntroObject_Egg eggObj;

    [Header("Boss")]
    public IntroObject_Boss bossObj;

    [Header("CharacterGroup")]
    public IntroObject_CharacterGroup characterGrp1;
    public IntroObject_CharacterGroup characterGrp2;

    [Header("BlackBar")]
    public IntroObject_BlackBar blackBarObj;
    public Texture labBlackBarTexture;

    [Header("Pixelate")]
    public RawImage pixelateImg1;
    public RawImage pixelateImg2;

    [Header("Text")]
    public DialogWriter.DialogWriterSingle dialogWriterSingle;
    public TextMeshProUGUI text_TC;
    public TextMeshProUGUI text_SC;
    public TextMeshProUGUI text_EN;

    [Header("Confirm")]
    public ConfirmButtonControl confirmBtnControl;

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
        eggObj.Setup();
        bossObj.Setup();
        characterGrp1.Setup();
        characterGrp2.Setup();
        blackBarObj.onVideoEndCallback += BlackBar_OnVideoEnd;
        blackBarObj.Setup();

        text_TC.alpha = 0;
        text_SC.alpha = 0;
        text_EN.alpha = 0;

        currStage = IntroVideoStage.None;
        root.SetActive(false);
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
        {
            if (currStage == IntroVideoStage.Page1)
            {
                SoundManager.instance.Play_Input(2);
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
                SoundManager.instance.Play_Input(2);
                //use dialog box's dialog writer
                if (DialogBoxManager.instance.dialogWriterSingle.IsActive())
                {
                    DialogBoxManager.instance.FinishCurrentDialog();
                }
                else
                {
                    TransitionAni_Page2To3();
                }
            }
            else if (currStage == IntroVideoStage.Page3)
            {
                SoundManager.instance.Play_Input(2);
                CancelInvoke("AutoPlay_Page3To4");
                //use current script dialog writer
                if (!dialogWriterSingle.IsActive() && mapObj.IsInLoopArea())
                {
                    currStage = IntroVideoStage.Page4;
                    ShowDialog(commonUtils.introVideoDialogs[3]);
                }
                else
                {
                    FinishCurrentDialog();
                    mapObj.ChangeFPS_Fast();
                }
            }
            else if (currStage == IntroVideoStage.Page4)
            {
                SoundManager.instance.Play_Input(2);
                CancelInvoke("AutoPlay_Page4To5");
                if (!dialogWriterSingle.IsActive() && mapObj.IsInLoopArea())
                {
                    TransitionAni_Page4To5();
                }
                else
                {
                    FinishCurrentDialog();
                }
            }
            else if (currStage == IntroVideoStage.Page5)
            {
                SoundManager.instance.Play_Input(2);
                CancelInvoke("AutoPlay_Page5To6");
                if (!dialogWriterSingle.IsActive())
                {
                    TransitionAni_Page5To6();
                }
                else
                {
                    FinishCurrentDialog();
                    eggObj.DirectShowAllText();
                }
            }
            else if (currStage == IntroVideoStage.Page6)
            {
                SoundManager.instance.Play_Input(2);
                CancelInvoke("AutoPlay_Page6To7");
                if (!dialogWriterSingle.IsActive())
                {
                    TransitionAni_Page6To7();
                }
                else
                {
                    FinishCurrentDialog();
                }
            }
            else if (currStage == IntroVideoStage.Page7)
            {
                SoundManager.instance.Play_Input(2);
                CancelInvoke("AutoPlay_Page7To8");
                if (!dialogWriterSingle.IsActive())
                {
                    TransitionAni_Page7To8();
                }
                else
                {
                    FinishCurrentDialog();
                    bossObj.DirectShowAllText();
                }
            }
            else if (currStage == IntroVideoStage.Page8)
            {
                SoundManager.instance.Play_Input(2);
                CancelInvoke("AutoPlay_Page8To9");
                if (!dialogWriterSingle.IsActive() && characterGrp1.IsInLoopArea())
                {
                    TransitionAni_Page8To9();
                }
                else
                {
                    FinishCurrentDialog();
                    characterGrp1.ChangeFPS_Fast();
                }
            }
            else if (currStage == IntroVideoStage.Page9)
            {
                SoundManager.instance.Play_Input(2);
                CancelInvoke("AutoPlay_Page9To10");
                if (!dialogWriterSingle.IsActive() && characterGrp2.IsInLoopArea())
                {
                    TransitionAni_Page9To10();
                }
                else
                {
                    FinishCurrentDialog();
                    characterGrp2.ChangeFPS_Fast();
                }
            }
        }
    }

    public void Play()
    {
        //prevent user press A many time and play many time
        if (currStage == IntroVideoStage.None)
        {
            StartCoroutine(Ani());
        }
        IEnumerator Ani()
        {
            root.SetActive(true);
            currStage = IntroVideoStage.Transition_Start;
            SoundManager.instance.Play_BGM(6, 4);
            if (commonUtils.currLang == Language.TC)
            {
                text_TC.alpha = 1;
                text_SC.alpha = 0;
                text_EN.alpha = 0;
            }
            else if (commonUtils.currLang == Language.SC)
            {
                text_TC.alpha = 0;
                text_SC.alpha = 1;
                text_EN.alpha = 0;
            }
            else if (commonUtils.currLang == Language.EN)
            {
                text_TC.alpha = 0;
                text_SC.alpha = 0;
                text_EN.alpha = 1;
            }
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
            pixelateImg2.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            //depixelate map
            mapObj.AlphaAni(1f, 0f);
            pixelateImg1.DOFade(0f, 0f);
            pixelateImg2.material.DOFloat(512f, "_PixelateSize", 1f).From(50f).SetEase(Ease.Linear);
            pixelateImg2.DOFade(0f, 1f);
            yield return new WaitForSeconds(0.5f);
            mapObj.Play();
        }
    }

    private void Map_OnTransitionStartDone()
    {
        currStage = IntroVideoStage.Page3;
        ShowDialog(commonUtils.introVideoDialogs[2]);

        Invoke("AutoPlay_Page3To4", commonUtils.data.IntroVideo_AutoRun_Page3To4);
    }

    void AutoPlay_Page3To4()
    {
        currStage = IntroVideoStage.Page4;
        ShowDialog(commonUtils.introVideoDialogs[3]);
        Invoke("AutoPlay_Page4To5", commonUtils.data.IntroVideo_AutoRun_Page4To5);
    }

    void AutoPlay_Page4To5()
    {
        TransitionAni_Page4To5();
    }

    void TransitionAni_Page4To5()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            currStage = IntroVideoStage.Transition_Page4To5;
            eggObj.AlphaAni(1, 1);
            ClearDialog();
            yield return new WaitForSeconds(1f);
            currStage = IntroVideoStage.Page5;
            eggObj.Play();
            ShowDialog(commonUtils.introVideoDialogs[4]);
            Invoke("AutoPlay_Page5To6", commonUtils.data.IntroVideo_AutoRun_Page5To6);
        }
    }

    void AutoPlay_Page5To6()
    {
        TransitionAni_Page5To6();
    }

    void TransitionAni_Page5To6()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            currStage = IntroVideoStage.Transition_Page5To6;
            eggObj.AlphaAni(0, 1);
            ClearDialog();
            yield return new WaitForSeconds(1f);
            currStage = IntroVideoStage.Page6;
            ShowDialog(commonUtils.introVideoDialogs[5]);
            eggObj.ResetAll();
            Invoke("AutoPlay_Page6To7", commonUtils.data.IntroVideo_AutoRun_Page6To7);
        }
    }

    void AutoPlay_Page6To7()
    {
        TransitionAni_Page6To7();
    }

    void TransitionAni_Page6To7()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            currStage = IntroVideoStage.Transition_Page6To7;
            bossObj.AlphaAni(1, 1);
            ClearDialog();
            yield return new WaitForSeconds(1f);
            currStage = IntroVideoStage.Page7;
            ShowDialog(commonUtils.introVideoDialogs[6]);
            bossObj.Play();
            Invoke("AutoPlay_Page7To8", commonUtils.data.IntroVideo_AutoRun_Page7To8);
        }
    }

    void AutoPlay_Page7To8()
    {
        TransitionAni_Page7To8();
    }

    void TransitionAni_Page7To8()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            currStage = IntroVideoStage.Transition_Page7To8;
            characterGrp1.AlphaAni(1, 0.5f);
            ClearDialog();
            yield return new WaitForSeconds(0.2f);
            currStage = IntroVideoStage.Page8;
            ShowDialog(commonUtils.introVideoDialogs[7]);
            characterGrp1.Play();
            Invoke("AutoPlay_Page8To9", commonUtils.data.IntroVideo_AutoRun_Page8To9);
        }
    }

    void AutoPlay_Page8To9()
    {
        TransitionAni_Page8To9();
    }

    void TransitionAni_Page8To9()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            currStage = IntroVideoStage.Transition_Page8To9;
            characterGrp2.AlphaAni(1, 0.5f);
            ClearDialog();
            yield return new WaitForSeconds(0.2f);
            currStage = IntroVideoStage.Page9;
            ShowDialog(commonUtils.introVideoDialogs[8]);
            characterGrp2.Play();
            Invoke("AutoPlay_Page9To10", commonUtils.data.IntroVideo_AutoRun_Page9To10);
        }
    }

    void AutoPlay_Page9To10()
    {
        TransitionAni_Page9To10();
    }

    void TransitionAni_Page9To10()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            SoundManager.instance.Play_BGM(4, 4);
            currStage = IntroVideoStage.Transition_Page9To10;
            ClearDialog();
            yield return new WaitForEndOfFrame();
            //screen cap page 9 and pixelate
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();
            pixelateImg1.texture = tex;
            pixelateImg1.DOFade(1f, 0.8f);
            pixelateImg1.material.DOFloat(200f, "_PixelateSize", 0.8f).From(512f).SetEase(Ease.Linear);
            SoundManager.instance.Play_SFX(11);
            yield return new WaitForSeconds(0.8f);
            //fade in pixelate game lab scene
            pixelateImg2.texture = labBlackBarTexture;
            pixelateImg2.material.SetFloat("_PixelateSize", 200f);
            pixelateImg2.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            //depixelate lab
            labObj.AlphaAni(0, 0);
            mapObj.AlphaAni(0, 0);
            eggObj.AlphaAni(0, 0);
            bossObj.AlphaAni(0, 0);
            characterGrp1.AlphaAni(0, 0);
            characterGrp2.AlphaAni(0, 0);
            blackBarObj.AlphaAni(1f, 0f);
            pixelateImg1.DOFade(0f, 0f);
            pixelateImg2.material.DOFloat(512f, "_PixelateSize", 0.8f).From(200f).SetEase(Ease.Linear);
            pixelateImg2.DOFade(0f, 0.8f);
            yield return new WaitForSeconds(0.5f);
            ClearDialog();
            blackBarObj.Play();
        }
    }

    private void BlackBar_OnVideoEnd()
    {
        labObj.AlphaAni(0, 0);
        labObj.ResetAll();
        mapObj.AlphaAni(0, 0);
        mapObj.ResetAll();
        eggObj.AlphaAni(0, 0);
        eggObj.ResetAll();
        bossObj.AlphaAni(0, 0);
        bossObj.ResetAll();
        characterGrp1.AlphaAni(0, 0);
        characterGrp1.ResetAll();
        characterGrp2.AlphaAni(0, 0);
        characterGrp2.ResetAll();
        blackBarObj.AlphaAni(0, 0);
        blackBarObj.ResetAll();
        root.SetActive(false);
        if (onVideoFinishedCallback != null)
        {
            onVideoFinishedCallback.Invoke();
        }
    }

    //-------

    public void ShowDialog(ConfigData_DialogBox dialogBox)
    {
        if (commonUtils.currLang == Language.TC)
        {
            dialogWriterSingle = DialogWriter.AddWriter_Static(text_TC, dialogBox.Text_TC, commonUtils.data.DialogBox_TimePerCharacter_TC, true, OnDialogLineEnd);
        }
        else if(commonUtils.currLang == Language.SC)
        {
            dialogWriterSingle = DialogWriter.AddWriter_Static(text_SC, dialogBox.Text_SC, commonUtils.data.DialogBox_TimePerCharacter_SC, true, OnDialogLineEnd);
        }
        else if (commonUtils.currLang == Language.EN)
        {
            dialogWriterSingle = DialogWriter.AddWriter_Static(text_EN, dialogBox.Text_EN, commonUtils.data.DialogBox_TimePerCharacter_EN, true, OnDialogLineEnd);
        }
        confirmBtnControl.SetAlpha(1, 0);
    }

    void OnDialogLineEnd()
    {
        
    }

    void FinishCurrentDialog()
    {
        dialogWriterSingle.WriteAllAndDestroy();
    }

    void ClearDialog()
    {
        text_TC.text = "";
        text_SC.text = "";
        text_EN.text = "";
        confirmBtnControl.SetAlpha(0, 0);
    }

    private void Update()
    {
        //hotkey skip intro
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BlackBar_OnVideoEnd();
        }
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.onValueChanged_ConfirmCallback -= InputManager_OnValueChanged_Confirm;
        }
        if (labObj != null)
        {
            labObj.onTransitionStartDoneCallback -= Lab_OnTransitionStartDone;
        }
        if (mapObj != null)
        {
            mapObj.onTransitionStartDoneCallback -= Map_OnTransitionStartDone;
        }
        if (blackBarObj != null)
        {
            blackBarObj.onVideoEndCallback -= BlackBar_OnVideoEnd;
        }
    }
}
