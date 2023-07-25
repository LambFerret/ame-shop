using Script.setting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.skewer
{
    public class ThirdIngredientButtonListener : MonoBehaviour
    {
        public SkewerController skewer;
        public TextMeshProUGUI text;
        public IngredientManager.ThirdIngredient selectedIngredient;

        private void Start()
        {
            text.text = selectedIngredient.ToString();
            GetComponent<Button>().onClick.AddListener(() => skewer.AddThirdIngredient(selectedIngredient));
        }
    }
}