using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBoxManager : MonoBehaviour
{
    public static ViewBoxManager instance;

    public GameObject npcObj;
    public GameObject droneObj;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("ViewBoxManager Awake");
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

    public void Setup()
    {
        HideViewBox_NPC();
        HideViewBox_Drone();
    }

    public void ShowViewBox_NPC()
    {
        npcObj.SetActive(true);
    }

    public void HideViewBox_NPC()
    {
        npcObj.SetActive(false);
    }

    public void ShowViewBox_Drone()
    {
        droneObj.SetActive(true);
    }

    public void HideViewBox_Drone()
    {
        droneObj.SetActive(false);
    }
}
