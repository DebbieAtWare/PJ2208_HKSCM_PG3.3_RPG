using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectionBookFeatureObject : MonoBehaviour
{
    public RectTransform rootRect;
    public TextMeshProUGUI text;

    public void Setup(string txt, float gap)
    {
        if (!string.IsNullOrEmpty(txt) && !string.IsNullOrWhiteSpace(txt))
        {
            gameObject.SetActive(true);
            text.text = txt;
            text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, text.preferredHeight);
            rootRect.sizeDelta = new Vector2(rootRect.sizeDelta.x, text.preferredHeight + gap);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
