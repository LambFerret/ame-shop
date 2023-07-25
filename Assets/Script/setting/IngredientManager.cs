using System.Collections.Generic;
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
        public List<GameObject> firstIngredientPrefab;

        public GameObject GetFirstIngredientPrefab(FirstIngredient type)
        {
            foreach (var prefab in firstIngredientPrefab)
            {
                var nameStr = prefab.GetComponent<IngredientBehavior>().GetName();
                if (nameStr.Equals(type.ToString()))
                {
                    return prefab;
                }
            }

            return null;
        }

        public FirstIngredient GetRandomFirstIngredient()
        {
            return (FirstIngredient) Random.Range(0, firstIngredientPrefab.Count);
        }
    }
}