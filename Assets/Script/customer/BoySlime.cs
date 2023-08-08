using System.Collections.Generic;
using Script.setting;
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
            3,
            -3,
            10,
            5,
            4,
            true,
            "Strawberry",
            20
        )
        {
        }
    }
}