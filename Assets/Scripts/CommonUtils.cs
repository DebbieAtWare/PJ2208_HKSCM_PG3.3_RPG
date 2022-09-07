﻿using System;
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

    [Header("PlayerPos")]
    public Vector3 playerPos_Carboniferous;
    public Vector3 playerPos_Permian;

    [Header("PlayerDir")]
    public PlayerDirection playerDir_Carboniferous;
    public PlayerDirection playerDir_Permian;

    [Header("DronePos")]
    public Vector3 dronePos_Carboniferous;
    public Vector3 dronePos_Permian;

    [Header("ConfigData")]
    public List<ConfigData_Character> NPC_Carboniferous = new List<ConfigData_Character>();
    public List<ConfigData_Character> NPC_Permian = new List<ConfigData_Character>();
    public ConfigData_DialogBox dialogBox_BossAlert;
    public List<ConfigData_Character> bosses = new List<ConfigData_Character>();
    public ConfigData_DialogBox dialogBox_TipsByDrone;
    public List<ConfigData_DialogBox> dialogBox_TipsByDrone_Hints = new List<ConfigData_DialogBox>();
    public ConfigData_DialogBox dialogBox_TipsByDrone_CollectionBook;
    public ConfigData_DialogBox dialogBox_TipsByDrone_ChangeMap;

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

    //--------

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
        dialog_m11.Text_TC = "你好呀！我就係林蜥喇！";
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

        ConfigData_Character boss2 = new ConfigData_Character();
        boss2.Id = "M02";
        boss2.Name_TC = "異齒龍屬";
        boss2.DescriptionTag_TC = "體積：約3.5米";
        boss2.IsCollectable = true;
        boss2.IsFirstMeetDone = false;
        boss2.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_m21 = new ConfigData_DialogBox();
        dialog_m21.ByWhom = "M02";
        dialog_m21.ImagePath = "";
        dialog_m21.Text_TC = "你好呀！我就係異齒龍喇！";
        boss2.DialogBoxes.Add(dialog_m21);
        ConfigData_DialogBox dialog_m22 = new ConfigData_DialogBox();
        dialog_m22.ByWhom = "AVA";
        dialog_m22.ImagePath = "";
        dialog_m22.Text_TC = "你好呀！我係從未來嚟嘅科學家！大概花了半生的運氣才能遇到你，真幸運！你看起來很威猛呢。";
        boss2.DialogBoxes.Add(dialog_m22);
        ConfigData_DialogBox dialog_m23 = new ConfigData_DialogBox();
        dialog_m23.ByWhom = "M02";
        dialog_m23.ImagePath = "";
        dialog_m23.Text_TC = "我是肉食性合弓動物的一屬，生存於二疊紀。";
        boss2.DialogBoxes.Add(dialog_m23);
        ConfigData_DialogBox dialog_m231 = new ConfigData_DialogBox();
        dialog_m231.ByWhom = "M02";
        dialog_m231.ImagePath = "Images/SupportImage/Dimetrodon_SupportImg1";
        dialog_m231.Text_TC = "我的大型頭顱骨中有兩種不同型態的牙齒，切割用的牙齒與銳利的犬齒。我以往側邊攤開的四肢及大型尾巴來支撐身體。";
        boss2.DialogBoxes.Add(dialog_m231);
        ConfigData_DialogBox dialog_m232 = new ConfigData_DialogBox();
        dialog_m232.ByWhom = "M02";
        dialog_m232.ImagePath = "Images/SupportImage/Dimetrodon_SupportImg2";
        dialog_m232.Text_TC = "我最明顯的特徵是背部的高大背帆，可用來控制體溫。背帆的表面可使加熱、冷卻更有效率。";
        boss2.DialogBoxes.Add(dialog_m232);
        ConfigData_DialogBox dialog_m24 = new ConfigData_DialogBox();
        dialog_m24.ByWhom = "M02";
        dialog_m24.ImagePath = "";
        dialog_m24.Text_TC = "好似唔小心就講咗好多關於自己嘅嘢，有啲唔好意思，下次有機會再傾多啲。";
        boss2.DialogBoxes.Add(dialog_m24);
        ConfigData_DialogBox dialog_m25 = new ConfigData_DialogBox();
        dialog_m25.ByWhom = "M02";
        dialog_m25.ImagePath = "";
        dialog_m25.Text_TC = "你快啲去繼續探索吓啦，仲有好多新奇有趣嘅事物等緊你！";
        boss2.DialogBoxes.Add(dialog_m25);
        bosses.Add(boss2);

        ConfigData_Character boss3 = new ConfigData_Character();
        boss3.Id = "M03";
        boss3.Name_TC = "水龍獸屬";
        boss3.DescriptionTag_TC = "體積：約0.9米";
        boss3.IsCollectable = true;
        boss3.IsFirstMeetDone = false;
        boss3.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_m31 = new ConfigData_DialogBox();
        dialog_m31.ByWhom = "M03";
        dialog_m31.ImagePath = "";
        dialog_m31.Text_TC = "你好呀！我就係水龍獸喇！";
        boss3.DialogBoxes.Add(dialog_m31);
        ConfigData_DialogBox dialog_m32 = new ConfigData_DialogBox();
        dialog_m32.ByWhom = "AVA";
        dialog_m32.ImagePath = "";
        dialog_m32.Text_TC = "你好呀！我係從未來嚟嘅科學家！大概花了半生的運氣才能遇到你，真幸運！你看起來很威猛呢。";
        boss3.DialogBoxes.Add(dialog_m32);
        ConfigData_DialogBox dialog_m33 = new ConfigData_DialogBox();
        dialog_m33.ByWhom = "M03";
        dialog_m33.ImagePath = "";
        dialog_m33.Text_TC = "我是種常見的合弓動物，屬於獸孔目二齒獸下目，體型接近豬，身長約0.9米，重量約90公斤。";
        boss3.DialogBoxes.Add(dialog_m33);
        ConfigData_DialogBox dialog_m331 = new ConfigData_DialogBox();
        dialog_m331.ByWhom = "M03";
        dialog_m331.ImagePath = "Images/SupportImage/Lystrosaurus_SupportImg1";
        dialog_m331.Text_TC = "與其他獸孔目不同，我的口鼻部相當短，只有兩顆上頜的長牙。口鼻部的前端有角質喙狀嘴，用來切碎植物。";
        boss3.DialogBoxes.Add(dialog_m331);
        ConfigData_DialogBox dialog_m332 = new ConfigData_DialogBox();
        dialog_m332.ByWhom = "M03";
        dialog_m332.ImagePath = "Images/SupportImage/Lystrosaurus_SupportImg2";
        dialog_m332.Text_TC = "我的頜部關節原始，只能做出往前、往後的動作，而不能做出往上、往下的動作。";
        boss3.DialogBoxes.Add(dialog_m332);
        ConfigData_DialogBox dialog_m34 = new ConfigData_DialogBox();
        dialog_m34.ByWhom = "M03";
        dialog_m34.ImagePath = "";
        dialog_m34.Text_TC = "好似唔小心就講咗好多關於自己嘅嘢，有啲唔好意思，下次有機會再傾多啲。";
        boss3.DialogBoxes.Add(dialog_m34);
        ConfigData_DialogBox dialog_m35 = new ConfigData_DialogBox();
        dialog_m35.ByWhom = "M03";
        dialog_m35.ImagePath = "";
        dialog_m35.Text_TC = "你快啲去繼續探索吓啦，仲有好多新奇有趣嘅事物等緊你！";
        boss3.DialogBoxes.Add(dialog_m35);
        bosses.Add(boss3);

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

        ConfigData_Character npc_p5 = new ConfigData_Character();
        npc_p5.Id = "NPC_P05";
        npc_p5.Name_TC = "舌羊齒屬";
        npc_p5.IsCollectable = false;
        npc_p5.IsFirstMeetDone = false;
        npc_p5.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p51 = new ConfigData_DialogBox();
        dialog_p51.ByWhom = "DRO";
        dialog_p51.ImagePath = "";
        dialog_p51.Text_TC = "看看這個是舌羊齒屬呀!";
        npc_p5.DialogBoxes.Add(dialog_p51);
        ConfigData_DialogBox dialog_p52 = new ConfigData_DialogBox();
        dialog_p52.ByWhom = "DRO";
        dialog_p52.ImagePath = "";
        dialog_p52.Text_TC = "舌羊齒的典型植株高度約為8公尺，呈喬木狀。葉片的大小與寬度各異，呈羊舌狀。";
        npc_p5.DialogBoxes.Add(dialog_p52);
        NPC_Permian.Add(npc_p5);

        ConfigData_Character npc_p6 = new ConfigData_Character();
        npc_p6.Id = "NPC_P06";
        npc_p6.Name_TC = "引螈屬";
        npc_p6.IsCollectable = false;
        npc_p6.IsFirstMeetDone = false;
        npc_p6.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p61 = new ConfigData_DialogBox();
        dialog_p61.ByWhom = "DRO";
        dialog_p61.ImagePath = "";
        dialog_p61.Text_TC = "看看這個是引螈屬呀!";
        npc_p6.DialogBoxes.Add(dialog_p61);
        ConfigData_DialogBox dialog_p62 = new ConfigData_DialogBox();
        dialog_p62.ByWhom = "DRO";
        dialog_p62.ImagePath = "";
        dialog_p62.Text_TC = "引螈屬在石炭紀才是全盛時期。";
        npc_p6.DialogBoxes.Add(dialog_p62);
        NPC_Permian.Add(npc_p6);

        ConfigData_Character npc_p7 = new ConfigData_Character();
        npc_p7.Id = "NPC_P07";
        npc_p7.Name_TC = "空尾蜥屬";
        npc_p7.IsCollectable = false;
        npc_p7.IsFirstMeetDone = false;
        npc_p7.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p71 = new ConfigData_DialogBox();
        dialog_p71.ByWhom = "DRO";
        dialog_p71.ImagePath = "";
        dialog_p71.Text_TC = "看看這個是空尾蜥屬呀!";
        npc_p7.DialogBoxes.Add(dialog_p71);
        ConfigData_DialogBox dialog_p72 = new ConfigData_DialogBox();
        dialog_p72.ByWhom = "DRO";
        dialog_p72.ImagePath = "";
        dialog_p72.Text_TC = "空尾蜥屬擁有特化的類似翅膀結構，使牠可以滑翔。";
        npc_p7.DialogBoxes.Add(dialog_p72);
        NPC_Permian.Add(npc_p7);

        ConfigData_Character npc_p8 = new ConfigData_Character();
        npc_p8.Id = "NPC_P08";
        npc_p8.Name_TC = "瘤頭龍屬";
        npc_p8.IsCollectable = false;
        npc_p8.IsFirstMeetDone = false;
        npc_p8.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p81 = new ConfigData_DialogBox();
        dialog_p81.ByWhom = "DRO";
        dialog_p81.ImagePath = "";
        dialog_p81.Text_TC = "看看這個是瘤頭龍屬呀!";
        npc_p8.DialogBoxes.Add(dialog_p81);
        ConfigData_DialogBox dialog_p82 = new ConfigData_DialogBox();
        dialog_p82.ByWhom = "DRO";
        dialog_p82.ImagePath = "";
        dialog_p82.Text_TC = "瘤頭龍屬與現代牛差不多大，背部有一個多節的頭骨和骨質板甲。";
        npc_p8.DialogBoxes.Add(dialog_p82);
        NPC_Permian.Add(npc_p8);

        ConfigData_Character npc_p9 = new ConfigData_Character();
        npc_p9.Id = "NPC_P09";
        npc_p9.Name_TC = "盾甲龍屬";
        npc_p9.IsCollectable = false;
        npc_p9.IsFirstMeetDone = false;
        npc_p9.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p91 = new ConfigData_DialogBox();
        dialog_p91.ByWhom = "DRO";
        dialog_p91.ImagePath = "";
        dialog_p91.Text_TC = "看看這個是盾甲龍屬呀!";
        npc_p9.DialogBoxes.Add(dialog_p91);
        ConfigData_DialogBox dialog_p92 = new ConfigData_DialogBox();
        dialog_p92.ByWhom = "DRO";
        dialog_p92.ImagePath = "";
        dialog_p92.Text_TC = "盾甲龍屬是大型無孔類爬行動物，而且不像其他爬行動物，牠們的腿是位在他們的身體底下，以支撐牠們的重量。";
        npc_p9.DialogBoxes.Add(dialog_p92);
        NPC_Permian.Add(npc_p9);

        ConfigData_Character npc_p10 = new ConfigData_Character();
        npc_p10.Id = "NPC_P10";
        npc_p10.Name_TC = "狼蜥獸屬";
        npc_p10.IsCollectable = false;
        npc_p10.IsFirstMeetDone = false;
        npc_p10.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p101 = new ConfigData_DialogBox();
        dialog_p101.ByWhom = "DRO";
        dialog_p101.ImagePath = "";
        dialog_p101.Text_TC = "看看這個是狼蜥獸屬呀!";
        npc_p10.DialogBoxes.Add(dialog_p101);
        ConfigData_DialogBox dialog_p102 = new ConfigData_DialogBox();
        dialog_p102.ByWhom = "DRO";
        dialog_p102.ImagePath = "";
        dialog_p102.Text_TC = "狼蜥獸屬是四足動物，四肢直立於身體下方。頭顱骨長45公分，身長約3到4公尺。";
        npc_p10.DialogBoxes.Add(dialog_p102);
        NPC_Permian.Add(npc_p10);

        ConfigData_Character npc_p11 = new ConfigData_Character();
        npc_p11.Id = "NPC_P11";
        npc_p11.Name_TC = "甲蟲";
        npc_p11.IsCollectable = false;
        npc_p11.IsFirstMeetDone = false;
        npc_p11.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p111 = new ConfigData_DialogBox();
        dialog_p111.ByWhom = "DRO";
        dialog_p111.ImagePath = "";
        dialog_p111.Text_TC = "看看這個是甲蟲呀!";
        npc_p11.DialogBoxes.Add(dialog_p111);
        ConfigData_DialogBox dialog_p112 = new ConfigData_DialogBox();
        dialog_p112.ByWhom = "DRO";
        dialog_p112.ImagePath = "";
        dialog_p112.Text_TC = "甲蟲像其他原始甲蟲一樣，它們被認為是食木動物。";
        npc_p11.DialogBoxes.Add(dialog_p112);
        NPC_Permian.Add(npc_p11);

        ConfigData_Character npc_p12 = new ConfigData_Character();
        npc_p12.Id = "NPC_P12";
        npc_p12.Name_TC = "古網翅目";
        npc_p12.IsCollectable = false;
        npc_p12.IsFirstMeetDone = false;
        npc_p12.IsSuccessCollectDone = false;
        ConfigData_DialogBox dialog_p121 = new ConfigData_DialogBox();
        dialog_p121.ByWhom = "DRO";
        dialog_p121.ImagePath = "";
        dialog_p121.Text_TC = "看看這個是古網翅目呀!";
        npc_p12.DialogBoxes.Add(dialog_p121);
        ConfigData_DialogBox dialog_p122 = new ConfigData_DialogBox();
        dialog_p122.ByWhom = "DRO";
        dialog_p122.ImagePath = "";
        dialog_p122.Text_TC = "古網翅目有像鳥喙的口器，前後翅膀相似，在第一對翅膀前有一對額外的小翅。";
        npc_p12.DialogBoxes.Add(dialog_p122);
        NPC_Permian.Add(npc_p12);

        //-------

        ConfigData_DialogBox dialog_tips = new ConfigData_DialogBox();
        dialog_tips.ByWhom = "DRO";
        dialog_tips.ImagePath = "";
        dialog_tips.Text_TC = "有甚麼需要幫忙的嗎？";
        List<string> options_tips = new List<string>();
        options_tips.Add("提示");
        options_tips.Add("古生物圖鑑");
        options_tips.Add("傳送門");
        dialog_tips.OptionTexts_TC = options_tips;
        dialogBox_TipsByDrone = dialog_tips;

        List<ConfigData_DialogBox> dialog_hints = new List<ConfigData_DialogBox>();
        ConfigData_DialogBox hint1 = new ConfigData_DialogBox();
        hint1.ByWhom = "DRO";
        hint1.ImagePath = "";
        hint1.Text_TC = "頭上有綠色圖標的就是目標羊膜動物，把握機會與牠們交談！";
        dialog_hints.Add(hint1);
        ConfigData_DialogBox hint2 = new ConfigData_DialogBox();
        hint2.ByWhom = "DRO";
        hint2.ImagePath = "";
        hint2.Text_TC = "你可以與地圖上的古生物交談。";
        dialog_hints.Add(hint2);
        ConfigData_DialogBox hint3 = new ConfigData_DialogBox();
        hint3.ByWhom = "DRO";
        hint3.ImagePath = "";
        hint3.Text_TC = "你可以隨時找我轉換地圖。";
        dialog_hints.Add(hint3);
        dialogBox_TipsByDrone_Hints = dialog_hints;

        ConfigData_DialogBox dialog_CB = new ConfigData_DialogBox();
        dialog_CB.ByWhom = "DRO";
        dialog_CB.Text_TC = "進入古生物圖鑑？";
        List<string> options_CB = new List<string>();
        options_CB.Add("是");
        options_CB.Add("否");
        dialog_CB.OptionTexts_TC = options_CB;
        dialogBox_TipsByDrone_CollectionBook = dialog_CB;

        ConfigData_DialogBox dialog_ChangeMap = new ConfigData_DialogBox();
        dialog_ChangeMap.ByWhom = "DRO";
        dialog_ChangeMap.Text_TC = "前往另一個地質時代？";
        List<string> options_ChangeMap = new List<string>();
        options_ChangeMap.Add("是");
        options_ChangeMap.Add("留在這裏繼續探索");
        dialog_ChangeMap.OptionTexts_TC = options_ChangeMap;
        dialogBox_TipsByDrone_ChangeMap = dialog_ChangeMap;
    }

    //tmp keyboard change scene
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        SceneManager.LoadScene("MainScene");
    //    }
    //    if (Input.GetKeyDown(KeyCode.O))
    //    {
    //        SceneManager.LoadScene("CarboniferousScene");
    //    }
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        SceneManager.LoadScene("PermianScene");
    //    }
    //}
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
    public List<string> OptionTexts_TC;
    public List<string> OptionTexts_SC;
    public List<string> OptionTexts_EN;
}

