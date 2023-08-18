using System.Collections.Generic;
using ingredient;
using UnityEngine;

namespace manager
{
    public class IngredientManager : MonoBehaviour
    {
        public List<Ingredient> ingredients;
        public static IngredientManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) Destroy(this);

            Instance = this;
        }

        public Ingredient GetRandomIngredient()
        {
            return ingredients[Random.Range(0, ingredients.Count)];
        }

        public int GetIngredientIndex(Ingredient type)
        {
            return ingredients.IndexOf(type);
        }
    }
}