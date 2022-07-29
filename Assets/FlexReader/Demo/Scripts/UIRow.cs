using UnityEngine;
using UnityEngine.UI;
using FlexFramework.Excel;

namespace FlexFramework.Demo
{
    [RequireComponent(typeof(RectTransform))]
    public class UIRow : MonoBehaviour
    {
        public void SetData(Row row)
        {
            foreach (RectTransform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (var i = 0; i < row.Count; i++)
            {
                var cell = row[i];
                var col = new GameObject(cell.Address.ToString(), typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
                var rect = col.GetComponent<RectTransform>();
                rect.SetParent(transform);
                rect.localScale = Vector3.one;
                rect.anchorMin = new Vector2(i * 1f / row.Count, 0);
                rect.anchorMax = new Vector2((i + 1f) / row.Count, 1);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
                var text = col.GetComponent<Text>();
                text.text = cell.Text;
                text.fontSize = 14;
                text.alignment = TextAnchor.MiddleCenter;
                text.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
                text.color = Color.black;
            }
        }
    }
}

