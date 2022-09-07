using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DroneStage
{
    None,
    Tips,
    Hint,
    CollectionBook,
    ChangeMap
}

public class DroneController : MonoBehaviour
{
    public static DroneController instance;

    [Header("Trigger")]
    public OnTriggerControl onTriggerControl;

    [Header("Talk Hint")]
    public GameObject talkHintObj;
    public bool canShowTalkHint;

    [Header("Follow")]
    public float speed;
    public float stopDist;

    [Header("Teleport")]
    public TeleportTo teleportTo_Permian;
    public TeleportTo teleportTo_Carboniferous;

    [Header("Curr")]
    public DroneStage currDroneStage;
    public int currSelectedOption = 0;
    public int currDialogLine_Hint = 0;

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

    void Start()
    {
        commonUtils = CommonUtils.instance;

        inputManager = InputManager.instance;
        inputManager.onValueChanged_VerticalCallback += InputManager_OnValueChanged_Vertical;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;

        onTriggerControl.onTriggerEnterCallback += OnTriggerEnter;
        onTriggerControl.onTriggerExitCallback += OnTriggerExit;

        canShowTalkHint = true;
        talkHintObj.SetActive(false);

        currDroneStage = DroneStage.None;
    }

    //close drone talk hint when inside NPC first trigger
    public void HideTalkHint()
    {
        talkHintObj.SetActive(false);
    }

    //when user leave NPC first trigger and inside drone trigger
    public void ShowTalkHint()
    {
        if (isAtTrigger)
        {
            talkHintObj.SetActive(true);
        }
    }

    private void OnTriggerEnter()
    {
        isAtTrigger = true;
        if (canShowTalkHint)
        {
            talkHintObj.SetActive(true);
        }
    }

    private void OnTriggerExit()
    {
        isAtTrigger = false;
        talkHintObj.SetActive(false);
    }

    //-----

