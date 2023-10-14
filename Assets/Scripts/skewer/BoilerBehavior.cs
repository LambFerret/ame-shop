using System;
using DG.Tweening;
using player;
using player.data;
using setting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace skewer
{
    public class BoilerBehavior : MonoBehaviour, IDataPersistence
    {
        [Header("info")] public int concentration;
        public int sugarUsage;
        public int gasUsage;
        public int water;
        public int sugar;

        [Header("Options")] public int fuelAddPerOnce;
        public int addLiquidPerOnce;
        public int temperature;
        public int boilerCapacity;

        [Header("Minus")] public int waterLossPerTime;
        public int sugarLossPerCm;
        public int waterLossPerCm;
        public int temperatureLossPerTime;
        public int temperatureLossPerOnce;
        public float lossTime;

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
            UpdateConcentration();
        }

        private void Update()
        {
            concentrationBar.text = concentration + "%";
            temperatureBar.text = temperature + "°C";
            _time += Time.deltaTime;
            if (_time >= lossTime)
            {
                MinusPerTime();
                _time = 0;
            }
        }

        public void LoadData(GameData data)
        {
            fuelAddPerOnce = (int)data.fuelAddPerOnce;
            addLiquidPerOnce = (int)data.addLiquidPerOnce;
            boilerCapacity = (int)data.boilerCapacity;
            waterLossPerTime = (int)data.waterLossPerTime;
            sugarLossPerCm = (int)data.sugarLossPerCm;
            waterLossPerCm = (int)data.waterLossPerCm;
            temperatureLossPerTime = (int)data.temperatureLossPerTime;
            temperatureLossPerOnce = (int)data.temperatureLossPerOnce;
            lossTime = data.lossTime;
        }

        public void SaveData(GameData data)
        {
            data.dayGasUsage = gasUsage;
            data.daySugarUsage = sugarUsage;
        }

        private void MinusPerTime()
        {
            PutWater(-waterLossPerTime);
            PutTemperature(-temperatureLossPerTime);
        }

        public void AddWater()
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFX.Water);
            PutWater(addLiquidPerOnce);
        }

        public void AddSugar()
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFX.Sugar);
            PutSugar(addLiquidPerOnce);
        }

        public void AddFuel()
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFX.Fuel);
            PutTemperature(fuelAddPerOnce);
        }

        private void PutWater(int value)
        {
            water += value;

            if (water < 0)
                water = 0;
            else if (water > boilerCapacity)
                water = boilerCapacity;

            if (water + sugar > boilerCapacity)
                sugar = boilerCapacity - water;

            UpdateConcentration();
        }

        private void PutSugar(int value)
        {
            sugarUsage += value;
            sugar += value;

            if (sugar < 0)
                sugar = 0;
            else if (sugar > boilerCapacity)
                sugar = boilerCapacity;

            if (sugar + water > boilerCapacity)
                water = boilerCapacity - sugar;

            UpdateConcentration();
        }


        private void UpdateConcentration()
        {
            if (water == 0 && sugar == 0) concentration = 0;
            else
            {
                float cons = sugar / (float)(sugar + water);
                concentration = (int)Math.Floor(cons * 100);
            }
        }


        private void PutTemperature(int value)
        {
            temperature += value;
            gasUsage += value;
            if (temperature > 300) temperature = 300;
            if (temperature < 20) temperature = 20;
        }

        public bool CheckHandIsSkewer()
        {
            return _hand.whatsOnHand == SkewerController.WhatsOnHand.Skewer;
        }

        public void Complete()
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFX.Pool);
            _image.DOKill();
            _image.DOFade(0, 0.5F).SetLoops(2 * 2, LoopType.Yoyo).SetEase(Ease.InOutSine);
            PutSugar(-(sugarLossPerCm * _hand.GetCurrentSize()));
            PutWater(-(waterLossPerCm * _hand.GetCurrentSize()));
            PutTemperature(-temperatureLossPerOnce);
            _hand.AddTemperature(concentration: concentration, temperature: temperature);
        }
    }
}