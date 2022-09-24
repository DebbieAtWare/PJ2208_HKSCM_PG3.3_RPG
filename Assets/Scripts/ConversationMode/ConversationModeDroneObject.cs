using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationModeDroneObject : MonoBehaviour
{
    public Image img;

    public List<Sprite> sprites = new List<Sprite>();

    int currIndex = 0;
    float aniTime = 0.25f;

    public void ChangeAni()
    {
        LoopAni();
    }

    void LoopAni()
    {
        currIndex++;

        if (currIndex == sprites.Count)
        {
            currIndex = 0;
        }

        img.sprite = sprites[currIndex];
        Invoke("LoopAni", aniTime);
    }
}
