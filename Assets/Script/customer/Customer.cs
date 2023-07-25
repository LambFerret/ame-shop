using System;
using System.Collections.Generic;
using Script.setting;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.customer
{
    [Serializable]
    public abstract class Customer : ScriptableObject
    {
        public enum QuoteLine
        {
            Enter,
            Good,
            Bad
        }

        public string id;
        public string customerName;
        public bool hasSpecialEvent;
        public int patience;
        public int maxPopularity;
        public int minPopularity;
        public int scoreLimitation;
        public int maxMoney;
        public int minMoney;
        public List<IngredientManager.FirstIngredient> firstIngredients;
        public Dictionary<QuoteLine, List<string>> QuoteLines = new();
        public List<IngredientManager.SecondIngredient> secondIngredients;
        public List<IngredientManager.ThirdIngredient> thirdIngredients;

        protected Customer(
            string id,
            bool hasSpecialEvent,
            int patience,
            int maxPopularity,
            int minPopularity,
            int scoreLimitation,
            int maxMoney,
            int minMoney,
            List<IngredientManager.FirstIngredient> firstIngredients,
            List<IngredientManager.SecondIngredient> secondIngredients,
            List<IngredientManager.ThirdIngredient> thirdIngredients
        )
        {
            this.id = id;
            this.hasSpecialEvent = hasSpecialEvent;
            this.patience = patience;
            this.maxPopularity = maxPopularity;
            this.minPopularity = minPopularity;
            this.scoreLimitation = scoreLimitation;
            this.maxMoney = maxMoney;
            this.minMoney = minMoney;
            this.firstIngredients = firstIngredients;
            this.secondIngredients = secondIngredients;
            this.thirdIngredients = thirdIngredients;
        }

        private async void OnEnable()
        {
            var locale = LocalizationSettings.SelectedLocale;
            var stringTableOp
                = LocalizationSettings.StringDatabase.GetTableAsync("Customer", locale);
            var stringTable = await stringTableOp.Task;
            var nameStr = stringTable[id + "Name"].GetLocalizedString();
            customerName = nameStr;
            stringTableOp.Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                    foreach (var line in Enum.GetValues(typeof(QuoteLine)))
                    {
                        var lineStr = stringTable[id + line].GetLocalizedString();
                        QuoteLines[(QuoteLine)line] = new List<string>(lineStr.Split('\n'));
                    }
            };
        }
    }
}