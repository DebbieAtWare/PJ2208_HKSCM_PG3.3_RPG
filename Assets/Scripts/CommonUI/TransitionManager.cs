using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using TMPro;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    [Header("UI")]
    public GameObject rootObj;
    public RawImage camFeedImg;
    public RawImage camFeedImg2;
    public Image timeTravelBkgImg;
    public List<Sprite> bkgSprites = new List<Sprite>();
    //48 frame, 12 fps, 4 sec. 4/48 = 0.083
    public float bkgAniTime = 0.083f;
    public int currBkgIndex;
    public Image blackImg;

    [Header("Transition Pic")]
    public Texture2D transitionTexture_Carboniferous;
    public Texture2D transitionTexture_Permian;
    public Texture2D transitionTexture_Lab;
    public Texture2D transitionTexture_InsideTreeCave;
    Texture2D transitionTexture_OutsideTreeCave;

    [Header("End to Lab")]
    public TextMeshProUGUI endToLab_Text_TC;
    public TextMeshProUGUI endToLab_Text_SC;
    public TextMeshProUGUI endToLab_Text_EN;

    CommonUtils commonUtils;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("TransitionManager Awake");
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

    void Start()
    {
        commonUtils = CommonUtils.instance;
    }

    public void ChangeMap(MapID currMap, MapID targetMap)
    {
        InputManager.instance.canInput_Confirm = false;
        SoundManager.instance.FadeOutStop_Dialog(0.3f);
        SoundManager.instance.Play_SFX(11);
        rootObj.SetActive(true);
        StartCoroutine(ChangeMap());
        IEnumerator ChangeMap()
        {
            GameManager.instance.fadingBetweenAreas = true;
            DroneController.instance.canShowTalkHint = false;
            DroneController.instance.HideTalkHint();
            yield return new WaitForEndOfFrame();
            //screen cap current map and pixelate
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();
            if (currMap == MapID.Lab)
            {
                camFeedImg.texture = tex;
            }
            else if (currMap == MapID.Carboniferous)
            {
                commonUtils.playerPos_Carboniferous = PlayerController.instance.transform.position;
                commonUtils.playerDir_Carboniferous = PlayerController.instance.GetDirection();
                commonUtils.dronePos_Carboniferous = DroneController.instance.transform.position;
                transitionTexture_Carboniferous = tex;
                camFeedImg.texture = transitionTexture_Carboniferous;
            }
            else if (currMap == MapID.Permian)
            {
                commonUtils.playerPos_Permian = PlayerController.instance.transform.position;
                commonUtils.playerDir_Permian = PlayerController.instance.GetDirection();
                commonUtils.dronePos_Permian = DroneController.instance.transform.position;
                transitionTexture_Permian = tex;
                camFeedImg.texture = transitionTexture_Permian;
            }
            camFeedImg.DOFade(1f, 1f);
            camFeedImg.material.DOFloat(50f, "_PixelateSize", 1f).From(512f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1f);
            //fade in time travel bkg
            currBkgIndex = 0;
            BkgLoopAni();
            timeTravelBkgImg.DOFade(1f, 1f);
            yield return new WaitForSeconds(1f);
            //change scene
            if (targetMap == MapID.Permian)
            {
                SceneManager.LoadScene("PermianScene");
                if (currMap == MapID.Lab)
                {
                    //first greeting: player facing down
                    PlayerController.instance.SetDirection(PlayerDirection.Down);
                }
                else
                {
                    PlayerController.instance.SetDirection(commonUtils.playerDir_Permian);
                }
                PlayerController.instance.transform.position = commonUtils.playerPos_Permian;
                DroneController.instance.ChangePos(commonUtils.dronePos_Permian);
            }
            else if (targetMap == MapID.Carboniferous)
            {
                SceneManager.LoadScene("CarboniferousScene");
                if (currMap == MapID.Lab)
                {
                    //first greeting: player facing down
                    PlayerController.instance.SetDirection(PlayerDirection.Down);
                }
                else
                {
                    PlayerController.instance.SetDirection(commonUtils.playerDir_Carboniferous);
                }
                PlayerController.instance.transform.position = commonUtils.playerPos_Carboniferous;
                DroneController.instance.ChangePos(commonUtils.dronePos_Carboniferous);
            }
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            yield return new WaitForSeconds(1f);
            //show next map and depixelate
            if (targetMap == MapID.Permian)
            {
                camFeedImg.texture = transitionTexture_Permian;
            }
            else if (targetMap == MapID.Carboniferous)
            {
                camFeedImg.texture = transitionTexture_Carboniferous;
            }
            timeTravelBkgImg.DOFade(0f, 1f);
            yield return new WaitForSeconds(0.5f);
            camFeedImg.material.DOFloat(512f, "_PixelateSize", 1f).From(50f).SetEase(Ease.Linear);
            camFeedImg.DOFade(0f, 1f);
            yield return new WaitForSeconds(1.4f);
            CancelInvoke("BkgLoopAni");
            GameManager.instance.fadingBetweenAreas = false;
            InputManager.instance.canInput_Confirm = true;
            if (currMap == MapID.Lab)
            {
                MainManger.instance.currStage = MainStage.InGame;
                if (targetMap == MapID.Carboniferous)
                {
                    CarboniferousManager.instance.FirstGreetingControl();
                }
                else if (targetMap == MapID.Permian)
                {
                    PermianManager.instance.FirstGreetingControl();
                }
            }
            else if (currMap == MapID.Carboniferous || currMap == MapID.Permian)
            {
                commonUtils.currEndingCheck = EndingCheckStage.None;
                //every time change map will call first greeting
                if (targetMap == MapID.Carboniferous)
                {
                    CarboniferousManager.instance.FirstGreetingControl();
                }
                else if (targetMap == MapID.Permian)
                {
                    PermianManager.instance.FirstGreetingControl();
                }
            }
            commonUtils.currMapId = targetMap;
        }
    }

    void BkgLoopAni()
    {
        currBkgIndex++;

        if (currBkgIndex == bkgSprites.Count)
        {
            currBkgIndex = 0;
        }

        timeTravelBkgImg.sprite = bkgSprites[currBkgIndex];
        Invoke("BkgLoopAni", bkgAniTime);
    }

    public void ChangeToInsideTreeCave()
    {
        SoundManager.instance.Play_SFX(6);
        StartCoroutine(ChangeToTreeCave());
        IEnumerator ChangeToTreeCave()
        {
            GameManager.instance.fadingBetweenAreas = true;
            yield return new WaitForEndOfFrame();
            //screen cap current map and pixelate
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();
            transitionTexture_OutsideTreeCave = tex;
            camFeedImg.texture = transitionTexture_OutsideTreeCave;
            camFeedImg.DOFade(1f, 1f);
            camFeedImg.material.DOFloat(50f, "_PixelateSize", 1f).From(512f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1f);
            //fade in inside cave pixelate img
            camFeedImg2.texture = transitionTexture_InsideTreeCave;
            camFeedImg2.material.SetFloat("_PixelateSize", 50f);
            camFeedImg2.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("Boss01Scene");
            PlayerController.instance.transform.position = commonUtils.playerPos_InsideTreeCave;
            PlayerController.instance.SetDirection(commonUtils.playerDir_InsideTreeCave);
            DroneController.instance.ChangePos(commonUtils.dronePos_InsideTreeCave);
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            //depixelate outside cave img
            camFeedImg.DOFade(0f, 0f);
            camFeedImg2.material.DOFloat(512f, "_PixelateSize", 1f).From(50f).SetEase(Ease.Linear);
            camFeedImg2.DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);
            GameManager.instance.fadingBetweenAreas = false;
        }
    }

    public void ChangeToOutsideTreeCave()
    {
        StartCoroutine(ChangeToOutsideTreeCave());
        IEnumerator ChangeToOutsideTreeCave()
        {
            GameManager.instance.fadingBetweenAreas = true;
            blackImg.DOFade(1f, 1f);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("CarboniferousScene");
            PlayerController.instance.transform.position = commonUtils.playerPos_OutsideTreeCave;
            PlayerController.instance.SetDirection(commonUtils.playerDir_OutsideTreeCave);
            DroneController.instance.ChangePos(commonUtils.dronePos_OutsideTreeCave);
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            yield return new WaitForSeconds(0.5f);
            blackImg.DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);
            //GameManager.instance.fadingBetweenAreas = false;
            //GameManager.instance.dialogActive = false;
            //InputManager.instance.canInput_Confirm = true;
            //DroneController.instance.canShowTalkHint = true;
            InputManager.instance.canInput_Confirm = true;
            commonUtils.EndingCheck();
        }
    }

    public void EndingVideoToLab()
    {
        StartCoroutine(Ani());
        IEnumerator Ani()
        {
            GameManager.instance.fadingBetweenAreas = true;
            SoundManager.instance.Play_SFX(11);
            yield return new WaitForEndOfFrame();
            //screen cap current map and pixelate
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();
            camFeedImg.texture = tex;
            camFeedImg.DOFade(1f, 1f);
            camFeedImg.material.DOFloat(50f, "_PixelateSize", 1f).From(512f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1f);
            EndVideoManager.instance.ResetAll();
            //fade in time travel bkg
            currBkgIndex = 0;
            BkgLoopAni();
            timeTravelBkgImg.DOFade(1f, 1f);
            if (commonUtils.currLang == Language.TC)
            {
                endToLab_Text_TC.DOFade(1f, 1f);
                endToLab_Text_SC.alpha = 0;
                endToLab_Text_EN.alpha = 0;
            }
            else if (commonUtils.currLang == Language.SC)
            {
                endToLab_Text_TC.alpha = 0;
                endToLab_Text_SC.DOFade(1f, 1f);
                endToLab_Text_EN.alpha = 0;
            }
            else if (commonUtils.currLang == Language.EN)
            {
                endToLab_Text_TC.alpha = 0;
                endToLab_Text_SC.alpha = 0;
                endToLab_Text_EN.DOFade(1f, 1f);
            }
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("MainScene");
            commonUtils.currMapId = MapID.Lab;
            PlayerController.instance.SetDirection(PlayerDirection.Down);
            PlayerController.instance.transform.position = new Vector3(-1.6f, 0f, 0f);
            DroneController.instance.ChangePos(new Vector3(0.8f, -0.8f, 0f));
            MinimapManager.instance.Hide(0);
            StatusBarManager.instance.Hide_Carbon(0);
            StatusBarManager.instance.Hide_Permian(0);
            yield return new WaitForSeconds(2f);
            //show next map and depixelate
            camFeedImg.texture = transitionTexture_Lab;
            timeTravelBkgImg.DOFade(0f, 1f);
            endToLab_Text_TC.DOFade(0f, 1f);
            endToLab_Text_SC.DOFade(0f, 1f);
            endToLab_Text_EN.DOFade(0f, 1f);
            yield return new WaitForSeconds(0.6f);
            camFeedImg.material.DOFloat(512f, "_PixelateSize", 1f).From(50f).SetEase(Ease.Linear);
            camFeedImg.DOFade(0f, 1f);
            yield return new WaitForSeconds(1.4f);
            CancelInvoke("BkgLoopAni");
            GameManager.instance.fadingBetweenAreas = false;
            MainManger.instance.ChangeStage_EndLab();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("ScreenCap1");
            StartCoroutine(ScreenShot());
            IEnumerator ScreenShot()
            {
                Debug.Log("ScreenCap2");
                yield return new WaitForEndOfFrame();
                Debug.Log("ScreenCap3");
                Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
                tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                tex.Apply();
                Byte[] bytes = tex.EncodeToPNG();
                File.WriteAllBytes(Path.Combine(Application.streamingAssetsPath, "Image.png"), bytes);
                Debug.Log("ScreenCap4");
            }
        }
    }
}
