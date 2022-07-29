using UnityEngine;
using FlexFramework.Excel;

namespace FlexFramework.Demo
{
    /// <summary>
    /// This script demonstrates how to load csv/xlsx directly in scene reference
    /// </summary>
    public class SceneReference : FlexLoader
    {
        [Tooltip("csv file asset")]
        public TextAsset csv;
        [Tooltip("xlsx file asset. Rename file extension to bytes to make it serializable")]
        public TextAsset xlsx;

        protected override void Start()
        {
            base.Start();
            this.modal.Show("This scene demonstrates how to load csv/xlsx directly in scene reference");
        }

        public override void LoadCSV()
        {
            var doc = Document.Load(csv.text);
            Populate(doc);
            this.modal.Show("Loaded CSV!");
        }

        public override void LoadExcel()
        {
            var book = new WorkBook(xlsx.bytes);
            var sheet = book[0];
            Populate(sheet);
            this.modal.Show("Loaded XLSX!");
        }
    }
}

