using UnityEngine;
using UnityEngine.UI;

namespace setting
{
    public class SetBackground : MonoBehaviour
    {
        public Vector2 referenceResolution = new(2560, 1440); // CanvasScaler의 reference resolution

        private void Awake()
        {
            AdjustBackgroundSize();
        }

        private void Start()
        {
            var canvasScaler = FindObjectOfType<CanvasScaler>();
            float screenRatio = (float)Screen.width / Screen.height;
            canvasScaler.matchWidthOrHeight = screenRatio > 1 ? 1 : 0;
        }

        private void AdjustBackgroundSize()
        {
            var canvasScaler = GetComponentInParent<CanvasScaler>();
            var rt = GetComponent<RectTransform>();

            if (!canvasScaler || !rt)
                return;

            // 현재 화면의 비율 계산
            float screenRatio = (float)Screen.width / Screen.height;
            float referenceRatio = referenceResolution.x / referenceResolution.y;

            if (screenRatio > referenceRatio) // 가로 길이가 더 길다면
            {
                // 현재 화면의 가로 세로 비율에 따른 새로운 width를 계산
                float newWidth = referenceResolution.y * screenRatio;
                rt.sizeDelta = new Vector2(newWidth, referenceResolution.y);
            }
        }
    }
}