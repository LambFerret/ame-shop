using System.Collections.Generic;
using DG.Tweening;
using ingredient;
using manager;
using player;
using player.data;
using setting;
using stage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace scene
{
    public class ResultScene : MonoBehaviour, IDataPersistence
    {
        [Header("Constant")] public int rentPrice;
        public int tax;
        public int electricityPrice;
        public int gasPrice;
        public int sugarPrice;

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

            foreach (var t in IngredientManager.Instance.ingredients)
            {
                var ingredient = Instantiate(Resources.Load<GameObject>("Prefabs/ShopItemButton"),
                    togglePlaceHolder.transform);
                Toggle toggle = ingredient.GetComponent<Toggle>();
                ShopItemButton itemButton = ingredient.GetComponent<ShopItemButton>();
                itemButton.SetIngredient(t);
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

            // debug
            sugarPrice = (int)data.sugarPrice;
            gasPrice = (int)data.gasPrice;
            rentPrice = (int)data.rentPrice;
            tax = (int)data.tax;
            electricityPrice = (int)data.electricityPrice;
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
            _ingredient = _gas * gasPrice +
                          _electricity * electricityPrice +
                          _sugar * sugarPrice;
            _rent = rentPrice;
            _tax = tax * (_day / 10);
            _netProfit = _totalMoneyEarned - _rent - _tax - _ingredient;
            _savings = _dataMoney + _netProfit;
        }
    }
}