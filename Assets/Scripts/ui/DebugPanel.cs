using System;
using System.Collections.Generic;
using player;
using player.data;
using TMPro;
using UnityEngine;

namespace ui
{
    public class DebugPanel : MonoBehaviour, IDataPersistence
    {
        /*
         * 게임 플레이
         * gameStartTime : 게임 시작 시간
         * gameEndTime : 게임 종료 시간
         * gameHourLength : 게임 한 시간 (분)
         *
         * 사탕 제조
         * perfectConcentrationRange : 완벽한 농도 범위
         * perfectTemperatureStart : 완벽한 온도 시작
         * perfectTemperatureEnd : 완벽한 온도 끝
         * perfectDryTime : 완벽한 건조 시간
         *
         * 보일러 손실 계산
         * sugarLossPerCm : 1cm당 설탕 손실량
         * waterLossPerCm : 1cm당 물 손실량
         * temperatureLossPerOnce : 1회당 온도 손실량
         * temperatureLossPerTime : 시간당 온도 손실량
         * waterLossPerTime : 시간당 물 손실량
         * lossTime : 손실이 일어나는 시간 (분)
         *
         * 보일러 성능 계산
         * boilerCapacity : 보일러 용량
         * addPerOnce : 버튼 1회당 설탕,물 추가량
         * fuelAddPerOnce : 버튼 1회당 연료 추가량
         *
         * 꼬치
         * skewerLength : 꼬치 길이
         * skewerMinLength : 최소 꼬치 길이(?)
         *
         * 만족도
         * basicSatisfaction : 기본 만족도
         * satisfactionLossWhenIngredientMistake : 잘못된 재료 사용 시 만족도 감소량
         * satisfactionLossWhenConcentrationIsHigh : 농도가 높을 때 만족도 감소량
         * satisfactionLossWhenConcentrationIsLow : 농도가 낮을 때 만족도 감소량
         * satisfactionLossWhenNeedMoreDry : 건조가 더 필요할 때 만족도 감소량
         * satisfactionLossWhenWaitTooLong : 너무 오래 기다렸을 때 만족도 감소량
         *
         * 평판
         * initialPopularity : 초기 평판
         * popularityLossWhenFail : 실패 시 평판 감소량
         * popularityGainWhenSuccess : 성공 시 평판 증가량
         * popularityLossWhenRefuse : 거절 시 평판 감소량
         * popularityLossWhenPoke : 찌르기 시 평판 감소량
         *
         * 가진거
         * money : 돈
         * cherry : 체리
         * dragonFruit : 용과
         * greenGrape : 청포도
         * mikan : 귤
         * pineapple : 파인애플
         * strawberry : 딸기
         *
         * 결산
         * sugarPrice : 설탕 가격
         * gasPrice : 가스 가격
         * rentPrice : 임대료
         * tax : 세금
         * electricityPrice : 전기세
         *
         *
         */
        public float gameStartTime;
        public float gameEndTime;
        public float gameHourLength;
        public float perfectConcentrationRange;
        public float perfectTemperatureStart;
        public float perfectTemperatureEnd;
        public float perfectDryTime;
        public float sugarLossPerCm;
        public float waterLossPerCm;
        public float temperatureLossPerOnce;
        public float temperatureLossPerTime;
        public float waterLossPerTime;
        public float lossTime;
        public float boilerCapacity;
        public float addLiquidPerOnce;
        public float fuelAddPerOnce;
        public float skewerLength;
        public float skewerMinLength;
        public float basicSatisfaction;
        public float satisfactionLossWhenIngredientMistake;
        public float satisfactionLossWhenConcentrationIsHigh;
        public float satisfactionLossWhenConcentrationIsLow;
        public float satisfactionLossWhenNeedMoreDry;
        public float satisfactionLossWhenWaitTooLong;
        public float initialPopularity;
        public float popularityLossWhenFail;
        public float popularityGainWhenSuccess;
        public float popularityLossWhenRefuse;
        public float popularityLossWhenPoke;
        public float money;
        public float cherry;
        public float dragonFruit;
        public float greenGrape;
        public float mikan;
        public float pineapple;
        public float strawberry;
        public float sugarPrice;
        public float gasPrice;
        public float rentPrice;
        public float tax;
        public float electricityPrice;


