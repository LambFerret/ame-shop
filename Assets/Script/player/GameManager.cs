using System;
using System.Collections.Generic;
using Script.customer;
using Script.skewer;
using Script.stage;
using UnityEngine;

namespace Script.player
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public StageController stageController;
        public SkewerController skewerController;

        public GameObject gamePausedPanel;
        public List<Customer> customers;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            stageController = transform.Find("StageController").GetComponent<StageController>();
            skewerController = transform.Find("SkewerController").GetComponent<SkewerController>();
            stageController.gameManager = this;
            skewerController.gameManager = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.timeScale == 0)
                {
                    gamePausedPanel.SetActive(false);
                    Time.timeScale = 1;
                }
                else
                {
                    gamePausedPanel.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }

        public Customer GetCustomer(string id)
        {
            foreach (Customer customer in customers)
            {
                if (customer.id == id)
                {
                    return customer;
                }
            }

            return null;
        }

        public Customer GetRandomCustomer()
        {
            return customers[UnityEngine.Random.Range(0, customers.Count)];
        }

        public Customer PopCustomer(string id)
        {
            foreach (Customer customer in customers)
            {
                if (customer.id == id)
                {
                    customers.Remove(customer);
                    return customer;
                }
            }

            return null;
        }

        public Customer PopRandomCustomer()
        {
            int index = UnityEngine.Random.Range(0, customers.Count);
            Customer customer = customers[index];
            customers.RemoveAt(index);
            return customer;
        }

        public void CreateSkewer()
        {
            skewerController.CreateNewSkewer();
        }
    }
}