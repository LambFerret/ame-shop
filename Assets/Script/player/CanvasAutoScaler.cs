using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.player
{
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasAutoScaler : MonoBehaviour
    {
        private CanvasScaler canvasScaler;

        public Vector2 referenceResolution = new Vector2(2560, 1440); // Target resolution

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