using ingredient;
using manager;
using player;
using UnityEngine;
using UnityEngine.UI;

namespace skewer
{
    public class MachineItemGroup : MonoBehaviour
    {
        public SkewerController skewer;

        private void Start()
        {
            foreach (Ingredient i in IngredientManager.Instance.ingredients)
            {
                GameObject itemButton = Instantiate(
                    Resources.Load<GameObject>("Prefabs/MachineItemButton/" + i.ingredientId),
                    transform);
                MachineItemButton button = itemButton.GetComponent<MachineItemButton>();
                button.SetIngredient(i);
                itemButton.transform.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    if (skewer.AddIngredientToSkewerInHand(i)) button.amount--;
                });
            }

            DataPersistenceManager.Instance.LoadGame();
        }
    }
}