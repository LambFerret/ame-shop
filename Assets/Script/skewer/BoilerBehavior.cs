using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.skewer
{
    public class BoilerBehavior : MonoBehaviour
    {
        private SkewerController _hand;
        public int fuelTemperature;
        public int addPerClick;
        public int temperature;
        public int concentration;

        public int water;
        public int sugar;

        [Header("GameObjects")] public TextMeshProUGUI temperatureBar;
        public TextMeshProUGUI concentrationBar;

        private void Start()
        {
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
                if (water != 0)
                {
                    cons = sugar / (float)(sugar + water);
                }

                concentration = (int)Math.Floor(cons * 100);
                concentrationBar.text = concentration + "%";
            }

            temperatureBar.text = temperature + "°C";
        }

        public void AddWater()
        {
            water += addPerClick;
        }

        public void AddSugar()
        {
            sugar += addPerClick;
        }

        public void AddFuel()
        {
            temperature += fuelTemperature;
        }

        public void Complete()
        {
            _hand.AddTemperature(concentration: concentration, temperature: temperature);
        }
    }
}