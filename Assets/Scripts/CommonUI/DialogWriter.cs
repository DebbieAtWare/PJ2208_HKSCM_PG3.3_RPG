using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogWriter : MonoBehaviour
{
    static DialogWriter instance;

    List<DialogWriterSingle> dialogWriterSingles = new List<DialogWriterSingle>();

    private void Awake()
    {
        instance = this;
    }

    public static DialogWriterSingle AddWriter_Static(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, bool removeWriterBeforeAdd)
    {
        if (removeWriterBeforeAdd)
        {
            instance.RemoveWriter(uiText);
        }
        return instance.AddWriter(uiText, textToWrite, timePerCharacter);
    }

    private DialogWriterSingle AddWriter(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter)
    {
        DialogWriterSingle dialogWriterSingle = new DialogWriterSingle(uiText, textToWrite, timePerCharacter);
        dialogWriterSingles.Add(dialogWriterSingle);
        return dialogWriterSingle;
    }

    public static void RemoveWriter_Static(TextMeshProUGUI uiText)
    {
        instance.RemoveWriter(uiText);
    }

    private void RemoveWriter(TextMeshProUGUI uiText)
    {
        for (int i = 0; i < dialogWriterSingles.Count; i++)
        {
            if (dialogWriterSingles[i].GetUIText() == uiText)
            {
                dialogWriterSingles.RemoveAt(i);
                i--;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < dialogWriterSingles.Count; i++)
        {
            bool destoryInstance = dialogWriterSingles[i].Update();
            if (destoryInstance)
            {
                dialogWriterSingles.RemoveAt(i);
                i--;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(dialogWriterSingles.Count);
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
            uiText.text = "";
        }

        public bool Update()
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
                    return true;
                }
            }
            return false;
        }

        public TextMeshProUGUI GetUIText()
        {
            return uiText;
        }

        public bool IsActive()
        {
            return characterIndex < textToWrite.Length;
        }

        public void WriteAllAndDestroy()
        {
            uiText.text = textToWrite;
            characterIndex = textToWrite.Length;
            DialogWriter.RemoveWriter_Static(uiText);
        }
    }



}