    private void InputManager_OnValueChanged_Vertical(int val)
    {
        if (currDroneStage == DroneStage.Tips)
        {
            if (currSelectedOption == 0)
            {
                if (val == -1)
                {
                    currSelectedOption = 1;
                    DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                }
            }
            else if (currSelectedOption == 1)
            {
                if (val == 1)
                {
                    currSelectedOption = 0;
                    DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                }
                else if (val == -1)
                {
                    currSelectedOption = 2;
                    DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                }
            }
            else if (currSelectedOption == 2)
            {
                if (val == 1)
                {
                    currSelectedOption = 1;
                    DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                }
            }
        }
        else if (currDroneStage == DroneStage.CollectionBook || currDroneStage == DroneStage.ChangeMap)
        {
            if (currSelectedOption == 0)
            {
                if (val == -1)
                {
                    currSelectedOption = 1;
                    DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                }
            }
            else if (currSelectedOption == 1)
            {
                if (val == 1)
                {
                    currSelectedOption = 0;
                    DialogBoxManager.instance.SetOptionArrow(currSelectedOption);
                }
            }
        }
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (currDroneStage == DroneStage.None && talkHintObj.activeInHierarchy)
        {
            GameManager.instance.dialogActive = true;
            DialogBoxManager.instance.ShowDialog(commonUtils.dialogBox_TipsByDrone);
            currDroneStage = DroneStage.Tips;
        }
        else if (currDroneStage == DroneStage.Tips)
        {
            if (currSelectedOption == 0)
            {
                currDroneStage = DroneStage.Hint;
                DialogBoxManager.instance.ShowDialog(commonUtils.dialogBox_TipsByDrone_Hints[currDialogLine_Hint]);
                currDialogLine_Hint++;
            }
            else if (currSelectedOption == 1)
            {
                currDroneStage = DroneStage.CollectionBook;
                DialogBoxManager.instance.ShowDialog(commonUtils.dialogBox_TipsByDrone_CollectionBook);
                currSelectedOption = 0;
            }
            else if (currSelectedOption == 2)
            {
                currDroneStage = DroneStage.ChangeMap;
                DialogBoxManager.instance.ShowDialog(commonUtils.dialogBox_TipsByDrone_ChangeMap);
                currSelectedOption = 0;
            }
        }
        else if (currDroneStage == DroneStage.Hint)
        {
            if (currDialogLine_Hint == commonUtils.dialogBox_TipsByDrone_Hints.Count)
            {
                DialogBoxManager.instance.HideDialog();
                currDialogLine_Hint = 0;
                currSelectedOption = 0;
                currDroneStage = DroneStage.None;
                GameManager.instance.dialogActive = false;
            }
            else
            {
                DialogBoxManager.instance.ShowDialog(commonUtils.dialogBox_TipsByDrone_Hints[currDialogLine_Hint]);
                currDialogLine_Hint++;
            }
        }
        else if (currDroneStage == DroneStage.CollectionBook)
        {
            if (currSelectedOption == 0)
            {
                DialogBoxManager.instance.HideDialog();
                //TODO open collection book
            }
            else if (currSelectedOption == 1)
            {
                DialogBoxManager.instance.HideDialog();
                currDialogLine_Hint = 0;
                currSelectedOption = 0;
                currDroneStage = DroneStage.None;
                GameManager.instance.dialogActive = false;
            }
        }
        else if (currDroneStage == DroneStage.ChangeMap)
        {
            if (currSelectedOption == 0)
            {
                DialogBoxManager.instance.HideDialog();
                currDialogLine_Hint = 0;
                currSelectedOption = 0;
                currDroneStage = DroneStage.None;
                GameManager.instance.dialogActive = false;
                if (SceneManager.GetActiveScene().name == "CarboniferousScene")
                {
                    StartCoroutine(wait());
                    
                    IEnumerator wait()
                    {
                        TransitionManager.instance.StartTransition();
                        yield return new WaitForSeconds(1f);
                        teleportTo_Permian.ManualTeleport();
                    }
                }
                else if (SceneManager.GetActiveScene().name == "PermianScene")
                {
                    StartCoroutine(wait());

                    IEnumerator wait()
                    {
                        TransitionManager.instance.StartTransition();
                        yield return new WaitForSeconds(1f);
                        teleportTo_Carboniferous.ManualTeleport();
                    }
                    
                }
            }
            else if (currSelectedOption == 1)
            {
                DialogBoxManager.instance.HideDialog();
                currDialogLine_Hint = 0;
                currSelectedOption = 0;
                currDroneStage = DroneStage.None;
                GameManager.instance.dialogActive = false;
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

    public void SharpChangePos()
    {
        if (CommonEvents.instance.facePlayerLeft)
        {
            transform.position = new Vector3(PlayerController.instance.transform.position.x + stopDist,
                                                                      PlayerController.instance.transform.position.y,
                                                                      PlayerController.instance.transform.position.z);
        }
        else if (CommonEvents.instance.facePlayerRight)
        {
            transform.position = new Vector3(PlayerController.instance.transform.position.x - stopDist,
                                                    PlayerController.instance.transform.position.y,
                                                    PlayerController.instance.transform.position.z);
        }
        else if (CommonEvents.instance.facePlayerUp)
        {
            transform.position = new Vector3(PlayerController.instance.transform.position.x,
                                                    PlayerController.instance.transform.position.y - stopDist,
                                                    PlayerController.instance.transform.position.z);
        }
        else if (CommonEvents.instance.facePlayerDown)
        {
            transform.position = new Vector3(PlayerController.instance.transform.position.x,
                                                    PlayerController.instance.transform.position.y + stopDist,
                                                    PlayerController.instance.transform.position.z);
        }
    }

    private void OnDestroy()
    {
        inputManager.onValueChanged_VerticalCallback -= InputManager_OnValueChanged_Vertical;
    }
}
