using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionBookBossObject : MonoBehaviour
{
    public CharacterID id;
    public TextMeshProUGUI nameText_TC;
    public GameObject frameObj;

    public void Setup(string name_TC)
    {
        nameText_TC.text = name_TC;
    }

    public void SetSelection(bool val)
    {
        frameObj.SetActive(val);
    }
}
