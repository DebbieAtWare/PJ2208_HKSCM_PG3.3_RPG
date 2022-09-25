using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionBookNPCObject : MonoBehaviour
{
    public CharacterID id;
    public GameObject frame_In;
    public GameObject frame_Out;
    public TextMeshProUGUI numText_In;
    public TextMeshProUGUI numText_Out;

    public void Setup(string id)
    {
        numText_In.text = id;
        numText_Out.text = id;
    }

    public void SetSelection(bool val)
    {
        if (val)
        {
            frame_In.SetActive(false);
            frame_Out.SetActive(true);
            numText_In.gameObject.SetActive(false);
            numText_Out.gameObject.SetActive(true);
        }
        else
        {
            frame_In.SetActive(true);
            frame_Out.SetActive(false);
            numText_In.gameObject.SetActive(true);
            numText_Out.gameObject.SetActive(false);
        }
    }
}
