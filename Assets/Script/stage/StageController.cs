using System;
using System.Collections;
using System.Collections.Generic;
using Script.customer;
using Script.player;
using Script.ui;
using UnityEngine;

namespace Script.stage
{
    public class StageController : MonoBehaviour
    {
        public float stageTimer;
        public List<Customer> waitList;
        public GameObject waitLine;
        public Timer timerObject;

        private GameManager _gameManager;
        private List<GameObject> _waitCustomerObjectList;
        private float _customerTimer;
        private const float CustomerTimer = 30f;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            stageTimer = 0;
            _customerTimer = 0;
        }

        private IEnumerator AddCustomer()
        {
            _customerTimer = 0;
            waitList.Add(_gameManager.GetRandomCustomer());
            yield return new WaitForSeconds(CustomerTimer);
        }

        private void Update()
        {
            StartCoroutine(AddCustomer());
            stageTimer += Time.deltaTime;
            _customerTimer += Time.deltaTime;
            if (_customerTimer >= CustomerTimer && waitList.Count < 3) StartCoroutine(AddCustomer());
            timerObject.SetTimer(stageTimer);
        }
    }
}