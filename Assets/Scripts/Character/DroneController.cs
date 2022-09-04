using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    CommonUtils commonUtils;
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

        onTriggerControl.onTriggerEnterCallback += OnTriggerEnter;
        onTriggerControl.onTriggerExitCallback += OnTriggerExit;
        talkHintObj.SetActive(false);
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

    void LateUpdate()
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
    }
}
