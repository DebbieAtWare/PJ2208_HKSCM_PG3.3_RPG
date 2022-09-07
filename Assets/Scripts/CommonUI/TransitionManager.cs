using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    public GameObject rootObj;
    public RawImage camFeedImg;
    public Image timeTravelBkgImg;

    void Start()
    {
        instance = this;
    }

    public void StartTransition()
    {
        rootObj.SetActive(true);
        StartCoroutine(ScreenShot());
        IEnumerator ScreenShot()
        {
            yield return new WaitForEndOfFrame();
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();
            camFeedImg.texture = tex;
            camFeedImg.DOFade(1f, 1f);
            camFeedImg.material.DOFloat(50f, "_PixelateSize", 1f).From(512f);
            yield return new WaitForSeconds(1f);
            timeTravelBkgImg.DOFade(1f, 1f);
            yield return new WaitForSeconds(1.5f);
            camFeedImg.DOFade(0f, 0f);
            timeTravelBkgImg.DOFade(0f, 1f);
        }
    }
}
