using System.Collections.Generic;
using System.Linq;
using customer;
using DG.Tweening;
using ingredient;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace skewer
{
    public class SkewerBehavior : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Constant Value")] public int maxLength;
        public int perfectTemperatureFrom;
        public int perfectTemperatureTo;
        public int concentrationTolerance;
        public bool IsAlreadyBlended;
        private int _currentConcentration;
        private int _currentDryTime;

        [Header("Coat Candy")] public GameObject cottonCandy;
        public Image cottonCandyImage;
        public GameObject originalCandy;

        private int _currentTemperature;

        private List<Ingredient> _Ingredients;

        private bool _isDraggable;

        private bool _isFirstThirdSecond;
        private bool _isSkewerFocused;
        private Vector3 _originalPosition;

        private int _perfectConcentration;

        private int _perfectDryTime;
        private RectTransform _rectTransform;

        // private List<IngredientManager.ThirdIngredient> _thirdIngredients;
        // public BlendedCandy BlendedCandy;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _Ingredients = new List<Ingredient>();
            cottonCandy = transform.Find("CottonCandy").gameObject;
            cottonCandyImage = cottonCandy.GetComponent<Image>();
            originalCandy = transform.Find("Candy").gameObject;
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

        public bool IsNotEnoughDry()
        {
            return _perfectDryTime > _currentDryTime;
        }

        // 양수면 너무 달음 음수면 너무 밍밍
        public int CheckConcentration()
        {
            if (_currentConcentration >= _perfectConcentration - concentrationTolerance &&
                _currentConcentration <= _perfectConcentration + concentrationTolerance)
                return 0;

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

            foreach (var ingredient in _Ingredients)
            {
                averageMoisture += ingredient.perfectConcentration;
                totalSize += ingredient.size;
            }

            _perfectConcentration = averageMoisture / _Ingredients.Count;
            _perfectDryTime = totalSize;

            Debug.Log(" what is perfect dry time : " + _perfectDryTime + " perfect concentration : " +
                      _perfectConcentration);
        }

        //TODO : 가루 사탕도 드셔보시겠습니까?
        /**
         * public void AddBlendIngredient()
         * {
         * BlendedCandy blendedCandy;
         * if (IsAlreadyBlended)
         * {
         * blendedCandy = BlendedCandy;
         * blendedCandy.Ingredients.AddRange(GetIngredients());
         * blendedCandy.ThirdIngredients.AddRange(GetThirdIngredients());
         * foreach (IngredientManager.SecondIngredient ingredient in GetSecondIngredients())
         * {
         * switch (ingredient)
         * {
         * case IngredientManager.SecondIngredient.NormalSugar:
         * blendedCandy.SugarAmount += 10;
         * break;
         * case IngredientManager.SecondIngredient.ExtraSugar:
         * blendedCandy.SugarAmount += 20;
         * break;
         * case IngredientManager.SecondIngredient.None:
         * break;
         * }
         * }
         * }
         * else
         * {
         * blendedCandy = new BlendedCandy();
         * blendedCandy.Ingredients = GetIngredients();
         * blendedCandy.ThirdIngredients = GetThirdIngredients();
         * // TODO
         * foreach (IngredientManager.SecondIngredient ingredient in GetSecondIngredients())
         * {
         * switch (ingredient)
         * {
         * case IngredientManager.SecondIngredient.NormalSugar:
         * blendedCandy.SugarAmount += 10;
         * break;
         * case IngredientManager.SecondIngredient.ExtraSugar:
         * blendedCandy.SugarAmount += 20;
         * break;
         * case IngredientManager.SecondIngredient.None:
         * break;
         * }
         * }
         * }
         *
         * _Ingredients.Clear();
         * // TODO
         * _secondIngredients.Clear();
         * _thirdIngredients.Clear();
         * _dryTime = 0;
         * _isFirstThirdSecond = false;
         * IsAlreadyBlended = true;
         * _Ingredients.Add(Ingredient.BlendedCandy);
         * }
         */

        // Setting base Ingredients
        public void AddIngredient(Ingredient ingredient)
        {
            _Ingredients.Add(ingredient);
            SkewIngredients(ingredient);

            CalculatePerfect();
            DebugReadIngredients();
        }

        private void SkewIngredients(Ingredient ingredient)
        {
            const float offset = 2.0f;

            var t = transform.Find("Candy");
            var childCount = t.childCount;
            var prefab = Instantiate(ingredient.prefab, t);
            prefab.transform.localPosition = new Vector3(childCount * offset, childCount * offset, 0);
        }

        public List<Ingredient> GetIngredients()
        {
            DebugReadIngredients();
            return _Ingredients;
        }

        public int GetSkewerPrice()
        {
            var ingredient = GetDominantIngredient(_Ingredients);
            return ingredient.pricePerOne * _Ingredients.Count(i => i.ingredientId == ingredient.ingredientId);
        }

        public bool IsDominantIngredient(Ingredient ingredient)
        {
            var a = GetDominantIngredient(_Ingredients)?.ingredientId;
            var b = ingredient.ingredientId;
            Debug.Log("what is dominant of this skewer? : " + a + " and ordered " + b);
            return a == b;
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

        public int GetSize()
        {
            int i = 0;
            foreach (var ing in _Ingredients) i += ing.size;

            return i;
        }

        private void DebugReadIngredients()
        {
            string listString = string.Join(" - ", _Ingredients.Select(i => i.ingredientId));
            Debug.Log($"[{listString}]");
        }

        public string GetIngredientText()
        {
            return string.Join(" - ", _Ingredients.Select(i => i.ingredientId));
        }

        // // TODO setting third ingredient
        // public List<IngredientManager.ThirdIngredient> GetThirdIngredients()
        // {
        //     string listString = string.Join(" - ", _thirdIngredients.Select(i => i.ToString()));
        //     Debug.Log($"[{listString}]");
        //
        //     return _thirdIngredients;
        // }

        // public void AddThirdIngredient(IngredientManager.ThirdIngredient type)
        // {
        // _thirdIngredients.Add(type);
        // GetThirdIngredients();
        // foreach (var prefab in IngredientPrefabs)
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
        // }

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