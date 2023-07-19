using UnityEngine;

namespace Script.skewer
{
    public class SkewerBehavior : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent as RectTransform, Input.mousePosition, null, out localPoint);
            _rectTransform.localPosition = localPoint;
        }
    }

}