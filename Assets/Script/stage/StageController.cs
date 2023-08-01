using System;
using System.Collections;
using DG.Tweening;
using Script.customer;
using Script.title;
using Script.ui;
using UnityEngine;
using UnityEngine.UI;

namespace Script.stage
{
    public class StageController : MonoBehaviour
    {
        public GameObject machineScene;
        public CustomerManager customerManager;

        public Timer timer;
        public GameObject[] waitCustomerObjectList = new GameObject[3];
        public Customer[] customerList = new Customer[3];
        public GameObject customerAlarm;

        [Header("Customer Interval")] public float baseInterval = 30f;
        public float minInterval = 5f;

        private readonly bool[] _waitLineOccupied = new bool[3];

        private void Start()
        {
            customerManager = GameObject.Find("CustomerManager").GetComponent<CustomerManager>();
        }

        private void Update()
        {
            if (timer.isEnded)
            {
                if (_waitLineOccupied[0] == false && _waitLineOccupied[1] == false && _waitLineOccupied[2] == false)
                {
                    timer.isStarted = false;
                    timer.isEnded = false;
                    StartCoroutine(EndDay());
                }
            }

            foreach (var o in waitCustomerObjectList)
            {
                _waitLineOccupied[Array.IndexOf(waitCustomerObjectList, o)] = o.activeSelf;
            }
        }

        private IEnumerator AddCustomer()
        {
            yield return new WaitForSeconds(1);
            while (timer.isStarted && !timer.isEnded)
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

                    var alarm = customerAlarm.GetComponent<Image>();
                    alarm.DOKill();
                    alarm.DOFade(0, 0.5F).SetLoops(2 * 2, LoopType.Yoyo).SetEase(Ease.InOutSine);
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

        private static IEnumerator EndDay()
        {
            yield return new WaitForSeconds(1);
            LoadingScreen.Instance.LoadScene("ResultScene");
        }

        public void GotToMachineStep(int step)
        {
            if (step is < 0 or > 2) return;
            var sb = machineScene.transform.Find("Scrollbar").GetComponent<Scrollbar>();
            DOTween.To(() => sb.value, x => sb.value = x, step * 0.5f, 1f).SetEase(Ease.OutBack);
        }


        public void OpenStore()
        {
            if (timer.isStarted) return;
            timer.isStarted = true;
            StartCoroutine(AddCustomer());
        }
    }
}