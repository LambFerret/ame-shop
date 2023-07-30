using System;
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
            if (OnPopularityChanged != null)
            {
                OnPopularityChanged(value);
            }
        }

        public event Action<int> OnMoneyChanged;

        public void MoneyChanged(int value)
        {
            if (OnMoneyChanged != null)
            {
                OnMoneyChanged(value);
            }
        }
    }
}