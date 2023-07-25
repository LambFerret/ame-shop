using System;
using System.Collections.Generic;
using Script.skewer;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.customer
{
    [Serializable]
    public abstract class Customer : ScriptableObject
    {
        public string id;
        public string customerName;
        public bool hasSpecialEvent;
        public Dictionary<QuoteLine, List<string>> QuoteLines = new Dictionary<QuoteLine, List<string>>();
        public int patience;
        public int maxPopularity;
        public int minPopularity;
        public int scoreLimitation;
        public int maxMoney;
        public int minMoney;
        public List<FirstIngredient> firstIngredients;
        public List<SecondIngredient> secondIngredients;
        public List<ThirdIngredient> thirdIngredients;

        protected Customer(
            string id,
            bool hasSpecialEvent,
            int patience,
            int maxPopularity,
            int minPopularity,
            int scoreLimitation,
            int maxMoney,
            int minMoney,
            List<FirstIngredient> firstIngredients,
            List<SecondIngredient> secondIngredients,
            List<ThirdIngredient> thirdIngredients
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
            // var stringTableOp
            //     = LocalizationSettings.StringDatabase.GetTableAsync("Customer", locale);
            // var stringTable = await stringTableOp.Task;
            Debug.Log(id + "Name");

            var nameStr = stringTable[id + "Name"].GetLocalizedString();
            customerName = nameStr;


            var stringTableOp = LocalizationSettings.StringDatabase.GetTableAsync("Customer", locale);

            stringTableOp.Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    var stringTable = op.Result;

                    foreach (var entry in stringTable.)
                    {
                        // Here, 'entry' is a KeyValuePair where 'entry.Key' is the key of the entry and 'entry.Value' is the LocalizedStringReference

                        // Add your logic to populate QuoteLines here
                        // For example:
                        QuoteLine quoteLine = ParseQuoteLine(entry.Key);
                        string localizedString = entry.Value.GetLocalizedString(locale).Result;

                        if (QuoteLines.ContainsKey(quoteLine))
                        {
                            QuoteLines[quoteLine].Add(localizedString);
                        }
                        else
                        {
                            QuoteLines[quoteLine] = new List<string> { localizedString };
                        }
                    }
                }
            };

        }

        public enum QuoteLine
        {
            Enter, Good, Bad
        }
    }
}