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
        [Header("Constant Value")] public int maxLength;
        public int perfectTemperatureFrom;
        public int perfectTemperatureTo;
        public int concentrationTolerance;

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

        // 양수면 너무 달음 음수면 너무 밍밍
        public int CheckConcentration()
        {
            if (_currentConcentration >= _perfectConcentration - concentrationTolerance &&
                _currentConcentration <= _perfectConcentration + concentrationTolerance)
            {
                return 0;
            }

            return _currentConcentration - _perfectConcentration;
        }

        public bool CheckTemperature()
        {
            return !(perfectTemperatureFrom > _currentTemperature || _currentTemperature > perfectTemperatureTo);
        }

        public void SetSize(int size)
        {
            maxLength = size;
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
                averageMoisture += ingredient.perfectConcentration;
                totalSize += ingredient.size;
            }

            _perfectConcentration = averageMoisture / _firstIngredients.Count;
            _perfectDryTime = totalSize;

            Debug.Log(" what is perfect dry time : " + _perfectDryTime + " perfect concentration : " +
                      _perfectConcentration);
        }

        //TODO : 가루 사탕도 드셔보시겠습니까?
        public void AddBlendIngredient()
        {
            BlendedCandy blendedCandy;
            if (IsAlreadyBlended)
            {
                blendedCandy = BlendedCandy;
                blendedCandy.FirstIngredients.AddRange(GetFirstIngredients());
                blendedCandy.ThirdIngredients.AddRange(GetThirdIngredients());
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

        public int GetSkewerPrice()
        {
            var ingredient = GetDominantIngredient(_firstIngredients);
            return ingredient.pricePerOne * _firstIngredients.Count(i => i.ingredientId == ingredient.ingredientId);
        }

        public bool IsDominantIngredient(IngredientManager.FirstIngredient ingredient)
        {
            return GetDominantIngredient(_firstIngredients).ingredientId == ingredient.ToString();
        }

        public static Ingredient GetDominantIngredient(IEnumerable<Ingredient> ingredients)
        {
            // Group the ingredients by their name and calculate the total size for each group
            var ingredientGroups = ingredients.GroupBy(i => i.ingredientId)
                .Select(g => new
                {
                    Ingredient = g.First(),
                    TotalSize = g.Sum(i => i.size)
                })
                .ToList();

            // Calculate the total size of all ingredients
            int totalSize = ingredientGroups.Sum(g => g.TotalSize);

            // Find a group where the total size is 80% or more of the total size
            var dominantGroup = ingredientGroups.FirstOrDefault(g => g.TotalSize >= totalSize * 0.8f);

            // If such a group exists, return the ingredient, otherwise return null
            return dominantGroup?.Ingredient;
        }

        private void DebugReadFirstIngredients()
        {
            string listString = string.Join(" - ", _firstIngredients.Select(i => i.ingredientId));
            Debug.Log($"[{listString}]");
        }

        public string GetFirstIngredientText()
        {
            return string.Join(" - ", _firstIngredients.Select(i => i.ingredientId));
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