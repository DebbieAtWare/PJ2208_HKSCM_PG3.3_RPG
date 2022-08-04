using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObject : MonoBehaviour
{
    [Header("Id")]
    public NPCID id;

    [Header("FirstTrigger")]
    public OnTriggerControl firstTriggerControl;
    public DialogStarter dialogStarter;

    //Check wheather the player is in range to talk to npc
    private bool canActivate;

    private void Start()
    {
        Setup();
    }
    private void Update()
    {
        UpdateRun();
    }




    public void Setup()
    {
        firstTriggerControl.onTriggerEnterCallback += FirstTrigger_OnEnter;
        firstTriggerControl.onTriggerExitCallback += FirstTrigger_OnExit;

        dialogStarter.lines = new String[3];
        dialogStarter.lines[0] = "//dff";
        dialogStarter.lines[1] = "hi1";
        dialogStarter.lines[2] = "sdfsdf";
    }

    private void FirstTrigger_OnEnter()
    {
        //Debug.Log("FirstTrigger_OnEnter");
        ViewBoxManager.instance.ShowViewBox();
        canActivate = true;
    }

    private void FirstTrigger_OnExit()
    {
        //Debug.Log("FirstTrigger_OnExit");
        ViewBoxManager.instance.HideViewBox();
        canActivate = false;
    }

    private void UpdateRun()
    {
        if (Input.GetButtonDown("RPGConfirmPC") && canActivate && DialogManager.instance.dialogBox.activeInHierarchy)
        {
            ViewBoxManager.instance.HideViewBox();
        }
    }

    private void OnDestroy()
    {
        firstTriggerControl.onTriggerEnterCallback -= FirstTrigger_OnEnter;
        firstTriggerControl.onTriggerExitCallback -= FirstTrigger_OnExit;
    }
}
