using TMPro;
using UnityEngine;

namespace skewer
{
    public class ToppingItemButton : MonoBehaviour
    {
        public SkewerController skewer;

        public TextMeshProUGUI text;
        // public IngredientManager.ThirdIngredient selectedIngredient;

        private void Start()
        {
            // text.text = selectedIngredient.ToString();
            // GetComponent<Button>().onClick.AddListener(() => skewer.AddThirdIngredient(selectedIngredient));
        }
    }
}