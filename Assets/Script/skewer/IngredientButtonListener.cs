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
    public class IngredientButtonListener : MonoBehaviour, IDataPersistence
    {
        public SkewerController skewer;
        public TextMeshProUGUI text;
        public IngredientManager.FirstIngredient selectedIngredient;
        public int amount;

        private Button _button;

        private Ingredient _ingredient;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            GameEventManager.Instance.OnIngredientChanged += OnIngredientChanged;

            _button.onClick.AddListener(() =>
            {
                if (skewer.AddFirstIngredientToSkewerInHand(selectedIngredient)) amount--;
            });
        }
        private void OnDestroy()
        {
            GameEventManager.Instance.OnIngredientChanged -= OnIngredientChanged;
        }

        private void Update()
        {
            text.text = selectedIngredient.ToString() + amount;
            if (amount <= 0) _button.interactable = false;

        }

        private void OnIngredientChanged(IngredientManager.FirstIngredient f, int value)
        {
            if (f == selectedIngredient)
            {
                amount += value;
            }
        }

        public void LoadData(GameData data)
        {
            amount = data.Ingredients[selectedIngredient];
        }

        public void SaveData(GameData data)
        {
            data.Ingredients[selectedIngredient] = amount;
        }

    }
}