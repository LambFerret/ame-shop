using System.Collections.Generic;
using Script.setting;

namespace Script.skewer
{
    public struct BlendedCandy
    {
        public int SugarAmount;
        public List<IngredientManager.FirstIngredient> FirstIngredients;
        public List<IngredientManager.ThirdIngredient> ThirdIngredients;
    }
}