using Script.setting;
using UnityEngine;

namespace Script.skewer
{
    public class IngredientBehavior : MonoBehaviour
    {
        public enum IngredientStep
        {
            First,
            Second,
            Third
        }

        public IngredientStep ingredientStepType;
        public IngredientStep f;
        public IngredientManager.SecondIngredient s;
        public IngredientManager.ThirdIngredient t;

        public string GetName()
        {
            return ingredientStepType switch
            {
                IngredientStep.First => f.ToString(),
                IngredientStep.Second => s.ToString(),
                IngredientStep.Third => t.ToString(),
                _ => ""
            };
        }
    }
}