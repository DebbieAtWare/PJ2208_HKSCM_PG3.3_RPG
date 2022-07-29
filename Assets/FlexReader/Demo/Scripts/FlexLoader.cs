#pragma warning disable 649

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FlexFramework.Excel;

namespace FlexFramework.Demo
{
    public abstract class FlexLoader : Navigator
    {
        [SerializeField]
        protected RectTransform container;
        [SerializeField]
        protected GameObject row;
        [SerializeField]
        protected Modal modal;

        public abstract void LoadCSV();

        public abstract void LoadExcel();

        protected virtual void Start()
        {
            Application.logMessageReceived += OnLogMessage;
        }

        private void OnLogMessage(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception && this.modal)
            {
                modal.Show(condition);
            }
        }

        protected void Populate(IEnumerable<Row> rows)
        {
            foreach (Transform child in container)
            {
                if (child.gameObject != this.row)
                {
                    Destroy(child.gameObject);
                }
            }
            // bypass empty rows
            foreach (var row in rows.Where(r => !r.IsEmpty()))
            {
                var go = Instantiate(this.row, container);
                go.SetActive(true);
                go.GetComponent<UIRow>().SetData(row);
            }
        }

        protected virtual void OnDestroy()
        {
            Application.logMessageReceived -= OnLogMessage;
        }
    }

}

