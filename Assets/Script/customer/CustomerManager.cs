using System.Collections.Generic;
using Script.player;
using UnityEngine;

namespace Script.customer
{
    public class CustomerManager : MonoBehaviour
    {
        public enum Difficulty
        {
            Easy,
            Normal,
            Hard
        }

        public List<Customer> easyCustomer;
        public List<Customer> normalCustomer;
        public List<Customer> hardCustomer;

        public Customer GetCustomerByDifficulty(Difficulty difficulty)
        {
            return (difficulty) switch
            {
                Difficulty.Easy => easyCustomer[Random.Range(0, easyCustomer.Count)],
                Difficulty.Normal => normalCustomer[Random.Range(0, normalCustomer.Count)],
                Difficulty.Hard => hardCustomer[Random.Range(0, hardCustomer.Count)],
                _ => null
            };
        }

    }
}