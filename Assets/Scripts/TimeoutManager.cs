using System;
using System.Collections;
using System.Collections.Generic;
using Timers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimeoutManager : MonoBehaviour
{
    public static TimeoutManager instance;

    public GameObject root;
    public Image timerImg;

    public List<GameObject> arrowObjs = new List<GameObject>();
    public int currArrowIndex = 0;

    public List<GameObject> langObjs_TC;
    public List<GameObject> langObjs_SC;
    public List<GameObject> langObjs_EN;

    public bool isTimeoutUIActive;

    public float tmp;

    float displayUITimerTarget = 18000000;
    float countdownUITimerTarget = 30;

    InputManager inputManager;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("TimeoutManager Awake");
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
        inputManager = InputManager.instance;
        inputManager.onValueChanged_OptionCallback += InputManager_Option;
        inputManager.onValueChanged_HorizontalCallback += InputManager_Horizontal;
        inputManager.onValueChanged_VerticalCallback += InputManager_Vertical;
        inputManager.onValueChanged_ConfirmCallback += InputManager_Confirm;

        currArrowIndex = 0;
        for (int i = 0; i < arrowObjs.Count; i++)
        {
            if (i == currArrowIndex)
            {
                arrowObjs[i].SetActive(true);
            }
            else
            {
                arrowObjs[i].SetActive(false);
            }
        }
        timerImg.fillAmount = 1;
        root.SetActive(false);
        isTimeoutUIActive = false;

        TimersManager.SetTimer(this, displayUITimerTarget, InvokeUI);
    }

    void ChangeLanaguage()
    {
        if (CommonUtils.instance.currLang == Language.TC)
        {
            for (int i = 0; i < langObjs_TC.Count; i++)
            {
                langObjs_TC[i].SetActive(true);
            }
            for (int i = 0; i < langObjs_SC.Count; i++)
            {
                langObjs_SC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_EN.Count; i++)
            {
                langObjs_EN[i].SetActive(false);
            }
        }
        else if (CommonUtils.instance.currLang == Language.SC)
        {
            for (int i = 0; i < langObjs_TC.Count; i++)
            {
                langObjs_TC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_SC.Count; i++)
            {
                langObjs_SC[i].SetActive(true);
            }
            for (int i = 0; i < langObjs_EN.Count; i++)
            {
                langObjs_EN[i].SetActive(false);
            }
        }
        else if (CommonUtils.instance.currLang == Language.EN)
        {
            for (int i = 0; i < langObjs_TC.Count; i++)
            {
                langObjs_TC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_SC.Count; i++)
            {
                langObjs_SC[i].SetActive(false);
            }
            for (int i = 0; i < langObjs_EN.Count; i++)
            {
                langObjs_EN[i].SetActive(true);
            }
        }
    }

    private void Update()
    {
        tmp = TimersManager.RemainingTime(InvokeUI);

        if (!TimersManager.IsTimerActive(InvokeUI) && !root.activeInHierarchy)
        {
            TimersManager.SetTimer(this, displayUITimerTarget, InvokeUI);
        }
    }

    void InvokeUI()
    {
        TimersManager.ClearTimer(InvokeUI);
        if (!CommonUtils.instance.isAtHomePage)
        {
            isTimeoutUIActive = true;
            root.SetActive(true);
            ChangeLanaguage();
            timerImg.DOFillAmount(0, countdownUITimerTarget).From(1).SetEase(Ease.Linear).OnComplete(CountdownUICompleted);
        }
    }

    void CountdownUICompleted()
    {
        Debug.Log("CountdownUICompleted");
        CommonUtils.instance.ResetGame();
    }

    void ResetAll()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            timerImg.DOKill();
            currArrowIndex = 0;
            for (int i = 0; i < arrowObjs.Count; i++)
            {
                if (i == currArrowIndex)
                {
                    arrowObjs[i].SetActive(true);
                }
                else
                {
                    arrowObjs[i].SetActive(false);
                }
            }
            timerImg.fillAmount = 1;
            root.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            isTimeoutUIActive = false;
        }
    }

    private void InputManager_Option()
    {
        TimersManager.ClearTimer(InvokeUI);
    }

    private void InputManager_Horizontal(int val)
    {
        TimersManager.ClearTimer(InvokeUI);
    }

    private void InputManager_Vertical(int val)
    {
        TimersManager.ClearTimer(InvokeUI);

        if (root.activeInHierarchy)
        {
            SoundManager.instance.Play_Input(0);
            if (currArrowIndex == 0)
            {
                if (val == -1)
                {
                    arrowObjs[currArrowIndex].SetActive(false);
                    currArrowIndex = 1;
                    arrowObjs[currArrowIndex].SetActive(true);
                }
            }
            else if (currArrowIndex == 1)
            {
                if (val == 1)
                {
                    arrowObjs[currArrowIndex].SetActive(false);
                    currArrowIndex = 0;
                    arrowObjs[currArrowIndex].SetActive(true);
                }
            }
        }
    }

    private void InputManager_Confirm()
    {
        TimersManager.ClearTimer(InvokeUI);

        if (root.activeInHierarchy)
        {
            SoundManager.instance.Play_Input(2);
            if (currArrowIndex == 0)
            {
                ResetAll();
            }
            else if (currArrowIndex == 1)
            {
                CommonUtils.instance.ResetGame();
            }
        }
    }
}
