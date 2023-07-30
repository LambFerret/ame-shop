using System;
using Script.events;
using Script.persistence;
using Script.persistence.data;
using Script.player;
using TMPro;
using UnityEngine;

namespace Script.ui
{
    public class PopularityPanel : MonoBehaviour, IDataPersistence
    {
        public TextMeshProUGUI text;

        private int _popularity;

        private void Awake()
        {
            text = transform.Find("text").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            GameEventManager.Instance.OnPopularityChanged += OnPopularityChanged;
        }

        private void OnDestroy()
        {
            GameEventManager.Instance.OnPopularityChanged -= OnPopularityChanged;
        }

        private void Update()
        {
            text.text = _popularity.ToString();
        }

        private void OnPopularityChanged(int value)
        {
            _popularity += value;
        }

        public void LoadData(GameData data)
        {
            _popularity = data.popularity;
        }

        public void SaveData(GameData data)
        {
            data.popularity = _popularity;
        }
    }
}