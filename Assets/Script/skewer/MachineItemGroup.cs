using System;
using System.Collections.Generic;
using Script.ingredient;
using Script.persistence;
using Script.setting;
using UnityEngine;
using UnityEngine.UI;

namespace Script.skewer
{
    public class MachineItemGroup : MonoBehaviour
    {
        public IngredientManager ingredientManager;
        public SkewerController skewer;
        private void Start()
        {
            foreach (Ingredient i in IngredientManager.Instance.Ingredients)
            {
                GameObject itemButton = Instantiate(Resources.Load<GameObject>("Prefabs/MachineItemButton/"+i.ingredientId),
                    transform);
                MachineItemButton button = itemButton.GetComponent<MachineItemButton>();
                button.SetIngredient(i);
                itemButton.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (skewer.AddIngredientToSkewerInHand(i)) button.amount--;
                });
            }
            DataPersistenceManager.Instance.LoadGame();
        }
    }
}