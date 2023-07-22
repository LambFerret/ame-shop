using UnityEngine;

namespace Script.skewer
{
    public class SkewerBehavior : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private bool _isSkewerFocused;
        private bool _isDraggable;
        private Vector3 _offset;


        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnMouseDown()
        {
            _offset = gameObject.transform.position -
                      Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        }

        private void OnMouseDrag()
        {
            if (!_isDraggable) return;
            Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            transform.position = Camera.main.ScreenToWorldPoint(newPosition) + _offset;
        }

        public void SetSkewerFocused(bool isFocused)
        {
            _isSkewerFocused = isFocused;
        }

        public void AddFirstIngredient(GameObject prefab)
        {
            const float offset = 2.0f;

            var t = transform;
            int childCount = t.childCount;
            var ingredient = Instantiate(prefab, t);
            ingredient.transform.localPosition = new Vector3(childCount * offset, childCount * offset, 0);
        }

        public void SwitchToPackUp()
        {
            _isDraggable = true;
            transform.Find("Candy").gameObject.SetActive(false);
            transform.Find("Package").gameObject.SetActive(true);
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