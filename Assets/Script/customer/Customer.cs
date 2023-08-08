using System;
using System.Collections.Generic;
using Script.ingredient;
using Script.setting;
using UnityEngine;

namespace Script.customer
{
    [Serializable]
    public abstract class Customer : ScriptableObject
    {
        public enum QuoteLine
        {
            Enter,
            Refused,
            TimeOut,
            Good,
            BadMildTaste,
            BadTooSweet,
            BadTooWatery,
            BadNotMyChoice,
            Poked
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
        public Ingredient ingredient;

        public bool isSlime;
        public string slimeIngredient;
        public int slimeIngredientCount;

        protected Customer(
            string id,
            bool hasSpecialEvent,
            int patience,
            int maxPopularity,
            int minPopularity,
            int scoreLimitation,
            int maxMoney,
            int minMoney,
            bool isSlime = false,
            string slimeIngredient = "BlendedCandy",
            int slimeIngredientCount = 0
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
            this.isSlime = isSlime;
            if (isSlime && slimeIngredient is not "BlendedCandy" && slimeIngredientCount > 0)
            {
                this.slimeIngredient = slimeIngredient;
                this.slimeIngredientCount = slimeIngredientCount;
            } else if (isSlime)
            {
                throw new Exception();
            }
        }
    }
}