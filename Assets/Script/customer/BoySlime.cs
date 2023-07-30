using System.Collections.Generic;
using Script.setting;
using UnityEngine;

namespace Script.customer
{
    [CreateAssetMenu(fileName = "Boy Slime", menuName = "Scriptable Objects/Customer/Boy Slime")]
    public class BoySlime : Customer
    {
        public BoySlime() : base(
            "BoySlime",
            false,
            100,
            3,
            -3,
            10,
            5,
            4,
            new List<IngredientManager.FirstIngredient> { IngredientManager.FirstIngredient.GreenGrape },
            true,
            IngredientManager.FirstIngredient.Strawberry,
            20
        )
        {
        }
    }
}