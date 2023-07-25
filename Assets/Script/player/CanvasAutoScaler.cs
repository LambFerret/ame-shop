using UnityEngine;
using UnityEngine.UI;

namespace Script.player
{
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasAutoScaler : MonoBehaviour
    {
        public Vector2 referenceResolution = new(2560, 1440); // Target resolution
        private CanvasScaler canvasScaler;

        private void Awake()
        {
            canvasScaler = GetComponent<CanvasScaler>();
        }

        private void Start()
        {
            SetScale();
        }

        private void SetScale()
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = referenceResolution;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0.5f; // Adjust this value as needed
        }
    }
}