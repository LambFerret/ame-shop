using System.Collections.Generic;
using Script.setting;
using UnityEngine;

namespace Script.customer
{
    [CreateAssetMenu(fileName = "Boy", menuName = "Scriptable Objects/Customer/Boy")]
    public class Boy : Customer
    {
        public Boy() : base(
            "Boy",
            false,
            120,
            3,
            -3,
            10,
            5,
            4
        )
        {
        }
    }
}