using System;
using System.Collections;
using Script.customer;
using Script.player;
using Script.ui;
using UnityEngine;
using DG.Tweening;

namespace Script.stage
{
    public class StageController : MonoBehaviour
    {
        public GameObject userInterfaces;
        public GameObject cashierScene;
        public GameObject machineScene;
        public GameManager gameManager;

        public float stageTimer;
        public Timer timerObject;
        public GameObject[] waitCustomerObjectList = new GameObject[3];
        public Customer[] customerList = new Customer[3];

        private readonly bool[] _waitLineOccupied = new bool[3];
        private const float CustomerTimer = 30f;
        private bool _isGameStarted;

        private void Start()
        {
            stageTimer = 0;
        }

        private IEnumerator AddCustomer()
        {
            yield return new WaitForSeconds(5);
            while (true)
            {
                int availableLine = Array.IndexOf(_waitLineOccupied, false);
                if (availableLine != -1)
                {
                    // 빈 자리 여부
                    _waitLineOccupied[availableLine] = true;

                    // Customer 객체 랜덤 생성
                    Customer pickedCustomer = gameManager.GetRandomCustomer();
                    customerList[availableLine] = pickedCustomer;

                    // 빈 슬롯 정보 가져오기
                    var emptySlot = waitCustomerObjectList[availableLine];
                    emptySlot.SetActive(true);

                    CustomerBehavior customerGameObjectScript =
                        emptySlot.transform.Find("Customer").GetComponent<CustomerBehavior>();
                    customerGameObjectScript.SetScript(pickedCustomer);

                    // newCustomer.GetComponent<Customer>().OnCustomerClicked += () =>
                    // {
                    //     _waitLineOccupied[availableLine] = false;
                    //     Destroy(newCustomer);
                    // };
                }

                yield return new WaitForSeconds(30);
            }
        }

        public void GoToMachine()
        {
            machineScene.transform.DOMove(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutBounce);
        }

        public void GotToMachineStep(int step)
        {
            if (step is < 0 or > 2) return;
            Debug.Log("step : " + step);
            machineScene.transform.DOMove(new Vector3(2560 * -step, 0, 0), 1F).SetEase(Ease.OutBack);
        }

        public void OpenStore()
        {
            StartCoroutine(AddCustomer());
        }

        private void Update()
        {
            stageTimer += Time.deltaTime;
            timerObject.SetTimer(stageTimer);
        }
    }
}