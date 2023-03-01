using System;
using System.Collections;
using System.Collections.Generic;
using Timers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TimeoutManager : MonoBehaviour
{
    public static TimeoutManager instance;

    public GameObject root;
    public TextMeshProUGUI timerText_TC;
    public TextMeshProUGUI timerText_SC;
    public TextMeshProUGUI timerText_EN;

    public List<GameObject> arrowObjs = new List<GameObject>();
    public int currArrowIndex = 0;

    public List<GameObject> langObjs_TC;
    public List<GameObject> langObjs_SC;
    public List<GameObject> langObjs_EN;

    public bool isTimeoutUIActive;

    float displayUITimer;
    float displayUITimerTarget;
    float countdownUITimerTarget;

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

        displayUITimerTarget = CommonUtils.instance.data.TimeOut_DisplayUITimerTarget;
        countdownUITimerTarget = CommonUtils.instance.data.TimeOut_CountdownUITimerTarget;

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
        timerText_TC.text = countdownUITimerTarget.ToString();
        timerText_SC.text = countdownUITimerTarget.ToString();
        timerText_EN.text = countdownUITimerTarget.ToString();
        root.SetActive(false);
        isTimeoutUIActive = false;
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
        if (displayUITimer > displayUITimerTarget)
        {
            InvokeUI();
        }
        else
        {
            displayUITimer += Time.deltaTime;
        }
    }

    void InvokeUI()
    {
        if (!CommonUtils.instance.isAtHomePage)
        {
            //make it invoke only once
            if (!isTimeoutUIActive)
            {
                isTimeoutUIActive = true;
                root.SetActive(true);
                ChangeLanaguage();
                countdownUITimer = countdownUITimerTarget;
                InvokeRepeating("TimerTextControl", 1, 1);
            }
        }
    }

    float countdownUITimer;
    void TimerTextControl()
    {
        countdownUITimer--;
        timerText_TC.text = countdownUITimer.ToString();
        timerText_SC.text = countdownUITimer.ToString();
        timerText_EN.text = countdownUITimer.ToString();
        if (countdownUITimer == 0)
        {
            CommonUtils.instance.ResetGame();
        }
    }

    void ResetAll()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            CancelInvoke("TimerTextControl");
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
            timerText_TC.text = countdownUITimerTarget.ToString();
            timerText_SC.text = countdownUITimerTarget.ToString();
            timerText_EN.text = countdownUITimerTarget.ToString();
            root.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            displayUITimer = 0;
            isTimeoutUIActive = false;
        }
    }

    private void InputManager_Option()
    {
        displayUITimer = 0;
    }

    private void InputManager_Horizontal(int val)
    {
        displayUITimer = 0;
    }

    private void InputManager_Vertical(int val)
    {
        displayUITimer = 0;

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
        displayUITimer = 0;

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
