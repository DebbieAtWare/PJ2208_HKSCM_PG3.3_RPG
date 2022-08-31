using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public static DroneController instance;

    [Header("Trigger")]
    public OnTriggerControl onTriggerControl;

    [Header("Follow")]
    public float speed;
    public float stopDist;

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
        onTriggerControl.onTriggerEnterCallback += OnTriggerEnter;
        onTriggerControl.onTriggerExitCallback += OnTriggerExit;
    }

    private void OnTriggerEnter()
    {
        Debug.Log("Drone OnTriggerEnter");
    }

    private void OnTriggerExit()
    {
        Debug.Log("Drone OnTriggerExit");
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) > stopDist)
        {
            transform.position = LeapEase(transform.position, PlayerController.instance.transform.position, speed);
        }

        //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    Vector2 LeapEase(Vector2 current, Vector2 target, float ease)
    {
        return current + (target - current) * ease;
    }

    public void SharpChangePos()
    {
        if (CommonEvents.instance.facePlayerLeft)
        {
            Debug.Log("facePlayerLeftfacePlayerLeft1   " + transform.position + "   " + PlayerController.instance.transform.position);
            transform.position = new Vector3(PlayerController.instance.transform.position.x + stopDist,
                                                                      PlayerController.instance.transform.position.y,
                                                                      PlayerController.instance.transform.position.z);
            Debug.Log("facePlayerLeftfacePlayerLeft3   " + transform.position + "   " + PlayerController.instance.transform.position);
        }
        else if (CommonEvents.instance.facePlayerRight)
        {
            Debug.Log("facePlayerRightfacePlayerRight1   " + transform.position + "   " + PlayerController.instance.transform.position);
            transform.position = new Vector3(PlayerController.instance.transform.position.x - stopDist,
                                                    PlayerController.instance.transform.position.y,
                                                    PlayerController.instance.transform.position.z);
            Debug.Log("facePlayerRightfacePlayerRight2   " + transform.position + "   " + PlayerController.instance.transform.position);
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
}
