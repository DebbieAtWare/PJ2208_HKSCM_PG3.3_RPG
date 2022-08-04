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

    [Header("Arrow")]
    public GameObject arrowObj_Green;
    public GameObject arrowObj_Grey;

    [Header("Curr")]
    public bool isFirstMeetDone;
    public bool isCollectable;
    public string name_TC;
    public string name_SC;
    public string name_EN;

    private string[] dialogLines;
    private int currDialogLine;
    private bool isAtFirstTrigger;

    private void Start()
    {
        Setup("鱗木屬", true);
    }
    private void Update()
    {
        UpdateRun();
    }




    public void Setup(string _name_TC, bool _isCollectable)
    {
        firstTriggerControl.onTriggerEnterCallback += FirstTrigger_OnEnter;
        firstTriggerControl.onTriggerExitCallback += FirstTrigger_OnExit;

        isCollectable = _isCollectable;
        name_TC = _name_TC;

        dialogLines = new String[3];
        dialogLines[0] = "hi1";
        dialogLines[1] = "hi2";
        dialogLines[2] = "hi3";

        arrowObj_Green.SetActive(true);
        arrowObj_Grey.SetActive(false);
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
                GameManager.instance.dialogActive = true;
                DialogBoxManager.instance.ShowDialog(dialogLines[currDialogLine]);
                ViewBoxManager.instance.HideViewBox();
                currDialogLine++;
                
                arrowObj_Green.SetActive(false);
                arrowObj_Grey.SetActive(false);
            }
            else if (currDialogLine == dialogLines.Length)
            {
                DialogBoxManager.instance.HideDialog();
                currDialogLine = 0;
                arrowObj_Green.SetActive(false);
                arrowObj_Grey.SetActive(true);
                if (isCollectable)
                {
                    if (!isFirstMeetDone)
                    {
                        CollectionBookManager.instance.ShowSuccessCollect(name_TC);
                        Invoke("CloseSuccessCollect", 2f);
                    }
                    else
                    {
                        GameManager.instance.dialogActive = false;
                    }
                }
                else
                {
                    isFirstMeetDone = true;
                    GameManager.instance.dialogActive = false;
                }
            }
            else
            {
                DialogBoxManager.instance.ShowDialog(dialogLines[currDialogLine]);
                currDialogLine++;
            }
        }
    }

    void CloseSuccessCollect()
    {
        CollectionBookManager.instance.HideSuccessCollect();
        isFirstMeetDone = true;
        GameManager.instance.dialogActive = false;
    }

    private void OnDestroy()
    {
        firstTriggerControl.onTriggerEnterCallback -= FirstTrigger_OnEnter;
        firstTriggerControl.onTriggerExitCallback -= FirstTrigger_OnExit;
    }
}
