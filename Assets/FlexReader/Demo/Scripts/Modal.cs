#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace FlexFramework.Demo
{
    /// <summary>
    /// Generic modal
    /// </summary>
    public class Modal : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        public void Show(string text)
        {
            _text.text = text;
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _text.text = "";
            this.gameObject.SetActive(false);
        }
    }

}
