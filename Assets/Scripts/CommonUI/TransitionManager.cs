using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    [Header("UI")]
    public GameObject rootObj;
    public RawImage camFeedImg;
    public Image timeTravelBkgImg;
    public List<Sprite> bkgSprites = new List<Sprite>();
    //48 frame, 12 fps, 4 sec. 4/48 = 0.083
    public float bkgAniTime = 0.083f;
    public int currBkgIndex;
    public Image blackImg;

    [Header("Transition Pic")]
    public Texture2D transitionTexture_Carboniferous_TC;
    public Texture2D transitionTexture_Carboniferous_SC;
    public Texture2D transitionTexture_Carboniferous_EN;
    public Texture2D transitionTexture_Permian_TC;
    public Texture2D transitionTexture_Permian_SC;
    public Texture2D transitionTexture_Permian_EN;

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
            yield return new WaitForEndOfFrame();
            //screen cap current map and pixelate
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();
            if (currMap == MapID.Carboniferous)
            {
                commonUtils.playerPos_Carboniferous = PlayerController.instance.transform.position;
                commonUtils.playerDir_Carboniferous = PlayerController.instance.GetDirection();
                commonUtils.dronePos_Carboniferous = DroneController.instance.transform.position;
                if (commonUtils.currLang == Language.TC)
                {
                    transitionTexture_Carboniferous_TC = tex;
                    camFeedImg.texture = transitionTexture_Carboniferous_TC;
                }
                else if (commonUtils.currLang == Language.SC)
                {
                    transitionTexture_Carboniferous_SC = tex;
                    camFeedImg.texture = transitionTexture_Carboniferous_SC;
                }
                else if (commonUtils.currLang == Language.EN)
                {
                    transitionTexture_Carboniferous_EN = tex;
                    camFeedImg.texture = transitionTexture_Carboniferous_EN;
                }
            }
            else if (currMap == MapID.Permian)
            {
                commonUtils.playerPos_Permian = PlayerController.instance.transform.position;
                commonUtils.playerDir_Permian = PlayerController.instance.GetDirection();
                commonUtils.dronePos_Permian = DroneController.instance.transform.position;
                if (commonUtils.currLang == Language.TC)
                {
                    transitionTexture_Permian_TC = tex;
                    camFeedImg.texture = transitionTexture_Permian_TC;
                }
                else if (commonUtils.currLang == Language.SC)
                {
                    transitionTexture_Permian_SC = tex;
                    camFeedImg.texture = transitionTexture_Permian_SC;
                }
                else if (commonUtils.currLang == Language.EN)
                {
                    transitionTexture_Permian_EN = tex;
                    camFeedImg.texture = transitionTexture_Permian_EN;
                }
                
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
                PlayerController.instance.transform.position = commonUtils.playerPos_Permian;
                PlayerController.instance.SetDirection(commonUtils.playerDir_Permian);
                if (currMap == MapID.Lab)
                {
                    DroneController.instance.ChangePos_FollowStopDist(commonUtils.playerDir_Permian);
                }
                else
                {
                    DroneController.instance.ChangePos(commonUtils.dronePos_Permian);
                }
            }
            else if (targetMap == MapID.Carboniferous)
            {
                SceneManager.LoadScene("CarboniferousScene");
                PlayerController.instance.transform.position = commonUtils.playerPos_Carboniferous;
                PlayerController.instance.SetDirection(commonUtils.playerDir_Carboniferous);
                if (currMap == MapID.Lab)
                {
                    DroneController.instance.ChangePos_FollowStopDist(commonUtils.playerDir_Carboniferous);
                }
                else
                {
                    DroneController.instance.ChangePos(commonUtils.dronePos_Carboniferous);
                }
            }
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            yield return new WaitForSeconds(1f);
            //show next map and depixelate
            if (targetMap == MapID.Permian)
            {
                if (commonUtils.currLang == Language.TC)
                {
                    camFeedImg.texture = transitionTexture_Permian_TC;
                }
                else if (commonUtils.currLang == Language.SC)
                {
                    camFeedImg.texture = transitionTexture_Permian_SC;
                }
                else if (commonUtils.currLang == Language.EN)
                {
                    camFeedImg.texture = transitionTexture_Permian_EN;
                }
            }
            else if (targetMap == MapID.Carboniferous)
            {
                if (commonUtils.currLang == Language.TC)
                {
                    camFeedImg.texture = transitionTexture_Carboniferous_TC;
                }
                else if (commonUtils.currLang == Language.SC)
                {
                    camFeedImg.texture = transitionTexture_Carboniferous_SC;
                }
                else if (commonUtils.currLang == Language.EN)
                {
                    camFeedImg.texture = transitionTexture_Carboniferous_EN;
                }
            }
            timeTravelBkgImg.DOFade(0f, 1f);
            yield return new WaitForSeconds(0.6f);
            camFeedImg.material.DOFloat(512f, "_PixelateSize", 1f).From(50f).SetEase(Ease.Linear);
            camFeedImg.DOFade(0f, 1f);
            yield return new WaitForSeconds(1.4f);
            CancelInvoke("BkgLoopAni");
            GameManager.instance.fadingBetweenAreas = false;
            InputManager.instance.canInput_Confirm = true;
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
            blackImg.DOFade(1f, 1f);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Boss01Scene");
            PlayerController.instance.transform.position = commonUtils.playerPos_InsideTreeCave;
            PlayerController.instance.SetDirection(commonUtils.playerDir_InsideTreeCave);
            DroneController.instance.ChangePos(commonUtils.dronePos_InsideTreeCave);
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            yield return new WaitForSeconds(0.5f);
            blackImg.DOFade(0f, 1f);
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
            GameManager.instance.fadingBetweenAreas = false;
            GameManager.instance.dialogActive = false;
            InputManager.instance.canInput_Confirm = true;
            DroneController.instance.canShowTalkHint = true;
        }
    }


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        Debug.Log("ScreenCap1");
    //        StartCoroutine(ScreenShot());
    //        IEnumerator ScreenShot()
    //        {
    //            Debug.Log("ScreenCap2");
    //            yield return new WaitForEndOfFrame();
    //            Debug.Log("ScreenCap3");
    //            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
    //            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    //            tex.Apply();
    //            Byte[] bytes = tex.EncodeToPNG();
    //            File.WriteAllBytes(Path.Combine(Application.streamingAssetsPath, "Image.png"), bytes);
    //            Debug.Log("ScreenCap4");
    //        }
    //    }
    //}
}
