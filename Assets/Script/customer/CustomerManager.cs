using System.Collections.Generic;
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

        private List<Customer> _allCustomerList;

        private void Awake()
        {
            _allCustomerList = new List<Customer>();
            _allCustomerList.AddRange(easyCustomer);
            _allCustomerList.AddRange(normalCustomer);
            _allCustomerList.AddRange(hardCustomer);
        }

        public Customer GetCustomerByDifficulty(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => easyCustomer[Random.Range(0, easyCustomer.Count)],
                Difficulty.Normal => normalCustomer[Random.Range(0, normalCustomer.Count)],
                Difficulty.Hard => hardCustomer[Random.Range(0, hardCustomer.Count)],
                _ => null
            };
        }

        public Customer GetByName(string customerName)
        {
            Customer customer = null;
            foreach (var c in _allCustomerList)
                if (c.id == customerName)
                {
                    customer = c;
                    break;
                }

            return customer;
        }
    }
}