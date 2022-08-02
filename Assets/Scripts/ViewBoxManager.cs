using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBoxManager : MonoBehaviour
{
    public static ViewBoxManager instance;

    public GameObject viewBoxObj;

    private void Start()
    {
        instance = this;
    }

    public void ShowViewBox()
    {
        viewBoxObj.SetActive(true);
    }

    public void HideViewBox()
    {
        viewBoxObj.SetActive(false);
    }
}
