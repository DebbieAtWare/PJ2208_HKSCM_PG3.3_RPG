using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManger : MonoBehaviour
{
    public static MainManger instance;

    CommonUtils commonUtils;

    void Awake()
    {
        Debug.Log("MainManger Awake");
        if (instance != null)
        {
            Debug.Log("More than one instance of MainManger");
            return;
        }
        instance = this;
    }

    void Start()
    {
        commonUtils = CommonUtils.instance;
        commonUtils.onSetupDoneCallback += CommonUtils_OnSetupDone;
        commonUtils.Setup();
    }

    private void CommonUtils_OnSetupDone()
    {
        //tmp for demo
        SceneManager.LoadScene("CarboniferousScene");
    }
}