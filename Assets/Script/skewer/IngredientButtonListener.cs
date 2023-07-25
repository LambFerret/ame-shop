using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.skewer
{
    public class IngredientButtonListener : MonoBehaviour
    {
        public SkewerController skewer;
        public FirstIngredient selectedIngredient;
        public TextMeshProUGUI text;

        private void Start()
        {
            text.text = selectedIngredient.ToString();
            GetComponent<Button>().onClick.AddListener(() => skewer.AddFirstIngredientToSkewerInHand(selectedIngredient));
        }
    }
}