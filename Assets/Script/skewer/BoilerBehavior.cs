using System;
using DG.Tweening;
using Script.persistence;
using Script.persistence.data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.skewer
{
    public class BoilerBehavior : MonoBehaviour, IDataPersistence
    {
        [Header("info")] public int concentration;
        public int sugarUsage;
        public int gasUsage;
        public int water;
        public int sugar;

        [Header("Options")] public int addFuelTemperature;
        public int addLiquidPerClick;
        public int temperature;
        public int maximumLiquidCapacity;

        [Header("Minus")] public int minusWaterByTime;
        public int minusSugarPerOnce;
        public int minusTemperatureByTime;
        public int minusTemperaturePerOnce;
        public float time;

        [Header("GameObjects")] public TextMeshProUGUI temperatureBar;
        public TextMeshProUGUI concentrationBar;
        private SkewerController _hand;
        private Image _image;
        private float _time;

        private void Awake()
        {
            _image = transform.Find("BoilerImage").GetComponent<Image>();
        }

        private void Start()
        {
            sugarUsage = 0;
            gasUsage = 0;
            _hand = GameObject.Find("SkewerController").GetComponent<SkewerController>();
            temperature = 110;
            sugar = 20;
            water = 30;
            concentration = sugar * 100 / (sugar + water);
        }

        private void Update()
        {
            if (water == 0 && sugar == 0)
            {
                concentrationBar.text = "0%";
            }
            else
            {
                float cons = 0;
                if (water != 0) cons = sugar / (float)(sugar + water);

                concentration = (int)Math.Floor(cons * 100);
                concentrationBar.text = concentration + "%";
            }

            temperatureBar.text = temperature + "°C";

            _time += Time.deltaTime;
            if (_time >= time)
            {
                MinusPerTime();
                _time = 0;
            }
        }

        public void LoadData(GameData data)
        {
        }

        public void SaveData(GameData data)
        {
            data.dayGasUsage = gasUsage;
            data.daySugarUsage = sugarUsage;
        }

        private void MinusPerTime()
        {
            water -= minusWaterByTime;
            temperature -= minusTemperatureByTime;
        }

        public void AddWater()
        {
            water += addLiquidPerClick;
            if (water + sugar > maximumLiquidCapacity) water = maximumLiquidCapacity - sugar;
        }

        public void AddSugar()
        {
            sugarUsage++;
            sugar += addLiquidPerClick;
            if (water + sugar > maximumLiquidCapacity) sugar = maximumLiquidCapacity - water;
        }

        public void AddFuel()
        {
            temperature += addFuelTemperature;
            gasUsage++;
        }

        public bool CheckHandIsSkewer()
        {
            return _hand.whatsOnHand == SkewerController.WhatsOnHand.Skewer;
        }

        public void Complete()
        {
            _image.DOKill();
            _image.DOFade(0, 0.5F).SetLoops(2 * 2, LoopType.Yoyo).SetEase(Ease.InOutSine);
            sugar -= minusSugarPerOnce * _hand.GetCurrentSize();
            temperature -= minusTemperaturePerOnce * _hand.GetCurrentSize();
            _hand.AddTemperature(concentration: concentration, temperature: temperature);
        }
    }
}