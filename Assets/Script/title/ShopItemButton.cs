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
        public IngredientManager.FirstIngredient ingredient;

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
            _ingredient = GameObject.Find("IngredientManager").GetComponent<IngredientManager>()
                .GetFirstIngredient(ingredient);
            cost = _ingredient.cost;
            transform.Find("cost").GetComponent<TextMeshProUGUI>().text = cost.ToString();
            transform.Find("each").GetComponent<TextMeshProUGUI>().text = "X " + _ingredient.piecePerEach;
            _text = transform.Find("stock").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _image.color = _toggle.isOn ? onColor : offColor;
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
            GameEventManager.Instance.OnIngredientChanged += OnIngredientChanged;
        }

        private void OnDestroy()
        {
            GameEventManager.Instance.OnIngredientChanged -= OnIngredientChanged;
        }

        private void OnToggleValueChanged(bool isOn)
        {
            _image.color = isOn ? onColor : offColor;
        }

        private void OnIngredientChanged(IngredientManager.FirstIngredient f, int value)
        {
            if (f == ingredient)
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
            value = data.ingredients[ingredient];
        }

        public void SaveData(GameData data)
        {
            data.ingredients[ingredient] = value;
        }
    }
}