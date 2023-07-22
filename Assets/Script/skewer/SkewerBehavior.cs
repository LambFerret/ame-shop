using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.skewer
{
    public class SkewerBehavior : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector3 originalPosition;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            originalPosition = rectTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 scale = new Vector2( 2560f /Screen.width,  1440f/Screen.height );
            rectTransform.anchoredPosition += new Vector2(eventData.delta.x * scale.x, eventData.delta.y * scale.y);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // If you want the object to return to its original position after being dragged
            rectTransform.anchoredPosition = originalPosition;
        }

        private RectTransform _rectTransform;
        private bool _isSkewerFocused;
        private bool _isDraggable;



        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }


        public void SetSkewerFocused(bool isFocused)
        {
            _isSkewerFocused = isFocused;
        }

        public void AddFirstIngredient(GameObject prefab)
        {
            const float offset = 2.0f;

            var t = transform.Find("Candy");
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