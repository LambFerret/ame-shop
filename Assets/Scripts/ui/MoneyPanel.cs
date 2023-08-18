using manager;
using player;
using player.data;
using TMPro;
using UnityEngine;

namespace ui
{
    public class MoneyPanel : MonoBehaviour, IDataPersistence
    {
        public TextMeshProUGUI text;
        private int _money;

        private void Awake()
        {
            text = transform.Find("text").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            GameEventManager.Instance.OnMoneyChanged += OnMoneyChanged;
        }

        private void Update()
        {
            text.text = _money.ToString();
        }

        private void OnDestroy()
        {
            GameEventManager.Instance.OnMoneyChanged -= OnMoneyChanged;
        }

        public void LoadData(GameData data)
        {
            _money = data.money;
        }

        public void SaveData(GameData data)
        {
            data.money = _money;
        }

        private void OnMoneyChanged(int value)
        {
            _money += value;
        }
    }
}