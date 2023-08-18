using System.Collections.Generic;
using DG.Tweening;
using manager;
using player;
using player.data;
using setting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace title
{
    public class ResultScene : MonoBehaviour, IDataPersistence
    {
        [Header("Constant")] public int rentCost;
        public int taxCost;
        public int electricityCostMultiplier;
        public int gasCostMultiplier;
        public int sugarCostMultiplier;

        public GameObject togglePlaceHolder;
        public AdsSetting ads;
        public Toggle adToggle1;
        public Toggle adToggle2;
        private int _dataMoney;

        private int _day;
        private float _dayPopularityGain;
        private float _dayPopularityLoss;
        private int _electricity;

        private TextMeshProUGUI _electricityText;
        private int _gas;
        private TextMeshProUGUI _gasText;
        private int _ingredient;
        private TextMeshProUGUI _ingredientText;

        private List<Toggle> _itemToggles;
        private TextMeshProUGUI _netMoneyEarnedText;
        private int _netProfit;
        private int _rent;
        private TextMeshProUGUI _rentText;
        private TextMeshProUGUI _savingMoneyText;
        private int _savings;
        private Sequence _sequence;
        private int _sugar;
        private TextMeshProUGUI _sugarText;
        private int _tax;
        private TextMeshProUGUI _taxText;
        private int _totalMoneyEarned;
        private TextMeshProUGUI _totalMoneyEarnedText;

        private void Awake()
        {
            _totalMoneyEarnedText = GameObject.Find("TotalMoneyEarned").transform.Find("value")
                .GetComponent<TextMeshProUGUI>();
            _rentText = GameObject.Find("Rent").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _taxText = GameObject.Find("Tax").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _ingredientText = GameObject.Find("Ingredient").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _gasText = GameObject.Find("Gas").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _electricityText = GameObject.Find("Electricity").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _sugarText = GameObject.Find("Sugar").transform.Find("value").GetComponent<TextMeshProUGUI>();
            _netMoneyEarnedText = GameObject.Find("NetMoneyEarned").transform.Find("value")
                .GetComponent<TextMeshProUGUI>();
            _savingMoneyText = GameObject.Find("SavingMoney").transform.Find("value").GetComponent<TextMeshProUGUI>();
            GameObject.Find("IngredientDetails").SetActive(false);
        }

        private void Start()
        {
            Calculate();
            ads = GameObject.Find("Loading").GetComponent<AdsSetting>();
            Debug.Log("-=-=-=-=-=-=-=-=ID-=-=-=-=-=-=");
            Debug.Log(ads.interstitialAdId);

            for (int i = 0; i < IngredientManager.Instance.ingredients.Count; i++)
            {
                var ingredient = Instantiate(Resources.Load<GameObject>("Prefabs/ShopItemButton"),
                    togglePlaceHolder.transform);
                Toggle toggle = ingredient.GetComponent<Toggle>();
                ShopItemButton itemButton = ingredient.GetComponent<ShopItemButton>();
                itemButton.SetIngredient(IngredientManager.Instance.ingredients[i]);
                toggle.onValueChanged.AddListener(arg0 =>
                {
                    if (arg0)
                        Buy(itemButton.cost);
                    else
                        Buy(-itemButton.cost);
                });
                _itemToggles.Add(toggle);
            }
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

        public void LoadData(GameData data)
        {
            _day = data.playerLevel;
            _totalMoneyEarned = data.dayMoneyEarned;
            _gas = data.dayGasUsage;
            _electricity = data.dayElectricityUsage;
            _sugar = data.daySugarUsage;
            _dataMoney = data.money;
            _dayPopularityGain = data.dayPopularityGain;
            _dayPopularityLoss = data.dayPopularityLoss;
        }

        public void SaveData(GameData data)
        {
            data.money = _dataMoney;
        }

        public void Confirm()
        {
            if (_savings < 0)
                // i can't buy
                return;

            foreach (Toggle itemToggle in _itemToggles)
                if (itemToggle.isOn)
                {
                    var item = itemToggle.GetComponent<ShopItemButton>();
                    Buy(item.cost);
                    item.Confirm();
                }

            _dataMoney = _savings;

            ads.ShowInterstitial();

            LoadingScreen.Instance.LoadScene("NewsScene");
        }


        public void LessPopularityByAds()
        {
            ads.ShowPopularity(-(_dayPopularityLoss * 0.2F));
        }

        public void MoreMoneyByAds()
        {
            ads.ShowMoney((int)(_netProfit * 0.2F));
        }


        private void Buy(int cost)
        {
            _savings -= cost;
        }

        private void Calculate()
        {
            _ingredient = _gas * gasCostMultiplier +
                          _electricity * electricityCostMultiplier +
                          _sugar * sugarCostMultiplier;
            _rent = rentCost;
            _tax = taxCost * (_day / 10);
            _netProfit = _totalMoneyEarned - _rent - _tax - _ingredient;
            _savings = _dataMoney + _netProfit;
        }
    }
}