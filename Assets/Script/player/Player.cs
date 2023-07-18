using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.player
{
    public class Player : MonoBehaviour
    {
        public PlayerData playerData;
        private List<IPlayerObserver> _observers;
        private GameManager _instance;
        private void Start()
        {
            _instance = GameManager.Instance;
            _observers = new List<IPlayerObserver>();
        }

        public void AddObserver(IPlayerObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IPlayerObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.UpdateData(playerData);
            }
        }
    }
}