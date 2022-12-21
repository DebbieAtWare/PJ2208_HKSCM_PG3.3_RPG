using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using FlexFramework.Excel;

namespace FlexFramework.Demo
{
    /// <summary>
    /// This script demonstrates how to load csv/xlsx in StreamingAssets folder
    /// </summary>
    public class StreamingAssetsLoadCMS : FlexLoader
    {
        protected override void Start()
        {
            base.Start();
            LoadExcel();
        }

        public override void LoadCSV()
        {
            // StreamingAssets/data-3.csv
            StartCoroutine(LoadFileAsync("data-3.csv", bytes =>
            {
                var doc = Document.Load(bytes);
                Populate(doc);
            }));
        }

        public override void LoadExcel()
        {
            // StreamingAssets/dummy-data-2.xlsx
            // no need to rename its extension to bytes since files in StreamingAssets folder are not pre-processed by Unity
            StartCoroutine(LoadFileAsync("Exh3.3_Content.xlsx", bytes =>
            {
                var book = new WorkBook(bytes);
                //Populate(book[0]);
                Debug.Log(book[0].Rows[12][6].Text);
            }));
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
}

