using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Profile Pic")]
    public Sprite profilePicSprite_Avatar;
    public Sprite profilePicSprite_Drone;
    public Sprite profilePicSprite_Boss01;
    public Sprite profilePicSprite_Boss02;
    public Sprite profilePicSprite_Boss03;

    [Header("ConfigData")]
    public List<ConfigData_Character> NPC_Carboniferous = new List<ConfigData_Character>();
    public List<ConfigData_Character> NPC_Permian = new List<ConfigData_Character>();
    public ConfigData_DialogBox dialogBox_BossAlert;
    public List<ConfigData_Character> bosses = new List<ConfigData_Character>();

    [Header("Curr")]
    public MapID currMapId;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("CommonUtils Awake");
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
        Debug.Log("CommonUtils Start");

        SceneManager.sceneLoaded += OnSceneLoaded;

        TmpExcelControl();

        //-----


        if (onSetupDoneCallback != null)
        {
            onSetupDoneCallback.Invoke();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("!!CommonUtils OnSceneLoaded " + scene.name + "  " + loadSceneMode.ToString());

        if (scene.name == "MainScene")
        {

        }
        else if (scene.name == "CarboniferousScene")
        {
            CarboniferousManager.instance.Setup();
        }
        else if (scene.name == "Boss01Scene")
        {
            Boss01Manager.instance.Setup();
        }
        else if (scene.name == "PermianScene")
        {
            PermianManager.instance.Setup();
        }
    }

    void TmpExcelControl()
    {
        ConfigData_Character npc_1 = new ConfigData_Character();
        npc_1.Id = "NPC_C01";
        npc_1.Name_TC = "鱗木屬";
        npc_1.IsCollectable = false;
        npc_1.IsFirstMeetDone = false;
        npc_1.IsSuccessCollectDone = false;
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
        npc_2.IsCollectable = false;
        npc_2.IsFirstMeetDone = false;
        npc_2.IsSuccessCollectDone = false;
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
        npc_3.IsCollectable = false;
        npc_3.IsFirstMeetDone = false;
        npc_3.IsSuccessCollectDone = false;
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
        npc_4.IsCollectable = false;
        npc_4.IsFirstMeetDone = false;
        npc_4.IsSuccessCollectDone = false;
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
        npc_5.IsCollectable = false;
        npc_5.IsFirstMeetDone = false;
        npc_5.IsSuccessCollectDone = false;
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
        npc_6.IsCollectable = false;
        npc_6.IsFirstMeetDone = false;
        npc_6.IsSuccessCollectDone = false;
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
        boss1.IsFirstMeetDone = false;
        boss1.IsSuccessCollectDone = false;
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
        dialog_m13.ImagePath = "Images/SupportImage/Hylonomus_SupportImg2";
        dialog_m13.Text_TC = "我身長大約20厘米長，而且外表應該相當接近現代蜥蜴。";
        boss1.DialogBoxes.Add(dialog_m13);
        ConfigData_DialogBox dialog_m131 = new ConfigData_DialogBox();
        dialog_m131.ByWhom = "M01";
        dialog_m131.ImagePath = "Images/SupportImage/Hylonomus_SupportImg1";
        dialog_m131.Text_TC = "我擁有銳利的小型牙齒，以早期昆蟲為生。";
        boss1.DialogBoxes.Add(dialog_m131);
        ConfigData_DialogBox dialog_m132 = new ConfigData_DialogBox();
        dialog_m132.ByWhom = "M01";
        dialog_m132.ImagePath = "";
        dialog_m132.Text_TC = "我的化石是發現於加拿大新斯科細亞。";
        boss1.DialogBoxes.Add(dialog_m132);
        ConfigData_DialogBox dialog_m14 = new ConfigData_DialogBox();
        dialog_m14.ByWhom = "M01";
        dialog_m14.ImagePath = "";
        dialog_m14.Text_TC = "好似唔小心就講咗好多關於自己嘅嘢，有啲唔好意思，下次有機會再傾多啲。";
        boss1.DialogBoxes.Add(dialog_m14);
        ConfigData_DialogBox dialog_m15 = new ConfigData_DialogBox();
        dialog_m15.ByWhom = "M01";
        dialog_m15.ImagePath = "";
        dialog_m15.Text_TC = "你快啲去繼續探索吓啦，仲有好多新奇有趣嘅事物等緊你！";
        boss1.DialogBoxes.Add(dialog_m15);
        bosses.Add(boss1);

        //----

        ConfigData_Character npc_p1 = new ConfigData_Character();
        npc_p1.Id = "NPC_P01";
        npc_p1.Name_TC = "科達樹";
        npc_p1.IsCollectable = false;
        npc_p1.IsFirstMeetDone = false;
        npc_p1.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p11 = new ConfigData_DialogBox();
        dialog_p11.ByWhom = "DRO";
        dialog_p11.ImagePath = "";
        dialog_p11.Text_TC = "看看這個是科達樹呀!";
        npc_p1.DialogBoxes.Add(dialog_p11);
        ConfigData_DialogBox dialog_p12 = new ConfigData_DialogBox();
        dialog_p12.ByWhom = "DRO";
        dialog_p12.ImagePath = "";
        dialog_p12.Text_TC = "科達樹是形成煤炭的重要植物，很多煤炭都是由石炭紀植物形成，這個地質時期因此稱為「石炭紀」。";
        npc_p1.DialogBoxes.Add(dialog_p12);
        NPC_Permian.Add(npc_p1);

        ConfigData_Character npc_p2 = new ConfigData_Character();
        npc_p2.Id = "NPC_P02";
        npc_p2.Name_TC = "節胸屬";
        npc_p2.IsCollectable = false;
        npc_p2.IsFirstMeetDone = false;
        npc_p2.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p21 = new ConfigData_DialogBox();
        dialog_p21.ByWhom = "DRO";
        dialog_p21.ImagePath = "";
        dialog_p21.Text_TC = "看看這個是節胸屬呀!";
        npc_p2.DialogBoxes.Add(dialog_p21);
        ConfigData_DialogBox dialog_p22 = new ConfigData_DialogBox();
        dialog_p22.ByWhom = "DRO";
        dialog_p22.ImagePath = "";
        dialog_p22.Text_TC = "節胸屬在石炭紀才是全盛時期。";
        npc_p2.DialogBoxes.Add(dialog_p22);
        NPC_Permian.Add(npc_p2);

        ConfigData_Character npc_p3 = new ConfigData_Character();
        npc_p3.Id = "NPC_P03";
        npc_p3.Name_TC = "巨脈蜻蜓";
        npc_p3.IsCollectable = false;
        npc_p3.IsFirstMeetDone = false;
        npc_p3.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p31 = new ConfigData_DialogBox();
        dialog_p31.ByWhom = "DRO";
        dialog_p31.ImagePath = "";
        dialog_p31.Text_TC = "看看這個是巨脈蜻蜓呀!";
        npc_p3.DialogBoxes.Add(dialog_p31);
        ConfigData_DialogBox dialog_p32 = new ConfigData_DialogBox();
        dialog_p32.ByWhom = "DRO";
        dialog_p32.ImagePath = "";
        dialog_p32.Text_TC = "巨脈蜻蜓在石炭紀才是全盛時期。";
        npc_p3.DialogBoxes.Add(dialog_p32);
        NPC_Permian.Add(npc_p3);

        ConfigData_Character npc_p4 = new ConfigData_Character();
        npc_p4.Id = "NPC_P04";
        npc_p4.Name_TC = "銀杏目";
        npc_p4.IsCollectable = false;
        npc_p4.IsFirstMeetDone = false;
        npc_p4.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p41 = new ConfigData_DialogBox();
        dialog_p41.ByWhom = "DRO";
        dialog_p41.ImagePath = "";
        dialog_p41.Text_TC = "看看這個是銀杏目呀!";
        npc_p4.DialogBoxes.Add(dialog_p41);
        ConfigData_DialogBox dialog_p42 = new ConfigData_DialogBox();
        dialog_p42.ByWhom = "DRO";
        dialog_p42.ImagePath = "";
        dialog_p42.Text_TC = "銀杏類植物為高大多枝落葉喬木、具有挺拔的樹幹與獨特的扇形葉片。";
        npc_p4.DialogBoxes.Add(dialog_p42);
        NPC_Permian.Add(npc_p4);
    }
}

[Serializable]
public class ConfigData_Character
{
    //for excel
    public string Id;
    public string Name_TC;
    public string Name_SC;
    public string Name_EN;
    public string DescriptionTag_TC;
    public string DescriptionTag_SC;
    public string DescriptionTag_EN;
    public bool IsCollectable;
    public List<ConfigData_DialogBox> DialogBoxes = new List<ConfigData_DialogBox>();

    //for program
    public bool IsFirstMeetDone;
    public bool IsSuccessCollectDone;
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

