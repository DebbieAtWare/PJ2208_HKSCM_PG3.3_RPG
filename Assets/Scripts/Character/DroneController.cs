﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DroneStage
{
    None,
    Main_Question,
    Main_Option,
    Hint,
    CollectionBook_Question,
    CollectionBook_Option,
    ChangeMap_Question,
    ChangeMap_Option
}

public class DroneController : MonoBehaviour
{
    public static DroneController instance;

    [Header("Id")]
    public CharacterID id;

    [Header("Sprite")]
    public Sprite dialogBoxProfileSprite;

    [Header("Ani")]
    public Animator animator;

    [Header("Trigger")]
    public OnTriggerControl onTriggerControl;

    [Header("Talk Hint")]
    public GameObject talkHintObj;
    public bool canShowTalkHint;

    [Header("Follow")]
    public float speed;
    public float stopDist;

    [Header("Curr")]
    public DroneStage currDroneStage;
    public int currSelectedOption = 0;
    public int currDialogLine_Hint = -1;


    CommonUtils commonUtils;
    InputManager inputManager;
    bool isAtTrigger;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Setup()
    {
        commonUtils = CommonUtils.instance;

        inputManager = InputManager.instance;
        inputManager.onValueChanged_VerticalCallback += InputManager_OnValueChanged_Vertical;
        inputManager.onValueChanged_HorizontalCallback += InputManager_OnValueChanged_Horizontal;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;

        onTriggerControl.onTriggerEnterCallback += OnTriggerEnter;
        onTriggerControl.onTriggerExitCallback += OnTriggerExit;

        currSelectedOption = 0;
        currDialogLine_Hint = -1;
        canShowTalkHint = true;
        talkHintObj.SetActive(false);

        animator.SetTrigger("Idle");


        currDroneStage = DroneStage.None;
    }

    //when inside NPC first trigger
    //when change map
    public void HideTalkHint()
    {
        talkHintObj.SetActive(false);
        ViewBoxManager.instance.HideViewBox_Drone();
    }

    //when user leave NPC first trigger and inside drone trigger
    //after change map and finish greeting msg
    public void ShowTalkHint()
    {
        if (isAtTrigger)
        {
            talkHintObj.SetActive(true);
            ViewBoxManager.instance.ShowViewBox_Drone();
        }
    }

    public void ForceShowTalkHint()
    {
        talkHintObj.SetActive(true);
        ViewBoxManager.instance.HideViewBox_Drone();
    }

    private void OnTriggerEnter()
    {
        isAtTrigger = true;
        if (canShowTalkHint)
        {
            talkHintObj.SetActive(true);
            if (commonUtils.currMapId != MapID.Lab)
            {
                ViewBoxManager.instance.ShowViewBox_Drone();
            }
        }
    }

    private void OnTriggerExit()
    {
        isAtTrigger = false;
        talkHintObj.SetActive(false);
        ViewBoxManager.instance.HideViewBox_Drone();
    }

    //-----

