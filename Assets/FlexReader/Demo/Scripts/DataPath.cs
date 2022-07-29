using System.IO;
using UnityEngine;
using FlexFramework.Excel;

namespace FlexFramework.Demo
{
    /// <summary>
    /// This script demonstrates how to load csv/xlsx in application folder.
    /// Use this method only for desktop applications.
    /// Consider using Application.persistentDataPath if your csv/xlsx files are fetched at runtime.
    /// </summary>
    public class DataPath : FlexLoader
    {
        protected override void Start()
        {
            base.Start();
            this.modal.Show("This scene demonstrates how to load csv/xlsx in application folder.\n" +
            "If not running in editor mode, files should be copied to application path manually.\n" +
            "Use this method only for desktop applications.\n" +
            "Consider using Application.persistentDataPath if your csv/xlsx files are saved at runtime.\n");
        }

        public override void LoadCSV()
        {
            // datapath is readonly on most platforms.
            // in editor it points to Assets folder
            // in windows build, it points to the executable(.exe) path
            // Assets/Demo/data-1.csv
            var path = Path.Combine(Application.dataPath, "Demo", "data-1.csv");
            var doc = Document.LoadAt(path);
            Populate(doc);
            this.modal.Show("Loaded CSV!");
        }

        public override void LoadExcel()
        {
            // Assets/Demo/data-1.xlsx.bytes
            var path = Path.Combine(Application.dataPath, "Content", "test_content.xlsx");
            var book = new WorkBook(path);
            Populate(book[0]);
            this.modal.Show("Loaded XLSX!");
        }
    }
}

