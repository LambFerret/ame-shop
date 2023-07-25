using System.Collections.Generic;
using Script.setting;
using UnityEngine;

namespace Script.customer
{
    [CreateAssetMenu(fileName = "Boy", menuName = "Scriptable Objects/Customer/Boy")]
    public class Boy : Customer
    {
        public Boy() : base(
            "Boy",
            false,
            100,
            3,
            -3,
            10,
            5,
            4,
            new List<IngredientManager.FirstIngredient> { IngredientManager.FirstIngredient.GreenGrape },
            new List<IngredientManager.SecondIngredient> { IngredientManager.SecondIngredient.NormalSugar },
            new List<IngredientManager.ThirdIngredient> { IngredientManager.ThirdIngredient.None }
        )
        {
        }
    }
}