using System.Collections.Generic;
using Script.skewer;
using UnityEngine;

namespace Script.customer
{
    [CreateAssetMenu(fileName = "Boy", menuName = "Scriptable Objects/Customer/Boy")]
    public class Boy : Customer
    {
        public Boy() : base(
            id: "Boy",
            hasSpecialEvent: false,
            patience: 100,
            maxPopularity: 3,
            minPopularity: -3,
            scoreLimitation: 10,
            maxMoney: 5,
            minMoney: 4,
            firstIngredients: new List<FirstIngredient>() { FirstIngredient.GreenGrape },
            secondIngredients: new List<SecondIngredient>() { SecondIngredient.NormalSugar },
            thirdIngredients: new List<ThirdIngredient>() { ThirdIngredient.None }
        )
        {
        }
    }
}