        public GameObject contentPrefab;

        private GameObject _contentParent;

        [SerializeField] private List<GameObject> contentList = new();

        private void Awake()
        {
            _contentParent = transform.Find("scroll/Content").gameObject;
        }

        private void MakeContent(string title, string description, List<(string, string)> items)
        {
            var content = Instantiate(contentPrefab, _contentParent.transform);
            contentList.Add(content);
            var contentComponent = content.GetComponent<DebugPanelContent>();
            contentComponent.SetTitle(title, description);
            foreach (var (key, value) in items)
            {
                contentComponent.AddItem(key, value);
            }
        }

        private void Start()
        {
            // 게임 플레이
            MakeContent("게임 플레이", "",// ok
                new List<(string, string)>
                {
                    ("게임 시작 시간", gameStartTime.ToString("F2")),
                    ("게임 종료 시간", gameEndTime.ToString("F2")),
                    ("게임 한 시간 (분)", gameHourLength.ToString("F2")),
                });

            // 사탕 제조
            MakeContent("사탕 제조", "", // ok
                new List<(string, string)>
                {
                    ("완벽한 농도 범위", perfectConcentrationRange.ToString("F2")),
                    ("완벽한 온도 시작", perfectTemperatureStart.ToString("F2")),
                    ("완벽한 온도 끝", perfectTemperatureEnd.ToString("F2")),
                    ("완벽한 건조 시간(안씀) 현재 건조타임은 그냥 길이 그대로임", perfectDryTime.ToString("F2")),
                    // 건조타임 이라는게 이상함 이건 수식임
                    // 현재 건조타임은 그냥 길이 그대로임
                });

            // 보일러 손실 계산
            MakeContent("보일러 손실 계산", "",// ok
                new List<(string, string)>
                {
                    ("1cm당 설탕 손실량", sugarLossPerCm.ToString("F2")),
                    ("1cm당 물 손실량", waterLossPerCm.ToString("F2")),
                    ("1회당 온도 손실량", temperatureLossPerOnce.ToString("F2")),
                    ("시간당 온도 손실량", temperatureLossPerTime.ToString("F2")),
                    ("시간당 물 손실량", waterLossPerTime.ToString("F2")),
                    ("손실이 일어나는 시간 (분)", lossTime.ToString("F2")),
                });

            // 보일러 성능 계산
            MakeContent("보일러 성능 계산", "",// ok
                new List<(string, string)>
                {
                    ("보일러 용량", boilerCapacity.ToString("F2")),
                    ("버튼 1회당 설탕,물 추가량", addLiquidPerOnce.ToString("F2")),
                    ("버튼 1회당 연료 추가량", fuelAddPerOnce.ToString("F2")),
                });

            // 꼬치
            MakeContent("꼬치", "", // ok
                new List<(string, string)>
                {
                    ("꼬치 길이", skewerLength.ToString("F2")),
                    ("최소 꼬치 길이(안씀)", skewerMinLength.ToString("F2")),
                });

            // 만족도
            MakeContent("만족도", "",
                new List<(string, string)>
                {
                    ("기본 만족도", basicSatisfaction.ToString("F2")),
                    ("잘못된 재료 사용 시 만족도 감소량", satisfactionLossWhenIngredientMistake.ToString("F2")),
                    ("농도가 높을 때 만족도 감소량", satisfactionLossWhenConcentrationIsHigh.ToString("F2")),
                    ("농도가 낮을 때 만족도 감소량", satisfactionLossWhenConcentrationIsLow.ToString("F2")),
                    ("건조가 더 필요할 때 만족도 감소량", satisfactionLossWhenNeedMoreDry.ToString("F2")),
                    ("너무 오래 기다렸을 때 만족도 감소량(안씀)", satisfactionLossWhenWaitTooLong.ToString("F2")),
                });

            // 평판
            MakeContent("평판", "",
                new List<(string, string)>
                {
                    ("초기 평판", initialPopularity.ToString("F2")),
                    ("실패 시 평판 감소량", popularityLossWhenFail.ToString("F2")),
                    ("성공 시 평판 증가량", popularityGainWhenSuccess.ToString("F2")),
                    ("거절 시 평판 감소량", popularityLossWhenRefuse.ToString("F2")),
                    ("찌르기 시 평판 감소량", popularityLossWhenPoke.ToString("F2")),
                });

            // 가진거
            MakeContent("가진거", "",
                new List<(string, string)>
                {
                    ("돈", money.ToString("F2")),
                    ("체리", cherry.ToString("F2")),
                    ("용과", dragonFruit.ToString("F2")),
                    ("청포도", greenGrape.ToString("F2")),
                    ("귤", mikan.ToString("F2")),
                    ("파인애플", pineapple.ToString("F2")),
                    ("딸기", strawberry.ToString("F2")),
                });

            // 결산
            MakeContent("결산", "",
                new List<(string, string)>
                {
                    ("설탕 가격", sugarPrice.ToString("F2")),
                    ("가스 가격", gasPrice.ToString("F2")),
                    ("임대료", rentPrice.ToString("F2")),
                    ("세금", tax.ToString("F2")),
                    ("전기세", electricityPrice.ToString("F2")),
                });
        }
        private GameData _tempGameData;
        private GameData _gameData;
        public void StartEdit()
        {
            gameObject.SetActive(true);
            DataPersistenceManager.Instance.LoadGame();
            _tempGameData = _gameData.Clone();
            LoadData(_tempGameData);
        }

