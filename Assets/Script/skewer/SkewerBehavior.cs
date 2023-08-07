using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Script.customer;
using Script.ingredient;
using Script.setting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.skewer
{
    public class SkewerBehavior : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Constant Value")] public int currentSkewerMaxLength;
        public int perfectTemperature = 145;

        private bool _isDraggable;
        private bool _isSkewerFocused;
        private Vector3 _originalPosition;
        private RectTransform _rectTransform;

        private List<Ingredient> _firstIngredients;

        private int _currentTemperature;

        private int _perfectDryTime;
        private int _currentDryTime;

        private int _perfectConcentration;
        private int _currentConcentration;

        private List<IngredientManager.ThirdIngredient> _thirdIngredients;

        private bool _isFirstThirdSecond;
        public bool IsAlreadyBlended;
        public BlendedCandy BlendedCandy;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _firstIngredients = new List<Ingredient>();
            _thirdIngredients = new List<IngredientManager.ThirdIngredient>();
        }

        public bool IsNotEnoughDry()
        {
            return _perfectDryTime > _currentDryTime;
        }

        public int CheckConcentration()
        {
            float tolerance = _perfectConcentration * 0.1f;

            if (_currentConcentration >= _perfectConcentration - tolerance &&
                _currentConcentration <= _perfectConcentration + tolerance)
            {
                return 0;
            }

            if (_currentConcentration > _perfectConcentration + tolerance) return 1;
            return -1;
        }

        public bool CheckTemperature()
        {
            return !(_currentTemperature < perfectTemperature - 10 || _currentTemperature > perfectTemperature + 10);
        }


        public void SetSize(int size)
        {
            currentSkewerMaxLength = size;
        }

        public void AddTemperature(int temperature, int concentration)
        {
            _currentTemperature = temperature;
            _currentConcentration = concentration;
        }

        public void AddDryTime(int dryTime)
        {
            _currentDryTime += dryTime;
        }

        private void CalculatePerfect()
        {
            int averageMoisture = 0;
            int totalSize = 0;

            foreach (var ingredient in _firstIngredients)
            {
                averageMoisture += ingredient.moisture;
                totalSize += ingredient.size;
            }

            //TODO 여기 공식 제대로 알고 갑시다
            averageMoisture /= _firstIngredients.Count;
            _perfectConcentration = 100 - averageMoisture;
            _perfectDryTime = totalSize * _perfectConcentration / 100;

            Debug.Log(" what is perfect dry time : " + _perfectDryTime + " perfect concentration : " +
                      _perfectConcentration);
        }

        public void AddBlendIngredient()
        {
            BlendedCandy blendedCandy;
            if (IsAlreadyBlended)
            {
                blendedCandy = BlendedCandy;
                blendedCandy.FirstIngredients.AddRange(GetFirstIngredients());
                blendedCandy.ThirdIngredients.AddRange(GetThirdIngredients());
                //TODO
                // foreach (IngredientManager.SecondIngredient ingredient in GetSecondIngredients())
                // {
                //     switch (ingredient)
                //     {
                //         case IngredientManager.SecondIngredient.NormalSugar:
                //             blendedCandy.SugarAmount += 10;
                //             break;
                //         case IngredientManager.SecondIngredient.ExtraSugar:
                //             blendedCandy.SugarAmount += 20;
                //             break;
                //         case IngredientManager.SecondIngredient.None:
                //             break;
                //     }
                // }
            }
            else
            {
                blendedCandy = new BlendedCandy();
                blendedCandy.FirstIngredients = GetFirstIngredients();
                blendedCandy.ThirdIngredients = GetThirdIngredients();
                // TODO
                // foreach (IngredientManager.SecondIngredient ingredient in GetSecondIngredients())
                // {
                //     switch (ingredient)
                //     {
                //         case IngredientManager.SecondIngredient.NormalSugar:
                //             blendedCandy.SugarAmount += 10;
                //             break;
                //         case IngredientManager.SecondIngredient.ExtraSugar:
                //             blendedCandy.SugarAmount += 20;
                //             break;
                //         case IngredientManager.SecondIngredient.None:
                //             break;
                //     }
                // }
            }

            _firstIngredients.Clear();
            // TODO
            // _secondIngredients.Clear();
            _thirdIngredients.Clear();
            // _dryTime = 0;
            _isFirstThirdSecond = false;
            IsAlreadyBlended = true;
            // _firstIngredients.Add(IngredientManager.FirstIngredient.BlendedCandy);
        }

        // Setting base Ingredients
        public void AddFirstIngredient(Ingredient ingredient)
        {
            _firstIngredients.Add(ingredient);
            SkewIngredients(ingredient);

            CalculatePerfect();
            DebugReadFirstIngredients();
        }

        private void SkewIngredients(Ingredient ingredient)
        {
            const float offset = 2.0f;

            var t = transform.Find("Candy");
            var childCount = t.childCount;
            var prefab = Instantiate(ingredient.prefab, t);
            prefab.transform.localPosition = new Vector3(childCount * offset, childCount * offset, 0);
        }

        public List<Ingredient> GetFirstIngredients()
        {
            DebugReadFirstIngredients();
            return _firstIngredients;
        }

        private void DebugReadFirstIngredients()
        {
            string listString = string.Join(" - ", _firstIngredients.Select(i => i.ingredientName));
            Debug.Log($"[{listString}]");
        }

        public string GetFirstIngredientText()
        {
            string listString = string.Join(" - ", _firstIngredients.Select(i => i.ingredientName));
            return listString;
        }

        // TODO setting third ingredient
        public List<IngredientManager.ThirdIngredient> GetThirdIngredients()
        {
            string listString = string.Join(" - ", _thirdIngredients.Select(i => i.ToString()));
            Debug.Log($"[{listString}]");

            return _thirdIngredients;
        }

        public void AddThirdIngredient(IngredientManager.ThirdIngredient type)
        {
            _thirdIngredients.Add(type);
            GetThirdIngredients();

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

        // Behavior that the skewer is focused or not
        private void Update()
        {
            if (_isSkewerFocused)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent as RectTransform,
                    Input.mousePosition, null, out Vector2 localPoint);
                _rectTransform.localPosition = localPoint;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPosition = _rectTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDraggable)
            {
                Debug.Log("you can");
                return;
            }

            var scale = new Vector2(2560f / Screen.width, 1440f / Screen.height);
            _rectTransform.anchoredPosition += new Vector2(eventData.delta.x * scale.x, eventData.delta.y * scale.y);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var hitCollider = Physics2D.OverlapPoint(eventData.position);
            if (_isDraggable)
            {
                if (hitCollider != null && hitCollider.CompareTag("Customer"))
                {
                    CustomerBehavior customer = hitCollider.transform.parent.GetComponent<CustomerBehavior>();
                    if (!customer.IsAccepted()) return;
                    customer.CheckServedSkewer(this);
                    Destroy(gameObject);
                }
                else
                {
                    _rectTransform.DOAnchorPos(_originalPosition, 0.5F).SetEase(Ease.OutCubic);
                }
            }
        }

        public void SetSkewerFocused(bool isFocused)
        {
            _isSkewerFocused = isFocused;
        }

        public void SwitchToPackUp()
        {
            _isDraggable = true;
            transform.Find("Candy").gameObject.SetActive(false);
            transform.Find("Package").gameObject.SetActive(true);
        }
    }
}