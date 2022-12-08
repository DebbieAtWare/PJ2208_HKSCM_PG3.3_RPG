using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroVideoManager : MonoBehaviour
{
    public static IntroVideoManager instance;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("IntroVideoManager Awake");
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
}
