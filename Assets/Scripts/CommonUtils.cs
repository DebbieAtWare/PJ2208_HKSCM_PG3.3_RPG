using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapID
{
    Carboniferous,
    Permian
}

public enum NPCID
{
    NPC_C01,
    NPC_C02,
    NPC_C03,
    NPC_C04,
    NPC_C05,
    NPC_C06,
    NPC_P01,
    NPC_P02,
    NPC_P03,
    NPC_P04,
    NPC_P05,
    NPC_P06,
    NPC_P07,
    NPC_P08,
    NPC_P09,
    NPC_P10,
    NPC_P11,
    NPC_P12,
}

public class CommonUtils : MonoBehaviour
{
    public static CommonUtils instance;

    [Header("Curr")]
    public MapID currMapId;

    //for one scene
    void Awake()
    {
        Debug.Log("CommonUtils Awake");
        if (instance != null)
        {
            Debug.Log("More than one instance of CommonUtils");
            return;
        }
        instance = this;
    }
}

[Serializable]
public class ConfigData_NPC
{
    public string Id;
    public string Name_TC;
    public string Name_SC;
    public string Name_EN;
    public bool IsCollectable;
    public List<ConfigData_DialogBox> DialogBoxes = new List<ConfigData_DialogBox>();
}

[Serializable]
public class ConfigData_DialogBox
{
    public string ByWhom;
    public string ImagePath;
    public string Text_TC;
    public string Text_SC;
    public string Text_EN;
}

