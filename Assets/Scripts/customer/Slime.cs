using UnityEngine;

namespace customer
{
    [CreateAssetMenu(fileName = "Slime", menuName = "Scriptable Objects/Customer/Slime")]
    public class Slime : Customer
    {
        public Slime() : base(
            "BoySlime",
            false,
            120,
            true
        )
        {
        }
    }
}