using System.Collections.Generic;
using Script.ingredient;
using Script.setting;

namespace Script.skewer
{
    public struct BlendedCandy
    {
        public int SugarAmount;
        public List<Ingredient> Ingredients;
        public List<IngredientManager.ThirdIngredient> ThirdIngredients;
    }
}