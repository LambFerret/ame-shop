using System;
using System.Collections;
using DG.Tweening;
using Script.customer;
using Script.player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Timer = Script.ui.Timer;

namespace Script.stage
{
    public class StageController : MonoBehaviour
    {
        public GameObject machineScene;
        public CustomerManager customerManager;

        public float stageTimer;
        public Timer timerObject;
        public GameObject[] waitCustomerObjectList = new GameObject[3];
        public Customer[] customerList = new Customer[3];

        [Header("Customer Interval")] public float baseInterval = 30f; // base interval time when popularity is 0
        public float minInterval = 5f; // minimum possible interval time

        [SerializeField] private bool[] _waitLineOccupied = new bool[3];
        private bool _isGameStarted;

        private void Start()
        {
            customerManager = GameObject.Find("CustomerManager").GetComponent<CustomerManager>();
            stageTimer = 0;
        }

        private void Update()
        {
            stageTimer += Time.deltaTime;
            timerObject.SetTimer(stageTimer);
            foreach (var o in waitCustomerObjectList)
            {
                _waitLineOccupied[Array.IndexOf(waitCustomerObjectList, o)] = o.activeSelf;
            }
        }

        private IEnumerator AddCustomer()
        {
            yield return new WaitForSeconds(1);
            while (true)
            {
                var availableLine = Array.IndexOf(_waitLineOccupied, false);
                if (availableLine != -1)
                {
                    // Customer 객체 랜덤 생성
                    var pickedCustomer = customerManager.GetCustomerByDifficulty(CustomerManager.Difficulty.Easy);
                    customerList[availableLine] = pickedCustomer;

                    // 빈 슬롯 정보 가져오기
                    var emptySlot = waitCustomerObjectList[availableLine];

                    // 빈 자리 여부
                    _waitLineOccupied[availableLine] = true;
                    emptySlot.SetActive(true);
                    emptySlot.GetComponent<CustomerBehavior>().SetScript(pickedCustomer);
                }

                float interval =
                    UnityEngine.Random.Range(minInterval,
                        baseInterval); // random interval time between minInterval and maxInterval

                yield return new WaitForSeconds(interval);
            }
        }

        public void SwitchCashierMachine(bool isCashierScene)
        {
            if (isCashierScene)
            {
                var sb = machineScene.transform.Find("Scrollbar").GetComponent<Scrollbar>();
                DOTween.To(() => sb.value, x => sb.value = x, 0, 1f).SetEase(Ease.InCirc);

                machineScene.transform.DOMove(new Vector3(Screen.width, 0, 0), 1f).SetEase(Ease.Linear);
            }
            else
            {
                machineScene.transform.DOMove(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutBounce);
            }
        }

        public void GotToMachineStep(int step)
        {
            if (step is < 0 or > 2) return;
            var sb = machineScene.transform.Find("Scrollbar").GetComponent<Scrollbar>();
            DOTween.To(() => sb.value, x => sb.value = x, step * 0.5f, 1f).SetEase(Ease.OutBack);
        }


        public void OpenStore()
        {
            if (_isGameStarted) return;
            _isGameStarted = true;
            StartCoroutine(AddCustomer());
        }
    }
}