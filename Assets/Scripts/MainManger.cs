using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManger : MonoBehaviour
{
    public static MainManger instance;

    CommonUtils commonUtils;
    InputManager inputManager;

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

        inputManager = InputManager.instance;
        inputManager.canInput_Vertical = true;
        inputManager.canInput_Horizontal = true;
        inputManager.canInput_Confirm = true;
    }
    
    private void CommonUtils_OnSetupDone()
    {
        //tmp for demo
        SceneManager.LoadScene("CarboniferousScene");
        //SceneManager.LoadScene("PermianScene");
    }
}
