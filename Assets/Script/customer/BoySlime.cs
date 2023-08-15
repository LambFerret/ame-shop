using UnityEngine;

namespace Script.customer
{
    [CreateAssetMenu(fileName = "Boy Slime", menuName = "Scriptable Objects/Customer/Boy Slime")]
    public class BoySlime : Customer
    {
        public BoySlime() : base(
            "BoySlime",
            false,
            120,
            true
        )
        {
        }
    }
}