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

    public void Play_BGM(int index)
    {
        if (index < clips_BGM.Count)
        {
            if (currSourcIndex_BGM == -1)
            {
                currSourcIndex_BGM = 0;
                audioSources_BGM[currSourcIndex_BGM].clip = clips_BGM[index];
                audioSources_BGM[currSourcIndex_BGM].Play();
                audioSources_BGM[currSourcIndex_BGM].DOFade(1f, 1f);
            }
            else if (currSourcIndex_BGM == 0)
            {
                audioSources_BGM[0].DOFade(0f, 1f).OnComplete(() => audioSources_BGM[0].Stop());
                audioSources_BGM[1].clip = clips_BGM[index];
                audioSources_BGM[1].Play();
                audioSources_BGM[1].DOFade(1f, 1f);
                currSourcIndex_BGM = 1;
            }
            else if (currSourcIndex_BGM == 1)
            {
                audioSources_BGM[1].DOFade(0f, 1f).OnComplete(() => audioSources_BGM[1].Stop());
                audioSources_BGM[0].clip = clips_BGM[index];
                audioSources_BGM[0].Play();
                audioSources_BGM[0].DOFade(1f, 1f);
                currSourcIndex_BGM = 0;
            }
        }
    }

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
}
