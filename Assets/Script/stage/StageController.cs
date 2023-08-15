using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Script.customer;
using Script.events;
using Script.persistence;
using Script.persistence.data;
using Script.title;
using Script.ui;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.stage
{
    public class StageController : MonoBehaviour, IDataPersistence
    {
        public enum TutorialStep
        {
            // 해야하는것 위주
            AcceptCustomer,
            GetSkewer,
            SkewIngredients,
            BoilerControl,
            PutIntoSugar,
            Dry,
            DryOut,
            Topping,
            Pack,
            GiveResult
        }

        public GameObject machineScene;
        public CustomerManager customerManager;
        public StageStartButton stageStartButton;
        public StageStartButton tutorialButton;

        public Timer timer;
        public GameObject[] waitCustomerObjectList = new GameObject[3];
        public Customer[] customerList = new Customer[3];
        public GameObject customerAlarm;

        [Header("Customer Interval")] public float baseInterval = 15f;
        public float minInterval = 10f;

        public bool[] _waitLineOccupied = new bool[3];
        public bool isCustomerTutorialOngoing;
        public float playerPopularity;

        public TutorialStep currentStepIfTutorial;

        [Header("Tutorial Objects")] public Mover weaponTutorialObject;
        private int _day;
        private bool _isSceneLoading;
        private bool _isSlimeTutorialOngoing;
        private bool _isTutorialDone;

        private void Start()
        {
            customerManager = GameObject.Find("CustomerManager").GetComponent<CustomerManager>();
            weaponTutorialObject.gameObject.SetActive(false);
            machineScene.SetActive(false);
        }

        private void Update()
        {
            if (!_isSceneLoading && timer.isEnded)
                if (GetAvailableLines().Count == 0)
                {
                    timer.isStarted = false;
                    timer.isEnded = false;
                    LoadingScreen.Instance.LoadScene("ResultScene");
                    _isSceneLoading = true;
                }

            for (int i = 0; i < waitCustomerObjectList.Length; i++)
                _waitLineOccupied[i] = waitCustomerObjectList[i].activeSelf;
        }

        public void LoadData(GameData data)
        {
            _isTutorialDone = data.isTutorialDone;
            _day = data.playerLevel;
            playerPopularity = data.popularity;
        }

        public void SaveData(GameData data)
        {
            data.isTutorialDone = _isTutorialDone;
        }

        private IEnumerator AddCustomer()
        {
            yield return new WaitForSeconds(1);
            while (timer.isStarted && !timer.isEnded)
            {
                yield return new WaitForSeconds(4);
                var availableLines = GetAvailableLines();

                if (availableLines.Count == 0) continue;

                // Customer 객체 랜덤 생성
                var pickedCustomer = customerManager.GetCustomerByDifficulty(CustomerManager.Difficulty.Easy);
                AddCustomer(pickedCustomer, availableLines[Random.Range(0, availableLines.Count)]);

                int waitTimeMultiple = availableLines.Count == _waitLineOccupied.Length ? 2 : 1;
                //(10~15초)*{5000/(5000+평판)}
                yield return new WaitForSeconds(Random.Range(minInterval, baseInterval) *
                    (5000 / (5000 + playerPopularity)) / waitTimeMultiple);
            }
        }

        private IEnumerator TutorialStart()
        {
            if (!(timer.isStarted && !timer.isEnded)) yield break;
            yield return new WaitForSeconds(2);

            var availableLines = GetAvailableLines();
            if (availableLines.Count == 0) throw new Exception("No available line In Tutorial?!");

            int availableLine = availableLines[Random.Range(0, availableLines.Count)];

            // Customer 객체 랜덤 생성
            var pickedCustomer = customerManager.GetByName("Boy");
            AddCustomer(pickedCustomer, availableLine);

            isCustomerTutorialOngoing = true;
            while (_waitLineOccupied.Any(occupied => occupied)) yield return null;

            isCustomerTutorialOngoing = false;

            yield return new WaitForSeconds(5);

            _isSlimeTutorialOngoing = true;

            availableLines = GetAvailableLines();
            int slimeLine = availableLines[Random.Range(0, availableLines.Count)];
            var pickedCustomerSlime = customerManager.GetByName("BoySlime");
            AddCustomer(pickedCustomerSlime, slimeLine);
            var obj = waitCustomerObjectList[slimeLine];
            weaponTutorialObject.Activate(obj.transform.position);
            while (_waitLineOccupied.Any(occupied => occupied)) yield return null;

            weaponTutorialObject.gameObject.SetActive(false);

            _isSlimeTutorialOngoing = false;
        }


        private List<int> GetAvailableLines()
        {
            List<int> availableIndices = new List<int>();

            for (int i = 0; i < _waitLineOccupied.Length; i++)
                if (!_waitLineOccupied[i])
                    availableIndices.Add(i);

            return availableIndices;
        }

        private void AddCustomer(Customer customer, int availableLine)
        {
            customerList[availableLine] = customer;

            // 빈 슬롯 정보 가져오기
            var emptySlot = waitCustomerObjectList[availableLine];

            // 빈 자리 여부
            _waitLineOccupied[availableLine] = true;
            emptySlot.SetActive(true);
            emptySlot.GetComponent<CustomerBehavior>().SetScript(customer);

            // 알람 효과
            var alarm = customerAlarm.GetComponent<Image>();
            alarm.DOKill();
            alarm.DOFade(0, 0.5F).SetLoops(2 * 2, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        public void SwitchCashierMachine(bool isCashierScene)
        {
            if (isCashierScene)
            {
                var sb = machineScene.transform.Find("Scrollbar").GetComponent<Scrollbar>();
                DOTween.To(() => sb.value, x => sb.value = x, 0, 1f).SetEase(Ease.InCirc);
                machineScene.transform.DOMove(new Vector3(Screen.width, 0, 0), 1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    machineScene.SetActive(false);
                });
            }
            else
            {
                machineScene.SetActive(true);
                machineScene.transform.DOMove(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutBounce);
            }
        }

        public void GotToMachineStep(int step)
        {
            if (step is < 0 or > 2) return;
            var sb = machineScene.transform.Find("Scrollbar").GetComponent<Scrollbar>();
            DOTween.To(() => sb.value, x => sb.value = x, step * 0.5f, 1f).SetEase(Ease.OutBack);
        }

        public void OpenStore(int day)
        {
            if (timer.isStarted) return;
            timer.isStarted = true;
            stageStartButton.SwitchAnimation(false);
            tutorialButton.SwitchAnimation(false);
            stageStartButton.gameObject.SetActive(false);
            tutorialButton.gameObject.SetActive(false);
            if (day == 1)
                StartCoroutine(TutorialStart());
            else
                StartCoroutine(AddCustomer());
        }
    }
}