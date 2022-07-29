using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using FlexFramework.Excel;

namespace FlexFramework.Demo
{
    /// <summary>
    /// This script demonstrates how to load csv/xlsx over internet
    /// </summary>
    public class WebLoader : FlexLoader
    {
        public GameObject loading;

        protected override void Start()
        {
            base.Start();
            this.modal.Show("This scene demonstrates how to load csv/xlsx over internet");
        }

        public override void LoadCSV()
        {
            StartCoroutine(DownloadFileAsync("https://flexframework.net/assets/data-1.csv", bytes =>
            {
                var doc = Document.Load(bytes);
                Populate(doc);
                this.modal.Show("Loaded CSV!");
            }));
        }

        public override void LoadExcel()
        {
            StartCoroutine(DownloadFileAsync("https://flexframework.net/assets/data-1.xlsx", bytes =>
            {
                var book = new WorkBook(bytes);
                Populate(book[0]);
                this.modal.Show("Loaded XLSX!");
            }));
        }

        private IEnumerator DownloadFileAsync(string url, DownloadHandler handler)
        {
            loading.SetActive(true);
            using (var req = UnityWebRequest.Get(url))
            {
                yield return req.SendWebRequest();
                if (req.isNetworkError)
                {
                    throw new System.Exception(req.error);
                }
                var bytes = req.downloadHandler.data;
                handler(bytes);
            }
            loading.SetActive(false);
        }
    }
}

