using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Follow")]
    public float speed;
    public float stopDist;

    [Header("Curr")]
    public DroneStage currDroneStage;
    public int currSelectedOption = 0;

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
        commonUtils.NPCAtFirstTrigger_OnEnterCallback += CommonUtils_NPCAtFirstTirgger_OnEnter;
        commonUtils.NPCAtFirstTrigger_OnExitCallback += CommonUtils_NPCAtFirstTrigger_OnExit;

        inputManager = InputManager.instance;
        inputManager.onValueChanged_VerticalCallback += InputManager_OnValueChanged_Vertical;
        inputManager.onValueChanged_ConfirmCallback += InputManager_OnValueChanged_Confirm;

        onTriggerControl.onTriggerEnterCallback += OnTriggerEnter;
        onTriggerControl.onTriggerExitCallback += OnTriggerExit;
        talkHintObj.SetActive(false);

        currDroneStage = DroneStage.None;
    }

    //close drone talk hint when inside NPC first trigger
    private void CommonUtils_NPCAtFirstTirgger_OnEnter()
    {
        talkHintObj.SetActive(false);
    }

    //when user leave NPC first trigger and inside drone trigger
    private void CommonUtils_NPCAtFirstTrigger_OnExit()
    {
        if (isAtTrigger)
        {
            talkHintObj.SetActive(true);
        }
    }

    private void OnTriggerEnter()
    {
        isAtTrigger = true;
        if (!commonUtils.isAtNPCFirstTrigger)
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
    }

    private void InputManager_OnValueChanged_Confirm()
    {
        if (currDroneStage == DroneStage.None && talkHintObj.activeInHierarchy)
        {
            GameManager.instance.dialogActive = true;
            DialogBoxManager.instance.ShowDialog(commonUtils.dialogBox_TipsByDrone);
            currDroneStage = DroneStage.Tips;
        }
        else if (currDroneStage == DroneStage.Tips && currSelectedOption == 0)
        {
            currDroneStage = DroneStage.Hint;
        }
        else if (currDroneStage == DroneStage.Tips && currSelectedOption == 1)
        {
            currDroneStage = DroneStage.CollectionBook;
        }
        else if (currDroneStage == DroneStage.Tips && currSelectedOption == 2)
        {
            currDroneStage = DroneStage.ChangeMap;
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
        commonUtils.NPCAtFirstTrigger_OnEnterCallback -= CommonUtils_NPCAtFirstTirgger_OnEnter;
        commonUtils.NPCAtFirstTrigger_OnExitCallback -= CommonUtils_NPCAtFirstTrigger_OnExit;
        inputManager.onValueChanged_VerticalCallback -= InputManager_OnValueChanged_Vertical;
    }
}
