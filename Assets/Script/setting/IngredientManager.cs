using System.Collections.Generic;
using Script.ingredient;
using Script.player;
using Script.skewer;
using UnityEngine;

namespace Script.setting
{
    public class IngredientManager : MonoBehaviour
    {
        public enum FirstIngredient
        {
            BlendedCandy,
            Strawberry,
            Banana,
            GreenGrape,
            Apple,
            BigGrape,
            Coconut
        }

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

        public List<Ingredient> firstIngredients;

        public GameObject GetFirstIngredientPrefab(FirstIngredient type)
        {
            foreach (var ingredient in firstIngredients)
            {
                if (ingredient.ingredientId.Equals(type.ToString()))
                {
                    return ingredient.prefab;
                }
            }

            return null;
        }

        public FirstIngredient GetRandomFirstIngredient()
        {
            return (FirstIngredient) Random.Range(0, firstIngredients.Count);
        }

        public Ingredient GetFirstIngredient(FirstIngredient type)
        {
            foreach (var ingredient in firstIngredients)
            {
                if (ingredient.ingredientId.Equals(type.ToString()))
                {
                    return ingredient;
                }
            }

            return null;
        }
    }
}