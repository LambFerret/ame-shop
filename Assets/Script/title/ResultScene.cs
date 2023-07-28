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

        public TextMeshProUGUI totalMoneyEarned;
        public TextMeshProUGUI rent;
        public TextMeshProUGUI tax;
        public TextMeshProUGUI ingredient;
        [Header("Ingredient")] public TextMeshProUGUI gas;
        public TextMeshProUGUI electricity;
        public TextMeshProUGUI sugar;
        [Header("Net Profit")] public TextMeshProUGUI netProfit;
        public TextMeshProUGUI savings;

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
                    Buy(itemToggle.GetComponent<ShopItemButton>().cost);
                }
            }
            _dataMoney = _savings;
            SceneManager.LoadSceneAsync("NewsScene");
        }

        public void Buy(int cost)
        {
            _savings -= cost;
        }

        private void Update()
        {
            totalMoneyEarned.text = _totalMoneyEarned.ToString();
            rent.text = _rent.ToString();
            tax.text = _tax.ToString();
            ingredient.text = _ingredient.ToString();
            gas.text = _gas.ToString();
            electricity.text = _electricity.ToString();
            sugar.text = _sugar.ToString();
            netProfit.text = _netProfit.ToString();
            savings.text = _savings.ToString();
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