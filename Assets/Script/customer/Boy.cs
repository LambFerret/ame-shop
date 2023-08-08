using UnityEngine;

namespace Script.customer
{
    [CreateAssetMenu(fileName = "Boy", menuName = "Scriptable Objects/Customer/Boy")]
    public class Boy : Customer
    {
        public Boy() : base(
            "Boy",
            false,
            120
        )
        {
        }
    }
}