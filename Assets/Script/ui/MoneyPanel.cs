using Script.player;
using TMPro;
using UnityEngine;

namespace Script.ui
{
    public class MoneyPanel : MonoBehaviour, IPlayerObserver
    {
        public TextMeshProUGUI text;

        private void Start()
        {
            text.text = Player.Instance.playerData.money.ToString();
        }

        public void UpdateData(PlayerData playerData)
        {
            text.text = playerData.money.ToString();
        }
    }
}