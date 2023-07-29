using UnityEngine;

namespace Script.ingredient
{
    [CreateAssetMenu(fileName = "New Ingredient", menuName = "Scriptable Objects/Ingredient")]
    public class Ingredient : ScriptableObject
    {
        public string ingredientName;
        public GameObject prefab;
        public int cost;
        public int moisture;
        public int size;
        public int piecePerEach;
    }
}