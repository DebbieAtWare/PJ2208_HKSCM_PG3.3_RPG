using FlexFramework.Excel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public delegate void DownloadHandler(byte[] bytes);

    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadFileAsync("Exh3.3_Content.xlsx", bytes =>
        {
            var book = new WorkBook(bytes);
            //Debug.Log(book[0].Rows[0].Count);
            //Debug.Log(book[0].Rows[12][7].Text);
            //for (int i = 0; i < book[0].Rows[11].Count; i++)
            //{
            //    Debug.Log(book[0].Rows[11][i].Text);
            //}
            Debug.Log(book[1].Rows[11][3]);
            text.text = ToSingleLineString(book[1].Rows[11][3].Text);
            text2.text = book[1].Rows[11][3].Text;
        }));
    }
    string ToSingleLineString(string txt)
    {
        return txt.Replace("\r", "").Replace("\n", "");
    }

    private IEnumerator LoadFileAsync(string path, DownloadHandler handler)
    {
        // streaming assets should be loaded via web request
        // on WebGL/Android platforms, this folder is in a compressed directory
        var url = Path.Combine(Application.streamingAssetsPath, path);
        using (var req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();
            var bytes = req.downloadHandler.data;
            handler(bytes);
        }
    }
}
