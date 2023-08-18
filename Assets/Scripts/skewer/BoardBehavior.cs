using System;
using UnityEngine;
using UnityEngine.UI;

namespace skewer
{
    public class BoardBehavior : MonoBehaviour
    {
        public SkewerController skewer;

        private SkewerBehavior _currentSkewer;
        private GameObject _currentSkewerGameObject;
        public Texture2D brushTexture;
        public float brushSize = 1.0f;
        private Texture2D _canvasTexture;
        private Color[] _brushPixels;
        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            SetupCanvas(GetComponent<Image>().sprite);
        }

        public void OnClickBoard()
        {
            if (_currentSkewer is null)
            {
                ReceiveSkewerFromController();
            }
            else
            {
                // GiveSkewerToController();
            }
        }

        private void ReceiveSkewerFromController()
        {
            skewer.GiveSkewerToBoiler(out GameObject skewerGameObject);
            if (skewerGameObject == null) return;

            _currentSkewerGameObject = skewerGameObject;
            _currentSkewer = skewerGameObject.GetComponent<SkewerBehavior>();
            _currentSkewerGameObject.transform.SetParent(transform);
        }

        private void GiveSkewerToController()
        {
            if (_currentSkewer == null || _currentSkewerGameObject == null)
            {
                Debug.Log("No skewer to give.");
                return;
            }

            if (!skewer.ReceiveSkewerFromBoiler(_currentSkewerGameObject)) return;
            _currentSkewer = null;
            _currentSkewerGameObject = null;
        }

        private void Update()
        {
            if (_currentSkewerGameObject && Input.GetMouseButton(0))
            {
                // Candy 아래의 모든 Image 컴포넌트를 가져옵니다.
                Image[] images = _currentSkewerGameObject.GetComponentsInChildren<Image>();

                foreach (Image image in images)
                {
                    RectTransform rectTransform = image.rectTransform;

                    // 마우스 커서가 해당 이미지의 RectTransform 위에 있는지 확인합니다.
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, null))
                    {
                        // 이미지의 중심 좌표를 계산합니다.
                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out Vector2 localCursor))
                        {
                            int x = (int)(localCursor.x + image.sprite.texture.width / 2F);
                            int y = (int)(localCursor.y + image.sprite.texture.height / 2F);

                            // Paint 메소드를 호출하여 해당 이미지 위에 그립니다.
                            Paint(x, y, image.sprite.texture);
                        }
                    }
                }
            }
        }



        private void SetupCanvas(Sprite sprite)
        {
            _canvasTexture = sprite.texture;
            _brushPixels = brushTexture.GetPixels();
        }

        private void Paint(int x, int y, Texture2D image)
        {

            _canvasTexture = image;

            int startX = x - (int)(brushTexture.width * brushSize) / 2;
            int startY = y - (int)(brushTexture.height * brushSize) / 2;

            int brushW = (int)(brushTexture.width * brushSize);
            int brushH = (int)(brushTexture.height * brushSize);

            Color[] canvasPixels = _canvasTexture.GetPixels(startX, startY, brushW, brushH);

            for (int i = 0; i < canvasPixels.Length; i++)
            {
                canvasPixels[i] = Color.Lerp(canvasPixels[i], _brushPixels[i], _brushPixels[i].a);
            }

            _canvasTexture.SetPixels(startX, startY, brushW, brushH, canvasPixels);
            _canvasTexture.Apply();
        }
    }
}