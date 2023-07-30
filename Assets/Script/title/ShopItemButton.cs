using System;
using Script.ingredient;
using Script.persistence;
using Script.persistence.data;
using Script.setting;
using TMPro;
using UnityEngine;

namespace Script.title
{
    public class ShopItemButton : MonoBehaviour, IDataPersistence
    {
        public IngredientManager.FirstIngredient ingredient;

        private Ingredient _ingredient;
        public int cost;
        public int value;

        private void Awake()
        {
            _ingredient = GameObject.Find("IngredientManager").GetComponent<IngredientManager>()
                .GetFirstIngredient(ingredient);
            cost = _ingredient.cost;
            transform.Find("cost").GetComponent<TextMeshProUGUI>().text = cost.ToString();
            transform.Find("each").GetComponent<TextMeshProUGUI>().text = "X " + _ingredient.piecePerEach;
        }

        public void Confirm()
        {
            value += _ingredient.piecePerEach;
        }

        public void LoadData(GameData data)
        {
            value = ingredient switch
            {
                IngredientManager.FirstIngredient.Strawberry => data.strawberry,
                IngredientManager.FirstIngredient.Banana => data.banana,
                IngredientManager.FirstIngredient.GreenGrape => data.greenGrape,
                IngredientManager.FirstIngredient.Apple => data.apple,
                IngredientManager.FirstIngredient.BigGrape => data.bigGrape,
                IngredientManager.FirstIngredient.Coconut => data.coconut,
                _ => -1
            };
            transform.Find("stock").GetComponent<TextMeshProUGUI>().text = "stock : " + value;
        }

        public void SaveData(GameData data)
        {
            switch (ingredient)
            {
                case IngredientManager.FirstIngredient.Strawberry:
                    data.strawberry = value;
                    break;
                case IngredientManager.FirstIngredient.Banana:
                    data.banana = value;
                    break;
                case IngredientManager.FirstIngredient.GreenGrape:
                    data.greenGrape = value;
                    break;
                case IngredientManager.FirstIngredient.Apple:
                    data.apple = value;
                    break;
                case IngredientManager.FirstIngredient.BigGrape:
                    data.bigGrape = value;
                    break;
                case IngredientManager.FirstIngredient.Coconut:
                    data.coconut = value;
                    break;
            }
        }
    }
}