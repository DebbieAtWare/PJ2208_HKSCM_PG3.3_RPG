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

    [Header("Transition Pic")]
    public Texture2D transitionTexture_Carboniferous;
    public Texture2D transitionTexture_Permian;

    CommonUtils commonUtils;

    void Start()
    {
        instance = this;

        commonUtils = CommonUtils.instance;
    }

    public void ChangeMap(bool isCurrCarbon)
    {
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
            if (isCurrCarbon)
            {
                commonUtils.playerPos_Carboniferous = PlayerController.instance.transform.position;
                commonUtils.playerDir_Carboniferous = PlayerController.instance.GetDirection();
                commonUtils.dronePos_Carboniferous = DroneController.instance.transform.position;
                transitionTexture_Carboniferous = tex;
                camFeedImg.texture = transitionTexture_Carboniferous;
            }
            else
            {
                commonUtils.playerPos_Permian = PlayerController.instance.transform.position;
                commonUtils.playerDir_Permian = PlayerController.instance.GetDirection();
                commonUtils.dronePos_Permian = DroneController.instance.transform.position;
                transitionTexture_Permian = tex;
                camFeedImg.texture = transitionTexture_Permian;
            }
            camFeedImg.DOFade(1f, 1f);
            camFeedImg.material.DOFloat(50f, "_PixelateSize", 1f).From(512f);
            yield return new WaitForSeconds(1f);
            //fade in time travel bkg
            timeTravelBkgImg.DOFade(1f, 1f);
            yield return new WaitForSeconds(1f);
            //change scene
            if (isCurrCarbon)
            {
                SceneManager.LoadScene("PermianScene");
                PlayerController.instance.transform.position = commonUtils.playerPos_Permian;
                PlayerController.instance.SetDirection(commonUtils.playerDir_Permian);
                DroneController.instance.ChangePos(commonUtils.dronePos_Permian);
            }
            else
            {
                SceneManager.LoadScene("CarboniferousScene");
                PlayerController.instance.transform.position = commonUtils.playerPos_Carboniferous;
                PlayerController.instance.SetDirection(commonUtils.playerDir_Carboniferous);
                DroneController.instance.ChangePos(commonUtils.dronePos_Carboniferous);
            }
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            yield return new WaitForSeconds(1f);
            //show next map and depixelate
            if (isCurrCarbon)
            {
                camFeedImg.texture = transitionTexture_Permian;
            }
            else
            {
                camFeedImg.texture = transitionTexture_Carboniferous;
            }
            timeTravelBkgImg.DOFade(0f, 1f);
            yield return new WaitForSeconds(0.6f);
            camFeedImg.material.DOFloat(512f, "_PixelateSize", 1f).From(50f);
            camFeedImg.DOFade(0f, 1f);
            yield return new WaitForSeconds(2f);
            GameManager.instance.fadingBetweenAreas = false;
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
    //            File.WriteAllBytes(Path.Combine(Application.streamingAssetsPath,"Image.png"), bytes);
    //            Debug.Log("ScreenCap4");
    //        }
    //    }
    //}
}
