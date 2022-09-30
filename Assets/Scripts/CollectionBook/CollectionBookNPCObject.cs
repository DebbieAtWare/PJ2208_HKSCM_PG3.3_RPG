using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionBookNPCObject : MonoBehaviour
{
    public CharacterID characterID;
    public MapID mapID { get; private set; }
    public int configDataIndex { get; private set; }
    public GameObject frame_In;
    public GameObject frame_Out;
    public TextMeshProUGUI numText_In;
    public TextMeshProUGUI numText_Out;

    public void Setup(MapID _mapID, int _configDataIndex, string numText)
    {
        mapID = _mapID;
        configDataIndex = _configDataIndex;
        numText_In.text = numText;
        numText_Out.text = numText;
    }

    public void SetSelection(bool val)
    {
        if (val)
        {
            frame_In.SetActive(true);
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