    private void InputManager_OnValueChanged_Vertical(int val)
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
        {
            if (talkHintObj.activeInHierarchy && MainManger.instance.currStage == MainStage.InGame && OptionManager.instance.currStage == OptionStage.None)
            {
                if (currDroneStage == DroneStage.Main_Option)
                {
                    if (currSelectedOption == 0)
                    {
                        if (val == 1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 3;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                        else if (val == -1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 1;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                    }
                    else if (currSelectedOption == 1)
                    {
                        if (val == 1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 0;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                        else if (val == -1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 2;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                    }
                    else if (currSelectedOption == 2)
                    {
                        if (val == 1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 1;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                        else if (val == -1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 3;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                    }
                    else if (currSelectedOption == 3)
                    {
                        if (val == 1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 2;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                        else if (val == -1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 0;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                    }
                }
                else if (currDroneStage == DroneStage.CollectionBook_Option || currDroneStage == DroneStage.ChangeMap_Option)
                {
                    if (currSelectedOption == 0)
                    {
                        if (val == -1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 1;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                    }
                    else if (currSelectedOption == 1)
                    {
                        if (val == 1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 0;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                    }
                }
            }
        }
    }

    private void InputManager_OnValueChanged_Horizontal(int val)
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
        {
            if (talkHintObj.activeInHierarchy && MainManger.instance.currStage == MainStage.InGame && OptionManager.instance.currStage == OptionStage.None)
            {
                if (currDroneStage == DroneStage.Main_Option)
                {
                    if (currSelectedOption == 0)
                    {
                        if (val == -1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 2;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                        else if (val == 1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 2;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                    }
                    else if (currSelectedOption == 1)
                    {
                        if (val == -1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 3;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                        else if (val == 1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 3;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                    }
                    else if (currSelectedOption == 2)
                    {
                        if (val == -1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 0;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                        else if (val == 1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 0;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                    }
                    else if (currSelectedOption == 3)
                    {
                        if (val == -1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 1;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                        else if (val == 1)
                        {
                            SoundManager.instance.Play_Input(0);
                            currSelectedOption = 1;
                            DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                        }
                    }
                }
            }
        }

        
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (!TimeoutManager.instance.isTimeoutUIActive)
        {
            if (talkHintObj.activeInHierarchy && MainManger.instance.currStage == MainStage.InGame)
            {
                if (currDroneStage == DroneStage.None)
                {
                    //有甚麼需要幫忙的嗎？ question only
                    currDroneStage = DroneStage.Main_Question;
                    SoundManager.instance.Play_Input(2);
                    GameManager.instance.dialogActive = true;
                    DialogBoxManager.instance.ShowDialog(commonUtils.dialogBox_TipsByDrone);
                }
                else if (currDroneStage == DroneStage.Main_Question)
                {
                    SoundManager.instance.Play_Input(2);
                    if (DialogBoxManager.instance.dialogWriterSingle != null && DialogBoxManager.instance.dialogWriterSingle.IsActive())
                    {
                        DialogBoxManager.instance.FinishCurrentDialog();
                    }
                    else
                    {
                        DialogBoxManager.instance.ShowOption(commonUtils.dialogBox_TipsByDrone);
                        currDroneStage = DroneStage.Main_Option;
                    }
                }
                else if (currDroneStage == DroneStage.Main_Option)
                {
                    if (currSelectedOption == 0)
                    {
                        SoundManager.instance.Play_Input(2);
                        currDroneStage = DroneStage.Hint;
                        currDialogLine_Hint++;
                        DialogBoxManager.instance.ShowDialog(commonUtils.dialogBox_TipsByDrone_Hints[currDialogLine_Hint]);
                    }
                    else if (currSelectedOption == 1)
                    {
                        SoundManager.instance.Play_Input(2);
                        currDroneStage = DroneStage.CollectionBook_Question;
                        DialogBoxManager.instance.ShowDialog(commonUtils.dialogBox_TipsByDrone_CollectionBook);
                        currSelectedOption = 0;
                    }
                    else if (currSelectedOption == 2)
                    {
                        SoundManager.instance.Play_Input(2);
                        currDroneStage = DroneStage.ChangeMap_Question;
                        DialogBoxManager.instance.ShowDialog(commonUtils.dialogBox_TipsByDrone_ChangeMap);
                        currSelectedOption = 0;
                    }
                    else if (currSelectedOption == 3)
                    {
                        SoundManager.instance.Play_Input(2);
                        currDroneStage = DroneStage.None;
                        DialogBoxManager.instance.HideDialog();
                        currSelectedOption = 0;
                        GameManager.instance.dialogActive = false;
                    }
                }
                else if (currDroneStage == DroneStage.Hint)
                {
                    SoundManager.instance.Play_Input(2);

                    if (DialogBoxManager.instance.dialogWriterSingle.IsActive())
                    {
                        DialogBoxManager.instance.FinishCurrentDialog();
                    }
                    else
                    {
                        if (currDialogLine_Hint == (commonUtils.dialogBox_TipsByDrone_Hints.Count - 1))
                        {
                            DialogBoxManager.instance.HideDialog();
                            currDialogLine_Hint = -1;
                            currSelectedOption = 0;
                            currDroneStage = DroneStage.None;
                            GameManager.instance.dialogActive = false;
                        }
                        else
                        {
                            currDialogLine_Hint++;
                            DialogBoxManager.instance.ShowDialog(commonUtils.dialogBox_TipsByDrone_Hints[currDialogLine_Hint]);
                        }
                    }
                }
                else if (currDroneStage == DroneStage.CollectionBook_Question)
                {
                    SoundManager.instance.Play_Input(2);
                    if (DialogBoxManager.instance.dialogWriterSingle.IsActive())
                    {
                        DialogBoxManager.instance.FinishCurrentDialog();
                    }
                    else
                    {
                        DialogBoxManager.instance.ShowOption(commonUtils.dialogBox_TipsByDrone_CollectionBook);
                        currDroneStage = DroneStage.CollectionBook_Option;
                    }
                }
                else if (currDroneStage == DroneStage.CollectionBook_Option)
                {
                    if (currSelectedOption == 0)
                    {
                        SoundManager.instance.Play_Input(2);
                        DialogBoxManager.instance.HideDialog();
                        CollectionBookManager.instance.Show_Main();
                        HideTalkHint();
                        canShowTalkHint = false;
                        currDroneStage = DroneStage.None;
                        GameManager.instance.dialogActive = true;
                    }
                    else if (currSelectedOption == 1)
                    {
                        SoundManager.instance.Play_Input(2);
                        DialogBoxManager.instance.HideDialog();
                        currDialogLine_Hint = -1;
                        currSelectedOption = 0;
                        currDroneStage = DroneStage.None;
                        GameManager.instance.dialogActive = false;
                    }
                }
                else if (currDroneStage == DroneStage.ChangeMap_Question)
                {
                    SoundManager.instance.Play_Input(2);
                    if (DialogBoxManager.instance.dialogWriterSingle.IsActive())
                    {
                        DialogBoxManager.instance.FinishCurrentDialog();
                    }
                    else
                    {
                        DialogBoxManager.instance.ShowOption(commonUtils.dialogBox_TipsByDrone_ChangeMap);
                        currDroneStage = DroneStage.ChangeMap_Option;
                    }
                }
                else if (currDroneStage == DroneStage.ChangeMap_Option)
                {
                    if (currSelectedOption == 0)
                    {
                        SoundManager.instance.Play_Input(2);
                        DialogBoxManager.instance.HideDialog();
                        currDialogLine_Hint = -1;
                        currSelectedOption = 0;
                        currDroneStage = DroneStage.None;
                        GameManager.instance.dialogActive = false;
                        if (commonUtils.currMapId == MapID.Carboniferous)
                        {
                            TransitionManager.instance.ChangeMap(commonUtils.currMapId, MapID.Permian);
                        }
                        else if (commonUtils.currMapId == MapID.Permian)
                        {
                            TransitionManager.instance.ChangeMap(commonUtils.currMapId, MapID.Carboniferous);
                        }

                    }
                    else if (currSelectedOption == 1)
                    {
                        SoundManager.instance.Play_Input(2);
                        DialogBoxManager.instance.HideDialog();
                        currDialogLine_Hint = -1;
                        currSelectedOption = 0;
                        currDroneStage = DroneStage.None;
                        GameManager.instance.dialogActive = false;
                    }
                }
            }
        }

        
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) > stopDist)
        {
            transform.position = LeapEase(transform.position, PlayerController.instance.transform.position, speed);
            //transform.position = Vector2.MoveTowards(transform.position, PlayerController.instance.transform.position, speed * Time.fixedDeltaTime);
        }
    }

    Vector2 LeapEase(Vector2 current, Vector2 target, float ease)
    {
        return current + (target - current) * ease;
    }

    public void ChangePos_FollowStopDist(PlayerDirection dir)
    {
        if (dir == PlayerDirection.Left)
        {
            transform.position = new Vector3(PlayerController.instance.transform.position.x + stopDist,
                                                                      PlayerController.instance.transform.position.y,
                                                                      PlayerController.instance.transform.position.z);
        }
        else if (dir == PlayerDirection.Right)
        {
            transform.position = new Vector3(PlayerController.instance.transform.position.x - stopDist,
                                                    PlayerController.instance.transform.position.y,
                                                    PlayerController.instance.transform.position.z);
        }
        else if (dir == PlayerDirection.Up)
        {
            transform.position = new Vector3(PlayerController.instance.transform.position.x,
                                                    PlayerController.instance.transform.position.y - stopDist,
                                                    PlayerController.instance.transform.position.z);
        }
        else if (dir == PlayerDirection.Down)
        {
            transform.position = new Vector3(PlayerController.instance.transform.position.x,
                                                    PlayerController.instance.transform.position.y + stopDist,
                                                    PlayerController.instance.transform.position.z);
        }
    }

    public void ChangePos(Vector3 pos)
    {
        transform.position = pos;
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.onValueChanged_VerticalCallback -= InputManager_OnValueChanged_Vertical;
            inputManager.onValueChanged_HorizontalCallback -= InputManager_OnValueChanged_Horizontal;
            inputManager.onValueChanged_ConfirmCallback -= InputManager_OnValueChanged_Confirm;
        }

        if (onTriggerControl != null)
        {
            onTriggerControl.onTriggerEnterCallback -= OnTriggerEnter;
            onTriggerControl.onTriggerExitCallback -= OnTriggerExit;
        }
    }
}
