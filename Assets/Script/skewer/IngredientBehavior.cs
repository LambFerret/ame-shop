using Script.setting;
using UnityEngine;

namespace Script.skewer
{
    public class IngredientBehavior : MonoBehaviour
    {
        public enum Ingredient
        {
            First,
            Second,
            Third
        }

        public Ingredient ingredientType;
        public IngredientManager.FirstIngredient f;
        public IngredientManager.SecondIngredient s;
        public IngredientManager.ThirdIngredient t;

        public string GetName()
        {
            return ingredientType switch
            {
                Ingredient.First => f.ToString(),
                Ingredient.Second => s.ToString(),
                Ingredient.Third => t.ToString(),
                _ => ""
            };
        }
    }
}