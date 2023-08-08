using System.Collections.Generic;
using Script.ingredient;
using Script.player;
using Script.skewer;
using UnityEngine;

namespace Script.setting
{
    public class IngredientManager : MonoBehaviour
    {

        public enum SecondIngredient
        {
            NormalSugar,
            ExtraSugar,
            None
        }

        public enum ThirdIngredient
        {
            ChocoChip,
            None
        }

        public GameManager gameManager;
        public static IngredientManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }

            Instance = this;
        }
        public List<Ingredient> Ingredients;

        public Ingredient GetRandomIngredient()
        {
            return Ingredients[Random.Range(0, Ingredients.Count)];
        }

        public int GetIngredientIndex(Ingredient type)
        {
            return Ingredients.IndexOf(type);
        }
    }
}