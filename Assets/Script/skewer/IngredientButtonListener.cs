using Script.setting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.skewer
{
    public class IngredientButtonListener : MonoBehaviour
    {
        public SkewerController skewer;
        public TextMeshProUGUI text;
        public IngredientManager.FirstIngredient selectedIngredient;

        private void Start()
        {
            text.text = selectedIngredient.ToString();
            GetComponent<Button>().onClick
                .AddListener(() => skewer.AddFirstIngredientToSkewerInHand(selectedIngredient));
        }
    }
}