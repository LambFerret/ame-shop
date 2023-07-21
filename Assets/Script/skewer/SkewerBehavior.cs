using UnityEngine;

namespace Script.skewer
{
    public class SkewerBehavior : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private bool _isSkewerFocused;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetSkewerFocused(bool isFocused)
        {
            Debug.Log("is this in your hand? : " + isFocused);
            // gameObject.SetActive(isFocused);
            _isSkewerFocused = isFocused;
        }

        private void Update()
        {
            if (_isSkewerFocused)
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent as RectTransform,
                    Input.mousePosition, null, out localPoint);
                _rectTransform.localPosition = localPoint;
            }
        }
    }
}