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
        public FirstIngredient f;
        public SecondIngredient s;
        public ThirdIngredient t;

        public string GetName()
        {
            return (ingredientType) switch
            {
                Ingredient.First => f.ToString(),
                Ingredient.Second => s.ToString(),
                Ingredient.Third => t.ToString(),
                _ => ""
            };
        }
    }
}