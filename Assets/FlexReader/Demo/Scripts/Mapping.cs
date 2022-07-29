#pragma warning disable 649

using System;
using System.Collections.Generic;
using UnityEngine;
using FlexFramework.Excel;

namespace FlexFramework.Demo
{
    /// <summary>
    /// This script demonstrates how to use mapping
    /// </summary>
    public class Mapping : Navigator
    {
        public Modal modal;
        public float speed = 3;
        public float distance = 2;
        private GameObject _root;
        private int _index;

        public void LoadCSV()
        {
            // when using Resources.Load, file extension should be omitted.
            var asset = Resources.Load<TextAsset>("data-5");
            // load document
            var doc = Document.Load(asset.bytes);
            // convert document to collections
            var users = doc.Convert<User>();
            Populate(users);
            this.modal.Show("Loaded CSV!");
        }

        public void LoadExcel()
        {
            // when using Resources.Load, file extension should be omitted.
            var asset = Resources.Load<TextAsset>("data-5.xlsx");
            // load workbook
            var book = new WorkBook(asset.bytes);
            // convert worksheet to collections
            var users = book[0].Convert<User>();
            Populate(users);
            this.modal.Show("Loaded XLSX!");
        }

        void Populate(IEnumerable<User> users)
        {
            if (_root)
            {
                Destroy(_root);
            }
            _root = new GameObject("users");
            _index = 0;

            // instantiate gameobjects
            foreach (var user in users)
            {
                var go = GameObject.CreatePrimitive(user.type);
                go.name = user.name;
                go.transform.SetParent(_root.transform);
                go.transform.position = user.position;
                go.GetComponent<Renderer>().material.color = user.theme;
                go.SetActive(user.active);
            }
        }

        void Start()
        {
            Application.logMessageReceived += OnLogMessage;
            // custom string converter
            ValueConverter.Register(input => string.IsNullOrEmpty(input) ? "--Unknown--" : input);
            this.modal.Show("This scene demonstrates how to use mapping");
        }

        void Update()
        {
            if (!_root)
            {
                return;
            }
            if (_root.transform.childCount == 0)
            {
                return;
            }
            if (_index >= _root.transform.childCount)
            {
                _index = 0;
            }
            var target = _root.transform.GetChild(_index);
            if (!target.gameObject.activeSelf)
            {
                _index++;
                return;
            }
            var cam = Camera.main.transform;
            var diff = Vector3.Normalize(target.position - cam.position) * distance;
            var dest = target.position - diff;
            if (cam.position == dest)
            {
                _index++;
            }
            cam.position = Vector3.Lerp(cam.position, dest, Time.deltaTime * speed);
            cam.LookAt(target.position);
        }

        void OnLogMessage(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception && this.modal)
            {
                modal.Show(condition);
            }
        }


        void OnDestroy()
        {
            Application.logMessageReceived -= OnLogMessage;
            ValueConverter.Unregister<string>();
        }

    }

    [Serializable, Table(1)]
    class User
    {
        [Column("A")] public int id;
        [Column("B")] public string name;
        [Column("C")] public int age;
        [Column("D")] public float score;
        [Column("E")] public bool active;
        [Column("F")] public string address;
        [Column("G")] public PrimitiveType type;
        [Column("H")] public Vector3 position;
        [Column("I")] public Color theme;
    }
}
