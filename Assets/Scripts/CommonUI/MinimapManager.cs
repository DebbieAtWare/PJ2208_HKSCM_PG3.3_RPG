using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager instance;

    public CanvasGroup canvasGrp;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("MinimapManager Awake");
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

    public void Show(float aniTime)
    {
        canvasGrp.gameObject.SetActive(true);
        canvasGrp.DOFade(1, aniTime);
    }

    public void Hide(float aniTime)
    {
        canvasGrp.DOFade(0, aniTime).OnComplete(() => canvasGrp.gameObject.SetActive(false));
    }
}
