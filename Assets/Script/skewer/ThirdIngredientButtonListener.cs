using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.skewer
{
    public class ThirdIngredientButtonListener : MonoBehaviour
    {
        public SkewerController skewer;
        public ThirdIngredient selectedIngredient;
        public TextMeshProUGUI text;

        private void Start()
        {
            text.text = selectedIngredient.ToString();
            GetComponent<Button>().onClick.AddListener(() => skewer.AddThirdIngredient(selectedIngredient));
        }
    }
}