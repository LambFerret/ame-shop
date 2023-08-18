using System;
using ingredient;
using UnityEngine;

namespace manager
{
    public class GameEventManager : MonoBehaviour
    {
        public static GameEventManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) Debug.LogError("Found more than one Game Events Manager in the scene.");
            Instance = this;
        }


        public event Action<float> OnPopularityChanged;

        public void PopularityChanged(float value)
        {
            OnPopularityChanged?.Invoke(value);
        }

        public event Action<int> OnMoneyChanged;

        public void MoneyChanged(int value)
        {
            OnMoneyChanged?.Invoke(value);
        }

        public event Action<Ingredient, int> OnIngredientChanged;

        public void IngredientChanged(Ingredient ingredient, int count)
        {
            OnIngredientChanged?.Invoke(ingredient, count);
        }
    }
}