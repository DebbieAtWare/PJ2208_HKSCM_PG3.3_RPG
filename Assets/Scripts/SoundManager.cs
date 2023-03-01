using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Background Music")]
    public List<AudioSource> audioSources_BGM;
    public List<AudioClip> clips_BGM;
    public int currSourcIndex_BGM = -1;

    [Header("Sound Effects")]
    public AudioSource audioSource_SFX;
    public List<AudioClip> clips_SFX;

    [Header("Input")]
    public AudioSource audioSource_Input;
    public List<AudioClip> clips_Input;

    [Header("Dialog")]
    public AudioSource audioSource_Dialog;
    public List<AudioClip> clips_Dialog;

    [Header("Dialog - Drone")]
    public AudioSource audioSource_Dialog_Drone;

    [Header("Walk")]
    public AudioSource audioSource_Walk;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("SoundManager Awake");
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

    //---------

    public void Play_BGM(int index, float fadeDuration)
    {
        if (index < clips_BGM.Count)
        {
            if (currSourcIndex_BGM == -1)
            {
                currSourcIndex_BGM = 0;
                audioSources_BGM[currSourcIndex_BGM].clip = clips_BGM[index];
                audioSources_BGM[currSourcIndex_BGM].Play();
                audioSources_BGM[currSourcIndex_BGM].DOFade(1f, fadeDuration);
            }
            else if (currSourcIndex_BGM == 0)
            {
                audioSources_BGM[0].DOFade(0f, fadeDuration).OnComplete(() => audioSources_BGM[0].Stop());
                audioSources_BGM[1].clip = clips_BGM[index];
                audioSources_BGM[1].Play();
                audioSources_BGM[1].DOFade(1f, fadeDuration);
                currSourcIndex_BGM = 1;
            }
            else if (currSourcIndex_BGM == 1)
            {
                audioSources_BGM[1].DOFade(0f, fadeDuration).OnComplete(() => audioSources_BGM[1].Stop());
                audioSources_BGM[0].clip = clips_BGM[index];
                audioSources_BGM[0].Play();
                audioSources_BGM[0].DOFade(1f, fadeDuration);
                currSourcIndex_BGM = 0;
            }
        }
    }

    public void FadeOutStop_BGM(float t)
    {
        currSourcIndex_BGM = -1;
        audioSources_BGM[0].DOFade(0f, t).OnComplete(() => audioSources_BGM[0].Stop());
        audioSources_BGM[1].DOFade(0f, t).OnComplete(() => audioSources_BGM[1].Stop());
    }

    //---------

    public void Play_SFX(int index)
    {
        if (index < clips_SFX.Count)
        {
            if (audioSource_SFX.isPlaying)
            {
                audioSource_SFX.Stop();
                audioSource_SFX.clip = clips_SFX[index];
                audioSource_SFX.Play();
                audioSource_SFX.DOFade(1f, 0f);
            }
            else
            {
                audioSource_SFX.clip = clips_SFX[index];
                audioSource_SFX.Play();
                audioSource_SFX.DOFade(1f, 0f);
            }
        }
    }

    public void FadeOutStop_SFX(float t)
    {
        audioSource_SFX.DOFade(0f, t).OnComplete(() => audioSource_SFX.Stop());
    }

    //---------

    public void Play_Input(int index)
    {
        if (index < clips_Input.Count)
        {
            if (audioSource_Input.isPlaying)
            {
                audioSource_Input.Stop();
                audioSource_Input.clip = clips_Input[index];
                audioSource_Input.Play();
            }
            else
            {
                audioSource_Input.clip = clips_Input[index];
                audioSource_Input.Play();
            }
        }
    }

    //---------

    public void Play_Dialog(int index)
    {
        if (index < clips_Dialog.Count)
        {
            if (audioSource_Dialog.isPlaying)
            {
                audioSource_Dialog.Stop();
                audioSource_Dialog.clip = clips_Dialog[index];
                audioSource_Dialog.Play();
                audioSource_Dialog.DOFade(1f, 0f);
            }
            else
            {
                audioSource_Dialog.clip = clips_Dialog[index];
                audioSource_Dialog.Play();
                audioSource_Dialog.DOFade(1f, 0f);
            }
        }
    }

    public void FadeOutStop_Dialog(float t)
    {
        audioSource_Dialog.DOFade(0f, t).OnComplete(() => audioSource_Dialog.Stop());
    }

    //---------

    public void Play_Dialog_Drone()
    {
        audioSource_Dialog_Drone.Play();
        audioSource_Dialog_Drone.DOFade(1f, 0f);
    }

    public void FadeOutStop_Dialog_Drone(float t)
    {
        audioSource_Dialog_Drone.DOFade(0f, t).OnComplete(() => audioSource_Dialog_Drone.Pause());
    }

    //---------

    public void Play_Walk()
    {
        if (!audioSource_Walk.isPlaying)
        {
            audioSource_Walk.Play();
            audioSource_Walk.DOFade(1f, 0f);
        }
    }

    public void FadeOutStop_Walk(float t)
    {
        if (audioSource_Walk.isPlaying)
        {
            audioSource_Walk.DOFade(0f, t).OnComplete(() => audioSource_Walk.Stop());
        }
    }
}
