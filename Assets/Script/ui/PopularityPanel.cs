using Script.player;
using TMPro;
using UnityEngine;

namespace Script.ui
{
    public class PopularityPanel : MonoBehaviour, IPlayerObserver
    {
        public TextMeshProUGUI text;

        private void Start()
        {
            text.text = Player.Instance.playerData.popularity.ToString();
        }

        public void UpdateData(PlayerData playerData)
        {
            text.text = playerData.popularity.ToString();
        }
    }
}