using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObject : MonoBehaviour
{
    [Header("Id")]
    public CharacterID id;

    [Header("FirstTrigger")]
    public OnTriggerControl firstTriggerControl;

    [Header("Arrow")]
    public GameObject arrowObj_Green;
    public GameObject arrowObj_Grey;

    [Header("Info")]
    public ConfigData_Character info;

    [Header("Curr")]
    public bool isFirstMeetDone;
    public int currDialogLine;
    public bool isAtFirstTrigger;
    public bool isAtSuccessCollect;

    public void Setup(ConfigData_Character _info)
    {
        info = _info;

        firstTriggerControl.onTriggerEnterCallback += FirstTrigger_OnEnter;
        firstTriggerControl.onTriggerExitCallback += FirstTrigger_OnExit;

        currDialogLine = 0;
        isAtFirstTrigger = false;
        isAtSuccessCollect = false;

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

    public void UpdateRun()
    {
        if (Input.GetButtonDown("RPGConfirmPC") && isAtFirstTrigger && !isAtSuccessCollect)
        {
            if (currDialogLine == 0)
            {
                GameManager.instance.dialogActive = true;
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine].Text_TC);
                ViewBoxManager.instance.HideViewBox();
                currDialogLine++;
                
                arrowObj_Green.SetActive(false);
                arrowObj_Grey.SetActive(false);
            }
            else if (currDialogLine == info.DialogBoxes.Count)
            {
                DialogBoxManager.instance.HideDialog();
                currDialogLine = 0;
                arrowObj_Green.SetActive(false);
                arrowObj_Grey.SetActive(true);
                if (info.IsCollectable)
                {
                    if (!isFirstMeetDone)
                    {
                        isAtSuccessCollect = true;
                        CollectionBookManager.instance.ShowSuccessCollect(info.Name_TC);
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
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine].Text_TC);
                currDialogLine++;
            }
        }
    }

    void CloseSuccessCollect()
    {
        CollectionBookManager.instance.HideSuccessCollect();
        isFirstMeetDone = true;
        GameManager.instance.dialogActive = false;
        isAtSuccessCollect = false;
    }

    private void OnDestroy()
    {
        firstTriggerControl.onTriggerEnterCallback -= FirstTrigger_OnEnter;
        firstTriggerControl.onTriggerExitCallback -= FirstTrigger_OnExit;
    }
}