        public void Confirm()
        {
            SaveData(_gameData);
            DataPersistenceManager.Instance.SaveGame();
            gameObject.SetActive(false);
        }

        public void Cancel()
        {
            LoadData(_gameData);
            gameObject.SetActive(false);
        }

        public void LoadData(GameData data)
        {
            _gameData = data;
            gameStartTime = data.gameStartTime;
            gameEndTime = data.gameEndTime;
            gameHourLength = data.gameHourLength;
            perfectConcentrationRange = data.perfectConcentrationRange;
            perfectTemperatureStart = data.perfectTemperatureStart;
            perfectTemperatureEnd = data.perfectTemperatureEnd;
            perfectDryTime = data.perfectDryTime;
            sugarLossPerCm = data.sugarLossPerCm;
            waterLossPerCm = data.waterLossPerCm;
            temperatureLossPerOnce = data.temperatureLossPerOnce;
            temperatureLossPerTime = data.temperatureLossPerTime;
            waterLossPerTime = data.waterLossPerTime;
            lossTime = data.lossTime;
            boilerCapacity = data.boilerCapacity;
            addLiquidPerOnce = data.addLiquidPerOnce;
            fuelAddPerOnce = data.fuelAddPerOnce;
            skewerLength = data.skewerLength;
            skewerMinLength = data.skewerMinLength;
            basicSatisfaction = data.basicSatisfaction;
            satisfactionLossWhenIngredientMistake = data.satisfactionLossWhenIngredientMistake;
            satisfactionLossWhenConcentrationIsHigh = data.satisfactionLossWhenConcentrationIsHigh;
            satisfactionLossWhenConcentrationIsLow = data.satisfactionLossWhenConcentrationIsLow;
            satisfactionLossWhenNeedMoreDry = data.satisfactionLossWhenNeedMoreDry;
            satisfactionLossWhenWaitTooLong = data.satisfactionLossWhenWaitTooLong;
            initialPopularity = data.initialPopularity;
            popularityLossWhenFail = data.popularityLossWhenFail;
            popularityGainWhenSuccess = data.popularityGainWhenSuccess;
            popularityLossWhenRefuse = data.popularityLossWhenRefuse;
            popularityLossWhenPoke = data.popularityLossWhenPoke;
            money = data.money;
            cherry = data.cherry;
            dragonFruit = data.dragonFruit;
            greenGrape = data.greenGrape;
            mikan = data.mikan;
            pineapple = data.pineapple;
            strawberry = data.strawberry;
            sugarPrice = data.sugarPrice;
            gasPrice = data.gasPrice;
            rentPrice = data.rentPrice;
            tax = data.tax;
            electricityPrice = data.electricityPrice;
        }

