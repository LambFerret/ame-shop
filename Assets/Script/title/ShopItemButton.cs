using System;
using Script.events;
using Script.ingredient;
using Script.persistence;
using Script.persistence.data;
using Script.setting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.title
{
    public class ShopItemButton : MonoBehaviour, IDataPersistence
    {
        private Ingredient _ingredient;
        public int cost;
        public int value;

        [Header("Image")]
        [SerializeField] private Color onColor = Color.green;
        [SerializeField] private Color offColor = Color.red;
        private Toggle _toggle;
        private Image _image;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _image = transform.Find("image").GetComponent<Image>();
           _text = transform.Find("stock").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _image.color = _toggle.isOn ? onColor : offColor;
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
            GameEventManager.Instance.OnIngredientChanged += OnIngredientChanged;
        }

        public void SetIngredient(Ingredient ingredient)
        {
            _ingredient = ingredient;
            _text.text = "stock : " + value;
            cost = _ingredient.costWhenResultShop;
            transform.Find("cost").GetComponent<TextMeshProUGUI>().text = cost.ToString();
            transform.Find("each").GetComponent<TextMeshProUGUI>().text = "X " + _ingredient.piecePerEach;

        }

        private void OnDestroy()
        {
            GameEventManager.Instance.OnIngredientChanged -= OnIngredientChanged;
        }

        private void OnToggleValueChanged(bool isOn)
        {
            _image.color = isOn ? onColor : offColor;
        }

        private void OnIngredientChanged(Ingredient f, int value)
        {
            if (f == _ingredient)
            {
                this.value += value;
                _text.text = "stock : " + this.value;
            }
        }

        public void Confirm()
        {
            value += _ingredient.piecePerEach;
        }

        public void LoadData(GameData data)
        {
            value = data.ingredients[IngredientManager.Instance.GetIngredientIndex(_ingredient)];
        }

        public void SaveData(GameData data)
        {
            data.ingredients[IngredientManager.Instance.GetIngredientIndex(_ingredient)] = value;
        }
    }
}