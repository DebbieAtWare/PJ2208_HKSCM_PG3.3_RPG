using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using FlexFramework.Excel;

namespace FlexFramework.Demo
{
    public delegate void DownloadHandler(byte[] bytes);

    /// <summary>
    /// This script demonstrates how to load csv/xlsx in StreamingAssets folder
    /// </summary>
    public class StreamingAssetsLoad : FlexLoader
    {
        protected override void Start()
        {
            base.Start();
            this.modal.Show("This scene demonstrates how to load csv/xlsx in StreamingAssets folder");
        }

        public override void LoadCSV()
        {
            // StreamingAssets/data-3.csv
            StartCoroutine(LoadFileAsync("data-3.csv", bytes =>
            {
                var doc = Document.Load(bytes);
                Populate(doc);
                this.modal.Show("Loaded CSV!");
            }));
        }

        public override void LoadExcel()
        {
            // StreamingAssets/dummy-data-2.xlsx
            // no need to rename its extension to bytes since files in StreamingAssets folder are not pre-processed by Unity
            StartCoroutine(LoadFileAsync("data-3.xlsx", bytes =>
            {
                var book = new WorkBook(bytes);
                Populate(book[0]);
                this.modal.Show("Loaded XLSX!");
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

