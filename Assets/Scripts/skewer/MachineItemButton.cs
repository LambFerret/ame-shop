using ingredient;
using manager;
using player;
using player.data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace skewer
{
    public class MachineItemButton : MonoBehaviour, IDataPersistence
    {
        public int amount;

        private Button _button;
        private Ingredient _ingredient;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _button = transform.Find("Button").GetComponent<Button>();
            _text = _button.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            GameEventManager.Instance.OnIngredientChanged += OnIngredientChanged;
        }

        private void Update()
        {
            _text.text = _ingredient.ingredientId + amount;
            _button.interactable = amount > 0;
        }

        private void OnDestroy()
        {
            GameEventManager.Instance.OnIngredientChanged -= OnIngredientChanged;
        }

        public void LoadData(GameData data)
        {
            amount = data.ingredients[IngredientManager.Instance.GetIngredientIndex(_ingredient)];
        }

        public void SaveData(GameData data)
        {
            data.ingredients[IngredientManager.Instance.GetIngredientIndex(_ingredient)] = amount;
        }

        public void SetIngredient(Ingredient ingredient)
        {
            _ingredient = ingredient;
        }

        private void OnIngredientChanged(Ingredient f, int value)
        {
            if (f == _ingredient) amount += value;
        }
    }
}