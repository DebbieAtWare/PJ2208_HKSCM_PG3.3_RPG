using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPCObject : MonoBehaviour
{
    [Header("Id")]
    public CharacterID id;

    [Header("FirstTrigger")]
    public OnTriggerControl firstTriggerControl;

    [Header("Arrow")]
    public GameObject arrowObj_Green;
    public GameObject arrowObj_Grey;

    [Header("Renderer")]
    public Renderer npcRenderer;

    [Header("Collection Book Thumbnail")]
    public Sprite collectionBookThumbnailSprite;

    [Header("Info")]
    public ConfigData_Character info;

    [Header("Curr")]
    public int currDialogLine;
    public bool isAtFirstTrigger;
    public bool isInSuccessCollectMode;

    CommonUtils commonUtils;

    public void Setup(ConfigData_Character _info)
    {
        commonUtils = CommonUtils.instance;

        info = _info;

        firstTriggerControl.onTriggerEnterCallback += FirstTrigger_OnEnter;
        firstTriggerControl.onTriggerExitCallback += FirstTrigger_OnExit;

        currDialogLine = 0;
        isAtFirstTrigger = false;
        isInSuccessCollectMode = false;

        OutlineControl();

        //if (info.IsFirstMeetDone)
        //{
        //    arrowObj_Green.SetActive(false);
        //    arrowObj_Grey.SetActive(true);
        //}
        //else
        //{
        //    arrowObj_Green.SetActive(true);
        //    arrowObj_Grey.SetActive(false);
        //}
    }

    void OutlineControl()
    {
        StartCoroutine(OutlineAni());
    }

    IEnumerator OutlineAni()
    {
        npcRenderer.material.DOFloat(1, "_OutlineAlpha", 1.5f);
        yield return new WaitForSeconds(1.8f);
        npcRenderer.material.DOFloat(0.7f, "_OutlineAlpha", 1.5f);
        yield return new WaitForSeconds(1.8f);
        Invoke("OutlineControl", 0f);
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
        if (Input.GetButtonDown("RPGConfirmPC") && isAtFirstTrigger && !isInSuccessCollectMode)
        {
            if (currDialogLine == 0)
            {
                GameManager.instance.dialogActive = true;
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                ViewBoxManager.instance.HideViewBox();
                currDialogLine++;
            }
            else if (currDialogLine == info.DialogBoxes.Count)
            {
                DialogBoxManager.instance.HideDialog();
                currDialogLine = 0;
                //arrowObj_Green.SetActive(false);
                //arrowObj_Grey.SetActive(true);
                if (info.IsCollectable)
                {
                    if (!info.IsFirstMeetDone)
                    {
                        isInSuccessCollectMode = true;
                        CollectionBookManager.instance.ShowSuccessCollect(info.Name_TC, collectionBookThumbnailSprite);
                        Invoke("CloseSuccessCollect", 2f);
                    }
                    else
                    {
                        GameManager.instance.dialogActive = false;
                    }
                }
                else
                {
                    info.IsFirstMeetDone = true;
                    GameManager.instance.dialogActive = false;
                }
            }
            else
            {
                DialogBoxManager.instance.ShowDialog(info.DialogBoxes[currDialogLine]);
                currDialogLine++;
            }
        }
    }

    void CloseSuccessCollect()
    {
        CollectionBookManager.instance.HideSuccessCollect();
        info.IsFirstMeetDone = true;
        info.IsSuccessCollectDone = true;
        GameManager.instance.dialogActive = false;
        isInSuccessCollectMode = false;
    }

    private void OnDestroy()
    {
        firstTriggerControl.onTriggerEnterCallback -= FirstTrigger_OnEnter;
        firstTriggerControl.onTriggerExitCallback -= FirstTrigger_OnExit;
    }
}
