using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogWriter : MonoBehaviour
{
    List<DialogWriterSingle> dialogWriterSingles = new List<DialogWriterSingle>();

    public void AddWriter(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter)
    {
        dialogWriterSingles.Add(new DialogWriterSingle(uiText, textToWrite, timePerCharacter));
    }

    private void Update()
    {
        for (int i = 0; i < dialogWriterSingles.Count; i++)
        {
            dialogWriterSingles[i].Update();
        }
    }

    public class DialogWriterSingle
    {
        TextMeshProUGUI uiText;
        string textToWrite;
        int characterIndex;
        float timePerCharacter;
        float timer;

        public DialogWriterSingle(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter)
        {
            this.uiText = uiText;
            this.textToWrite = textToWrite;
            this.timePerCharacter = timePerCharacter;
            characterIndex = 0;
        }

        public void Update()
        {
            if (uiText != null)
            {
                timer -= Time.deltaTime;
                while (timer <= 0)
                {
                    timer += timePerCharacter;
                    characterIndex++;
                    string txt = textToWrite.Substring(0, characterIndex);
                    txt += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                    uiText.text = txt;

                    if (characterIndex >= textToWrite.Length)
                    {
                        uiText = null;
                        return;
                    }
                }
            }
        }
    }



}
