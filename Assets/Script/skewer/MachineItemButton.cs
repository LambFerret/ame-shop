using Script.events;
using Script.ingredient;
using Script.persistence;
using Script.persistence.data;
using Script.setting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.skewer
{
    public class MachineItemButton : MonoBehaviour, IDataPersistence
    {
        public int amount;

        private Button _button;
        private TextMeshProUGUI _text;
        private Ingredient _ingredient;

        private void Awake()
        {
            // _text = transform.Find("butt").Find("text").GetComponent<TextMeshProUGUI>();
            _button = transform.Find("Button").GetComponent<Button>();
            _text = _button.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            GameEventManager.Instance.OnIngredientChanged += OnIngredientChanged;
        }

        public void SetIngredient(Ingredient ingredient)
        {
            _ingredient = ingredient;
        }

        private void OnDestroy()
        {
            GameEventManager.Instance.OnIngredientChanged -= OnIngredientChanged;
        }

        private void Update()
        {
            _text.text = _ingredient.ingredientId + amount;
            _button.interactable = amount > 0;
        }

        private void OnIngredientChanged(Ingredient f, int value)
        {
            if (f == _ingredient)
            {
                amount += value;
            }
        }

        public void LoadData(GameData data)
        {
            amount = data.ingredients[IngredientManager.Instance.GetIngredientIndex(_ingredient)];
        }

        public void SaveData(GameData data)
        {
            data.ingredients[IngredientManager.Instance.GetIngredientIndex(_ingredient)] = amount;
        }
    }
}