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
    public List<ConfigData_Character> NPC_Carboniferous = new List<ConfigData_Character>();

    [Header("Boss")]
    public ConfigData_DialogBox dialogBox_BossAlert;
    public ConfigData_Character Boss01;


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
        ConfigData_Character npc_1 = new ConfigData_Character();
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

        ConfigData_Character npc_2 = new ConfigData_Character();
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

        ConfigData_Character npc_3 = new ConfigData_Character();
        npc_3.Id = "NPC_C03";
        npc_3.Name_TC = "節胸屬";
        npc_3.IsCollectable = true;
        ConfigData_DialogBox dialog_31 = new ConfigData_DialogBox();
        dialog_31.ByWhom = "DRO";
        dialog_31.ImagePath = "";
        dialog_31.Text_TC = "看看這個是節胸屬呀!";
        npc_3.DialogBoxes.Add(dialog_31);
        ConfigData_DialogBox dialog_32 = new ConfigData_DialogBox();
        dialog_32.ByWhom = "DRO";
        dialog_32.ImagePath = "";
        dialog_32.Text_TC = "節胸屬，又稱節胸蜈蚣屬，是史前的倍足綱動物，即現今蜈蚣及馬陸的遠古親屬。";
        npc_3.DialogBoxes.Add(dialog_32);
        NPC_Carboniferous.Add(npc_3);

        ConfigData_Character npc_4 = new ConfigData_Character();
        npc_4.Id = "NPC_C04";
        npc_4.Name_TC = "巨脈蜻蜓";
        npc_4.IsCollectable = true;
        ConfigData_DialogBox dialog_41 = new ConfigData_DialogBox();
        dialog_41.ByWhom = "DRO";
        dialog_41.ImagePath = "";
        dialog_41.Text_TC = "看看這個是巨脈蜻蜓呀!";
        npc_4.DialogBoxes.Add(dialog_41);
        ConfigData_DialogBox dialog_42 = new ConfigData_DialogBox();
        dialog_42.ByWhom = "DRO";
        dialog_42.ImagePath = "";
        dialog_42.Text_TC = "巨脈蜻蜓，又名大尾蜻蜓或巨尾蜻蜓，是3億年前石炭紀一種已滅絕的昆蟲。";
        npc_4.DialogBoxes.Add(dialog_42);
        NPC_Carboniferous.Add(npc_4);


        ConfigData_Character npc_5 = new ConfigData_Character();
        npc_5.Id = "NPC_C05";
        npc_5.Name_TC = "芬氏彼得足螈";
        npc_5.IsCollectable = true;
        ConfigData_DialogBox dialog_51 = new ConfigData_DialogBox();
        dialog_51.ByWhom = "DRO";
        dialog_51.ImagePath = "";
        dialog_51.Text_TC = "看看這個是芬氏彼得足螈呀!";
        npc_5.DialogBoxes.Add(dialog_51);
        ConfigData_DialogBox dialog_52 = new ConfigData_DialogBox();
        dialog_52.ByWhom = "DRO";
        dialog_52.ImagePath = "";
        dialog_52.Text_TC = "芬氏彼得足螈是早石炭紀四足動物的一個已滅絕屬。";
        npc_5.DialogBoxes.Add(dialog_52);
        NPC_Carboniferous.Add(npc_5);

        ConfigData_Character npc_6 = new ConfigData_Character();
        npc_6.Id = "NPC_C06";
        npc_6.Name_TC = "引螈屬";
        npc_6.IsCollectable = true;
        ConfigData_DialogBox dialog_61 = new ConfigData_DialogBox();
        dialog_61.ByWhom = "DRO";
        dialog_61.ImagePath = "";
        dialog_61.Text_TC = "看看這個是引螈屬呀!";
        npc_6.DialogBoxes.Add(dialog_61);
        ConfigData_DialogBox dialog_62 = new ConfigData_DialogBox();
        dialog_62.ByWhom = "DRO";
        dialog_62.ImagePath = "";
        dialog_62.Text_TC = "引螈是石炭紀、二疊紀探索上最大的動物。體長1.8米以上。";
        npc_6.DialogBoxes.Add(dialog_62);
        NPC_Carboniferous.Add(npc_6);

        //----

        ConfigData_DialogBox dialog = new ConfigData_DialogBox();
        dialog.ByWhom = "DRO";
        dialog.ImagePath = "";
        dialog.Text_TC = "機械人嘅直覺話比我知即將有新發現。快啲過去睇吓啦。";
        dialogBox_BossAlert = dialog;

        ConfigData_Character boss1 = new ConfigData_Character();
        boss1.Id = "M01";
        boss1.Name_TC = "林蜥屬";
        boss1.DescriptionTag_TC = "體積：約20厘米";
        boss1.IsCollectable = true;
        ConfigData_DialogBox dialog_m11 = new ConfigData_DialogBox();
        dialog_m11.ByWhom = "M01";
        dialog_m11.ImagePath = "";
        dialog_m11.Text_TC = "你好呀！我就係林蜥屬喇！";
        boss1.DialogBoxes.Add(dialog_m11);
        ConfigData_DialogBox dialog_m12 = new ConfigData_DialogBox();
        dialog_m12.ByWhom = "AVA";
        dialog_m12.ImagePath = "";
        dialog_m12.Text_TC = "你好呀！我係從未來嚟嘅科學家！大概花了半生的運氣才能遇到你，真幸運！你看起來很威猛呢。";
        boss1.DialogBoxes.Add(dialog_m12);
        ConfigData_DialogBox dialog_m13 = new ConfigData_DialogBox();
        dialog_m13.ByWhom = "M01";
        dialog_m13.ImagePath = "";
        dialog_m13.Text_TC = "我擁有銳利的小型牙齒，以早期昆蟲為生。";
        boss1.DialogBoxes.Add(dialog_m13);
        ConfigData_DialogBox dialog_m14 = new ConfigData_DialogBox();
        dialog_m14.ByWhom = "M01";
        dialog_m14.ImagePath = "";
        dialog_m14.Text_TC = "好似唔小心就講咗好多關於自己嘅嘢，有啲唔好意思，下次有機會再傾多啲。<br>你快啲去繼續探索吓啦，仲有好多新奇有趣嘅事物等緊你！";
        boss1.DialogBoxes.Add(dialog_m14);
        Boss01 = boss1;

        if (onSetupDoneCallback != null)
        {
            onSetupDoneCallback.Invoke();
        }
    }
}

[Serializable]
public class ConfigData_Character
{
    public string Id;
    public string Name_TC;
    public string Name_SC;
    public string Name_EN;
    public string DescriptionTag_TC;
    public string DescriptionTag_SC;
    public string DescriptionTag_EN;
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

