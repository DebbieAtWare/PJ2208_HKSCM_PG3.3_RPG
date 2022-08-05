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

    [Header("Curr")]
    public bool isFirstMeetDone;
    public bool isCollectable;
    public string name_TC;
    public string name_SC;
    public string name_EN;
    public List<ConfigData_DialogBox> dialogBoxes = new List<ConfigData_DialogBox>();
    public int currDialogLine;
    public bool isAtFirstTrigger;

    public void Setup(string _name_TC, string _name_SC, string _name_EN, bool _isCollectable, List<ConfigData_DialogBox> _dialogBoxes)
    {
        firstTriggerControl.onTriggerEnterCallback += FirstTrigger_OnEnter;
        firstTriggerControl.onTriggerExitCallback += FirstTrigger_OnExit;

        name_TC = _name_TC;
        name_SC = _name_SC;
        name_EN = _name_EN;
        isCollectable = _isCollectable;
        dialogBoxes = _dialogBoxes;

        currDialogLine = 0;
        isAtFirstTrigger = false;

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
        if (Input.GetButtonDown("RPGConfirmPC") && isAtFirstTrigger)
        {
            if (currDialogLine == 0)
            {
                GameManager.instance.dialogActive = true;
                DialogBoxManager.instance.ShowDialog(dialogBoxes[currDialogLine].Text_TC);
                ViewBoxManager.instance.HideViewBox();
                currDialogLine++;
                
                arrowObj_Green.SetActive(false);
                arrowObj_Grey.SetActive(false);
            }
            else if (currDialogLine == dialogBoxes.Count)
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
                DialogBoxManager.instance.ShowDialog(dialogBoxes[currDialogLine].Text_TC);
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
