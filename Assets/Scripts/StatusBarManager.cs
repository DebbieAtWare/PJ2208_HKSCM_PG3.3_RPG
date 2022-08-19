using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class StatusBarManager : MonoBehaviour
{
    public static StatusBarManager instance;

    public CanvasGroup canvasGrp_Carbon;
    public CanvasGroup canvasGrp_Permian;

    public Image img_Boss1;
    public Image img_Boss2;
    public Image img_Boss3;

    public TextMeshProUGUI text_Carbon;
    public TextMeshProUGUI text_Permian;

    public Sprite dot_Lock;
    public Sprite dot_Open;

    void Start()
    {
        instance = this;
    }

    public void Show_Carbon(float aniTime)
    {
        canvasGrp_Carbon.DOFade(1, aniTime);
    }

    public void Hide_Carbon(float aniTime)
    {
        canvasGrp_Carbon.DOFade(0, aniTime);
    }
    
    public void Update_Carbon(bool boss1)
    {
        if (boss1)
        {
            img_Boss1.sprite = dot_Open;
            text_Carbon.text = "1/1";
        }
        else
        {
            img_Boss1.sprite = dot_Lock;
            text_Carbon.text = "0/1";
        }
    }


    public void Show_Permian(float aniTime)
    {
        canvasGrp_Permian.DOFade(1, aniTime);
    }

    public void Hide_Permian(float aniTime)
    {
        canvasGrp_Permian.DOFade(0, aniTime);
    }

    public void Update_Permian(bool boss2, bool boss3)
    {
        if (!boss2 && !boss3)
        {
            img_Boss2.sprite = dot_Lock;
            img_Boss3.sprite = dot_Lock;
            text_Permian.text = "0/2";
        }
        else if (boss2 && !boss3)
        {
            img_Boss2.sprite = dot_Open;
            img_Boss3.sprite = dot_Lock;
            text_Permian.text = "1/2";
        }
        else if (!boss2 && boss3)
        {
            img_Boss2.sprite = dot_Lock;
            img_Boss3.sprite = dot_Open;
            text_Permian.text = "1/2";
        }
        else if (boss2 && boss3)
        {
            img_Boss2.sprite = dot_Open;
            img_Boss3.sprite = dot_Open;
            text_Permian.text = "2/2";
        }
    }
}
