using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogWriter : MonoBehaviour
{
    static DialogWriter instance;

    List<DialogWriterSingle> dialogWriterSingles = new List<DialogWriterSingle>();

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("DialogWriter Awake");
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static DialogWriterSingle AddWriter_Static(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, bool removeWriterBeforeAdd, Action onComplete)
    {
        if (removeWriterBeforeAdd)
        {
            instance.RemoveWriter(uiText);
        }
        return instance.AddWriter(uiText, textToWrite, timePerCharacter, onComplete);
    }

    private DialogWriterSingle AddWriter(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, Action onComplete)
    {
        DialogWriterSingle dialogWriterSingle = new DialogWriterSingle(uiText, textToWrite, timePerCharacter, onComplete);
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
    }

    public class DialogWriterSingle
    {
        public TextMeshProUGUI uiText;
        public string textToWrite;
        public int characterIndex;
        public float timePerCharacter;
        public float timer;
        Action onComplete;


        public DialogWriterSingle(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, Action onComplete)
        {
            this.uiText = uiText;
            this.textToWrite = textToWrite;
            this.timePerCharacter = timePerCharacter;
            this.onComplete = onComplete;
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

                if (!string.IsNullOrEmpty(textToWrite) && !string.IsNullOrWhiteSpace(textToWrite))
                {
                    string txt = textToWrite.Substring(0, characterIndex);
                    //to prevent showing <> rich tag in running text ani
                    if (textToWrite[characterIndex - 1] == '<')
                    {
                        for (int i = characterIndex; i < textToWrite.Length; i++)
                        {
                            if (textToWrite[i] == '>')
                            {
                                if (i + 2 < (textToWrite.Length - 1))
                                {
                                    characterIndex = i + 2;
                                    txt = textToWrite.Substring(0, characterIndex);
                                }
                                else
                                {
                                    characterIndex = textToWrite.Length;
                                    txt = textToWrite.Substring(0, characterIndex);
                                }
                                break;
                            }
                        }
                    }
                    txt += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                    uiText.text = txt;

                    if (characterIndex >= textToWrite.Length)
                    {
                        if (onComplete != null)
                        {
                            onComplete();
                        }
                        return true;
                    }
                }
                else
                {
                    if (onComplete != null)
                    {
                        onComplete();
                    }
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
            //return characterIndex < textToWrite.Length;
            if (!string.IsNullOrEmpty(textToWrite) && !string.IsNullOrWhiteSpace(textToWrite))
            {
                return characterIndex < textToWrite.Length;
            }
            else
            {
                return false;
            }
        }

        public void WriteAllAndDestroy()
        {
            uiText.text = textToWrite;
            characterIndex = textToWrite.Length;
            if (onComplete != null)
            {
                onComplete();
            }
            DialogWriter.RemoveWriter_Static(uiText);
        }
    }



}
