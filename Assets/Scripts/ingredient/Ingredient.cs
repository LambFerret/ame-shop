using UnityEngine;

namespace ingredient
{
    [CreateAssetMenu(fileName = "New Ingredient", menuName = "Scriptable Objects/Ingredient")]
    public class Ingredient : ScriptableObject
    {
        public string ingredientId;
        public GameObject prefab;
        public int size;
        public int pricePerOne;
        public int perfectConcentration;
        public int costWhenResultShop;
        public int piecePerEach;
    }
}