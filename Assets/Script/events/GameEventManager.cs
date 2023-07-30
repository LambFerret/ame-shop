using System;
using Script.setting;
using UnityEngine;

namespace Script.events
{
    public class GameEventManager : MonoBehaviour
    {
        public static GameEventManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Game Events Manager in the scene.");
            }


            Instance = this;
        }

        public event Action<int> OnPopularityChanged;

        public void PopularityChanged(int value)
        {
            OnPopularityChanged?.Invoke(value);
        }

        public event Action<int> OnMoneyChanged;

        public void MoneyChanged(int value)
        {
            OnMoneyChanged?.Invoke(value);
        }

        public event Action<IngredientManager.FirstIngredient, int> OnIngredientChanged;

        public void IngredientChanged(IngredientManager.FirstIngredient ingredient, int count)
        {
            OnIngredientChanged?.Invoke(ingredient, count);
        }
    }
}