using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetControl : MonoBehaviour
{
    void Start()
    {
        Debug.Log("ResetControl 1");
        DestroyImmediate(GameObject.Find("PlayerAvatar"));
        DestroyImmediate(GameObject.Find("Drone"));
        DestroyImmediate(GameObject.Find("Game Manager"));
        DestroyImmediate(GameObject.Find("CommonUtils"));
        DestroyImmediate(GameObject.Find("InputManager"));
        DestroyImmediate(GameObject.Find("MainManager"));
        DestroyImmediate(GameObject.Find("TransitionManager"));
        DestroyImmediate(GameObject.Find("SoundManager"));
        DestroyImmediate(GameObject.Find("IntroVideoManager"));
        DestroyImmediate(GameObject.Find("EndVideoManager"));
        DestroyImmediate(GameObject.Find("OptionManager"));
        DestroyImmediate(GameObject.Find("StatusBarManager"));
        DestroyImmediate(GameObject.Find("MinimapManager"));
        DestroyImmediate(GameObject.Find("CollectionBookManager"));
        DestroyImmediate(GameObject.Find("DialogBoxManager"));
        DestroyImmediate(GameObject.Find("ViewBoxManager"));
        DestroyImmediate(GameObject.Find("ConversationModeManager"));
        DestroyImmediate(GameObject.Find("UI"));
        SceneManager.LoadScene("MainScene");
        Debug.Log("ResetControl 2");
    }
}
