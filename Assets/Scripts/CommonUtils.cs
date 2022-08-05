using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapID
{
    Carboniferous,
    Permian
}

public enum CharacterID
{
    AVA,
    DRO,
    M01,
    M02,
    M03,
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

    public delegate void OnSetupDone();
    public OnSetupDone onSetupDoneCallback;

    [Header("NPC")]
    public List<ConfigData_NPC> NPC_Carboniferous = new List<ConfigData_NPC>();

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

    void Start()
    {
        ConfigData_NPC npc_1 = new ConfigData_NPC();
        npc_1.Id = "NPC_C01";
        npc_1.Name_TC = "鱗木屬";
        npc_1.IsCollectable = true;
        ConfigData_DialogBox dialog_11 = new ConfigData_DialogBox();
        dialog_11.ByWhom = "DRO";
        dialog_11.ImagePath = "";
        dialog_11.Text_TC = "看看這個是鱗木屬呀!";
        npc_1.DialogBoxes.Add(dialog_11);
        ConfigData_DialogBox dialog_12 = new ConfigData_DialogBox();
        dialog_12.ByWhom = "DRO";
        dialog_12.ImagePath = "";
        dialog_12.Text_TC = "鱗木是一屬已滅絕的石松類，為一類高大的樹狀蕨類，生長在炎熱潮濕的沼澤地帶中。";
        npc_1.DialogBoxes.Add(dialog_12);
        NPC_Carboniferous.Add(npc_1);

        ConfigData_NPC npc_2 = new ConfigData_NPC();
        npc_2.Id = "NPC_C02";
        npc_2.Name_TC = "科達樹";
        npc_2.IsCollectable = true;
        ConfigData_DialogBox dialog_21 = new ConfigData_DialogBox();
        dialog_21.ByWhom = "AVA";
        dialog_21.ImagePath = "";
        dialog_21.Text_TC = "這些裸子植物中長得真高…";
        npc_2.DialogBoxes.Add(dialog_21);
        ConfigData_DialogBox dialog_22 = new ConfigData_DialogBox();
        dialog_22.ByWhom = "DRO";
        dialog_22.ImagePath = "";
        dialog_22.Text_TC = "科達樹是形成煤炭的重要植物，很多煤炭都是由石炭紀植物形成，這個地質時期因此稱為「石炭紀」。";
        npc_2.DialogBoxes.Add(dialog_22);
        NPC_Carboniferous.Add(npc_2);

        if (onSetupDoneCallback != null)
        {
            onSetupDoneCallback.Invoke();
        }
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

