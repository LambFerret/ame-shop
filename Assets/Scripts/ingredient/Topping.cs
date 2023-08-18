using UnityEngine;

namespace ingredient
{
    [CreateAssetMenu(fileName = "New Topping", menuName = "Scriptable Objects/Topping")]
    public class Topping : ScriptableObject
    {
        public string toppingId;
        public GameObject prefab;
        public bool isLiquid;
        public int costPerClick;
        public int popularityPerClick;
    }
}