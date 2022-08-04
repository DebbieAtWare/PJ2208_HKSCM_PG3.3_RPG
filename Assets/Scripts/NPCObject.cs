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

    private string[] dialogLines;
    private int currDialogLine;
    private bool isAtFirstTrigger;

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

        dialogLines = new String[3];
        dialogLines[0] = "hi1";
        dialogLines[1] = "hi2";
        dialogLines[2] = "hi3";
    }

    private void FirstTrigger_OnEnter()
    {
        //Debug.Log("FirstTrigger_OnEnter");
        ViewBoxManager.instance.ShowViewBox();
        isAtFirstTrigger = true;
    }

    private void FirstTrigger_OnExit()
    {
        //Debug.Log("FirstTrigger_OnExit");
        ViewBoxManager.instance.HideViewBox();
        isAtFirstTrigger = false;
    }

    private void UpdateRun()
    {
        if (Input.GetButtonDown("RPGConfirmPC") && isAtFirstTrigger)
        {
            if (currDialogLine == 0)
            {
                DialogBoxManager.instance.ShowDialog(dialogLines[currDialogLine]);
                ViewBoxManager.instance.HideViewBox();
                currDialogLine++;
            }
            else if (currDialogLine == dialogLines.Length)
            {
                DialogBoxManager.instance.HideDialog();
                currDialogLine = 0;
            }
            else
            {
                DialogBoxManager.instance.ShowDialog(dialogLines[currDialogLine]);
                currDialogLine++;
            }
        }
    }

    private void OnDestroy()
    {
        firstTriggerControl.onTriggerEnterCallback -= FirstTrigger_OnEnter;
        firstTriggerControl.onTriggerExitCallback -= FirstTrigger_OnExit;
    }
}
