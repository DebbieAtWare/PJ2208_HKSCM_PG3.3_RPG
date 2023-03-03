using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TmpDebug : MonoBehaviour
{
    public static TmpDebug instance;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("TmpDebug Awake");
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Text text;

    private void Update()
    {
        text.text = "isTimeoutUIActive: " + TimeoutManager.instance.isTimeoutUIActive +
            "  lang: " + CommonUtils.instance.currLang +
            "  currMainStage: " + MainManger.instance.currStage +
            "  talkHintObj: " + DroneController.instance.talkHintObj.activeInHierarchy +
            "  currDroneStage: " + DroneController.instance.currDroneStage;
    }
}
