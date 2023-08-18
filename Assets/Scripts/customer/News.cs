using UnityEngine;

namespace customer
{
    [CreateAssetMenu(fileName = "News", menuName = "Scriptable Objects/News")]
    public class News : ScriptableObject
    {
        public string title;
        public string content;
        public Sprite image;
        public int dayMin;
        public int dayMax;
    }
}