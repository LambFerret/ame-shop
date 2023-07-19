using System;
using System.Collections;
using Script.customer;
using Script.player;
using Script.ui;
using UnityEngine;
using DG.Tweening;
using Script.skewer;

namespace Script.stage
{
    public class StageController : MonoBehaviour
    {

        public GameObject userInterfaces;
        public GameObject cashierScene;
        public GameObject machineScene;
        public SkewerController skewerController;

        public float stageTimer;
        public Timer timerObject;
        public GameObject[] waitCustomerObjectList = new GameObject[3];
        public Customer[] customerList = new Customer[3];

        private GameManager _gameManager;
        [SerializeField] private bool[] _waitLineOccupied = new bool[3];
        private const float CustomerTimer = 30f;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            stageTimer = 0;
            StartCoroutine(AddCustomer());
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
                    Customer pickedCustomer = _gameManager.GetRandomCustomer();
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
            skewerController.CreateNewSkewer();
        }

        private void Update()
        {
            stageTimer += Time.deltaTime;
            timerObject.SetTimer(stageTimer);
        }
    }
}