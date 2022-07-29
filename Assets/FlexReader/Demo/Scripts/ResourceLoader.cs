using UnityEngine;
using FlexFramework.Excel;

namespace FlexFramework.Demo
{
    /// <summary>
    /// This script demonstrates how to load csv/xlsx in Resources folder
    /// </summary>
    public class ResourceLoader : FlexLoader
    {
        protected override void Start()
        {
            base.Start();
            this.modal.Show("This scene demonstrates how to load csv/xlsx in Resources folder");
        }

        public override void LoadCSV()
        {
            // Resources/data-2.csv
            // when using Resources.Load, file extension should be omitted.
            var asset = Resources.Load<TextAsset>("data-2");
            var doc = Document.Load(asset.text);
            Populate(doc);
            this.modal.Show("Loaded CSV!");
        }

        public override void LoadExcel()
        {
            // Resources/data-2.xlsx.bytes
            // when using Resources.Load, file extension should be omitted.
            var asset = Resources.Load<TextAsset>("data-2.xlsx");
            var book = new WorkBook(asset.bytes);
            Populate(book[0]);
            this.modal.Show("Loaded XLSX!");
        }
    }

}