        public void SaveData(GameData data)
        {
            List<float> floatValues = new List<float>();
            foreach (GameObject list in contentList)
            {
                var a = list.GetComponent<DebugPanelContent>().GetFloatList();
                // add all into floatValues
                floatValues.AddRange(a);
            }

            int index = 0; // to iterate through floatValues
            if (floatValues.Count >= contentList.Count) // Make sure there are enough values
            {
                // Assuming each GameObject in contentList corresponds to a float field in GameData
                // You will need to ensure the order in floatValues matches the order of fields in GameData

                data.gameStartTime = floatValues[index++];
                data.gameEndTime = floatValues[index++];
                data.gameHourLength = floatValues[index++];
                data.perfectConcentrationRange = floatValues[index++];
                data.perfectTemperatureStart = floatValues[index++];
                data.perfectTemperatureEnd = floatValues[index++];
                data.perfectDryTime = floatValues[index++];
                data.sugarLossPerCm = floatValues[index++];
                data.waterLossPerCm = floatValues[index++];
                data.temperatureLossPerOnce = floatValues[index++];
                data.temperatureLossPerTime = floatValues[index++];
                data.waterLossPerTime = floatValues[index++];
                data.lossTime = floatValues[index++];
                data.boilerCapacity = floatValues[index++];
                data.addLiquidPerOnce = floatValues[index++];
                data.fuelAddPerOnce = floatValues[index++];
                data.skewerLength = floatValues[index++];
                data.skewerMinLength = floatValues[index++];
                data.basicSatisfaction = floatValues[index++];
                data.satisfactionLossWhenIngredientMistake = floatValues[index++];
                data.satisfactionLossWhenConcentrationIsHigh = floatValues[index++];
                data.satisfactionLossWhenConcentrationIsLow = floatValues[index++];
                data.satisfactionLossWhenNeedMoreDry = floatValues[index++];
                data.satisfactionLossWhenWaitTooLong = floatValues[index++];
                data.initialPopularity = floatValues[index++];
                data.popularityLossWhenFail = floatValues[index++];
                data.popularityGainWhenSuccess = floatValues[index++];
                data.popularityLossWhenRefuse = floatValues[index++];
                data.popularityLossWhenPoke = floatValues[index++];
                data.money = (int)floatValues[index++];
                data.cherry = floatValues[index++];
                data.dragonFruit = floatValues[index++];
                data.greenGrape = floatValues[index++];
                data.mikan = floatValues[index++];
                data.pineapple = floatValues[index++];
                data.strawberry = floatValues[index++];
                data.sugarPrice = floatValues[index++];
                data.gasPrice = floatValues[index++];
                data.rentPrice = floatValues[index++];
                data.tax = floatValues[index++];
                data.electricityPrice = floatValues[index++];

                data.ingredients = new List<int>
                {
                    (int)data.cherry,
                    (int)data.dragonFruit,
                    (int)data.greenGrape,
                    (int)data.mikan,
                    (int)data.pineapple,
                    (int)data.strawberry
                };
            }
            else
            {
                // Log an error or handle the mismatch between floatValues and GameData fields
                Debug.LogError("Mismatch between DebugPanel content and GameData fields.");
            }
        }
    }
}