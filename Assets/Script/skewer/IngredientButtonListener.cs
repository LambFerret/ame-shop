using System;
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
            _button.onClick.AddListener(() =>
            {
                if (skewer.AddFirstIngredientToSkewerInHand(selectedIngredient)) amount--;
            });
        }

        private void Update()
        {
            text.text = selectedIngredient.ToString() + amount;
            if (amount <= 0) _button.interactable = false;

        }

        public void LoadData(GameData data)
        {
            amount = selectedIngredient switch
            {
                IngredientManager.FirstIngredient.Strawberry => data.strawberry,
                IngredientManager.FirstIngredient.Banana => data.banana,
                IngredientManager.FirstIngredient.GreenGrape => data.greenGrape,
                IngredientManager.FirstIngredient.Apple => data.apple,
                IngredientManager.FirstIngredient.BigGrape => data.bigGrape,
                IngredientManager.FirstIngredient.Coconut => data.coconut,
                _ => -1
            };
        }

        public void SaveData(GameData data)
        {
            switch (selectedIngredient)
            {
                case IngredientManager.FirstIngredient.Strawberry:
                    data.strawberry = amount;
                    break;
                case IngredientManager.FirstIngredient.Banana:
                    data.banana = amount;
                    break;
                case IngredientManager.FirstIngredient.GreenGrape:
                    data.greenGrape = amount;
                    break;
                case IngredientManager.FirstIngredient.Apple:
                    data.apple = amount;
                    break;
                case IngredientManager.FirstIngredient.BigGrape:
                    data.bigGrape = amount;
                    break;
                case IngredientManager.FirstIngredient.Coconut:
                    data.coconut = amount;
                    break;
                default:
                    amount = -1;
                    break;
            }
        }
    }
}