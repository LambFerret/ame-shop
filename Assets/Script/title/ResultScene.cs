using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Script.persistence;
using Script.persistence.data;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.title
{
    public class ResultScene : MonoBehaviour, IDataPersistence
    {
        [Header("Constant")] public int rentCost;
        public int taxCost;
        public int electricityCostMultiplier;
        public int gasCostMultiplier;
        public int sugarCostMultiplier;

        private TextMeshProUGUI _totalMoneyEarnedText;
        private TextMeshProUGUI _rentText;
        private TextMeshProUGUI _taxText;
        private TextMeshProUGUI _ingredientText;
        private TextMeshProUGUI _gasText;
        private TextMeshProUGUI _electricityText;
        private TextMeshProUGUI _sugarText;
        private TextMeshProUGUI _netMoneyEarnedText;
        private TextMeshProUGUI _savingMoneyText;

        private int _day;

        private int _totalMoneyEarned;
        private int _rent;
        private int _tax;
        private int _ingredient;
        private int _gas;
        private int _electricity;
        private int _sugar;
        private int _netProfit;

        private Sequence _sequence;
        private int _dataMoney;
        private int _savings;

        public List<Toggle> itemToggles;

        private void Awake()
        {
            _totalMoneyEarnedText = GameObject.Find("TotalMoneyEarned").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _rentText = GameObject.Find("Rent").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _taxText = GameObject.Find("Tax").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _ingredientText = GameObject.Find("Ingredient").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _gasText = GameObject.Find("Gas").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _electricityText = GameObject.Find("Electricity").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _sugarText = GameObject.Find("Sugar").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _netMoneyEarnedText = GameObject.Find("NetMoneyEarned").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _savingMoneyText = GameObject.Find("SavingMoney").transform.Find("value").GetComponent<TextMeshProUGUI>();
            GameObject.Find("IngredientDetails").SetActive(false);
        }

        private void Start()
        {
            Calculate();

            foreach (var toggle in itemToggles)
            {
                toggle.onValueChanged.AddListener(arg0 =>
                {
                    if (arg0)
                    {
                        Buy(toggle.GetComponent<ShopItemButton>().cost);
                    }
                    else
                    {
                        Buy(-toggle.GetComponent<ShopItemButton>().cost);
                    }
                });
            }
        }

        public void Confirm()
        {
            if (_savings < 0)
            {
                // i can't buy
                return;
            }

            foreach (Toggle itemToggle in itemToggles)
            {
                if (itemToggle.isOn)
                {
                    var item = itemToggle.GetComponent<ShopItemButton>();
                    Buy(item.cost);
                    item.Confirm();
                }
            }

            _dataMoney = _savings;
            LoadingScreen.Instance.LoadScene("NewsScene");
        }

        private void Buy(int cost)
        {
            _savings -= cost;
        }

        private void Update()
        {
            _totalMoneyEarnedText.text = _totalMoneyEarned.ToString();
            _rentText.text = _rent.ToString();
            _taxText.text = _tax.ToString();
            _ingredientText.text = _ingredient.ToString();
            _gasText.text = _gas.ToString();
            _electricityText.text = _electricity.ToString();
            _sugarText.text = _sugar.ToString();
            _netMoneyEarnedText.text = _netProfit.ToString();
            _savingMoneyText.text = _savings.ToString();
        }

        private void Calculate()
        {
            _ingredient = _gas * gasCostMultiplier +
                          _electricity * electricityCostMultiplier +
                          _sugar * sugarCostMultiplier;
            _rent = rentCost;
            _tax = taxCost * (_day / 10);
            _netProfit = _totalMoneyEarned - _rent - _tax - _ingredient;
            _netProfit = 3000;
            _savings = _dataMoney + _netProfit;
        }

        public void LoadData(GameData data)
        {
            _day = data.playerLevel;
            _totalMoneyEarned = data.dayMoneyEarned;
            _gas = data.dayGasUsage;
            _electricity = data.dayElectricityUsage;
            _sugar = data.daySugarUsage;
            _dataMoney = data.money;
        }

        public void SaveData(GameData data)
        {
            data.money = _dataMoney;
        }
    }
}