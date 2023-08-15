using System.Collections.Generic;
using Script.ingredient;
using Script.player;
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
        public List<Ingredient> ingredients;
        public static IngredientManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) Destroy(this);

            Instance = this;
        }

        public Ingredient GetRandomIngredient()
        {
            return ingredients[Random.Range(0, ingredients.Count)];
        }

        public int GetIngredientIndex(Ingredient type)
        {
            return ingredients.IndexOf(type);
        }
    }
}