using System.Collections.Generic;
using DG.Tweening;
using Script.customer;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.skewer
{
    public class SkewerBehavior : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public List<GameObject> firstIngredientPrefabs;

        private Skewer _currentSkewer;
        private Vector3 _originalPosition;
        private RectTransform _rectTransform;
        private bool _isSkewerFocused;
        private bool _isDraggable;


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _currentSkewer = new Skewer();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPosition = _rectTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDraggable) return;
            Vector2 scale = new Vector2(2560f / Screen.width, 1440f / Screen.height);
            _rectTransform.anchoredPosition += new Vector2(eventData.delta.x * scale.x, eventData.delta.y * scale.y);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Collider2D hitCollider = Physics2D.OverlapPoint(eventData.position);
            if (_isDraggable)
            {
                if (hitCollider != null && hitCollider.CompareTag("Customer"))
                {
                    hitCollider.GetComponent<CustomerBehavior>().CheckServedSkewer(_currentSkewer);
                    Destroy(gameObject);
                }
                else
                {
                    _rectTransform.DOAnchorPos(_originalPosition, 0.5F).SetEase(Ease.OutCubic);
                }
            }
            else
            {
                if (hitCollider != null && hitCollider.CompareTag("Customer"))
                {
                    Debug.Log("u poke customer with skewer.");
                }
            }
        }

        public void Blend()
        {
            _currentSkewer.AddBlendIngredient();
        }

        public void SetSkewerFocused(bool isFocused)
        {
            _isSkewerFocused = isFocused;
        }

        public void AddFirstIngredient(FirstIngredient type)
        {
            const float offset = 2.0f;
            if (_currentSkewer == null) return;
            if (_currentSkewer.GetFirstIngredients().Count > 3) return;

            _currentSkewer.AddFirstIngredient(type);

            foreach (var prefab in firstIngredientPrefabs)
            {
                var nameStr = prefab.GetComponent<IngredientBehavior>().GetName();
                if (nameStr.Equals(type.ToString()))
                {
                    var t = transform.Find("Candy");
                    int childCount = t.childCount;
                    var ingredient = Instantiate(prefab, t);
                    ingredient.transform.localPosition = new Vector3(childCount * offset, childCount * offset, 0);
                }
            }
        }

        public void AddThirdIngredient(ThirdIngredient type)
        {
            if (_currentSkewer == null) return;

            _currentSkewer.AddThirdIngredient(type);

            // foreach (var prefab in firstIngredientPrefabs)
            // {
            //     var nameStr = prefab.GetComponent<IngredientBehavior>().GetName();
            //     if (nameStr.Equals(type.ToString()))
            //     {
            //         var t = transform.Find("Candy");
            //         int childCount = t.childCount;
            //         var ingredient = Instantiate(prefab, t);
            //         ingredient.transform.localPosition = new Vector3(childCount * offset, childCount * offset, 0);
            //     }
            // }
        }

        public Skewer GetSkewer()
        {
            return _currentSkewer;
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