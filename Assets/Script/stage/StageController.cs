using System;
using System.Collections;
using Script.customer;
using Script.player;
using Script.ui;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Script.stage
{
    public class StageController : MonoBehaviour
    {
        public GameObject userInterfaces;
        public GameObject cashierScene;
        public GameObject machineScene;
        public GameManager gameManager;
        public CanvasAutoScaler canvasAutoScaler;

        public float stageTimer;
        public Timer timerObject;
        public GameObject[] waitCustomerObjectList = new GameObject[3];
        public Customer[] customerList = new Customer[3];

        private readonly bool[] _waitLineOccupied = new bool[3];
        private const float CustomerTimer = 30f;
        private bool _isGameStarted;
        private bool _isCashierScene;

        private void Start()
        {
            _isCashierScene = true;
            stageTimer = 0;
        }

        private IEnumerator AddCustomer()
        {
            yield return new WaitForSeconds(1);
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
                    emptySlot.GetComponent<CustomerBehavior>().SetScript(pickedCustomer);
                }

                yield return new WaitForSeconds(30);
            }
        }

        public void SwitchCashierMachine(bool isCashierScene)
        {
            _isCashierScene = isCashierScene;
            if (isCashierScene)
            {
                Scrollbar sb = machineScene.transform.Find("Scrollbar").GetComponent<Scrollbar>();
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
            Scrollbar sb = machineScene.transform.Find("Scrollbar").GetComponent<Scrollbar>();
            DOTween.To(() => sb.value, x => sb.value = x, step * 0.5f, 1f).SetEase(Ease.OutBack);
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