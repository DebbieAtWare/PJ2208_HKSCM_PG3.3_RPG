using UnityEngine;

namespace FlexFramework.Demo
{
    [RequireComponent(typeof(RectTransform))]
    public class Loading : MonoBehaviour
    {
        private RectTransform rect;
        private float rotateSpeed = 300f;

        private void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            rect.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        }
    }

}

