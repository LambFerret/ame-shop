using UnityEngine;

namespace customer
{
    [CreateAssetMenu(fileName = "Normal", menuName = "Scriptable Objects/Customer/Normal")]
    public class Normal : Customer
    {
        public Normal() : base(
            "Boy",
            false,
            90
        )
        {
        }
    }
}