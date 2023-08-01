using System;
using System.Collections.Generic;
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
        public List<IngredientManager.FirstIngredient> firstIngredients;

        public bool isSlime;
        public IngredientManager.FirstIngredient slimeIngredient;
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
            List<IngredientManager.FirstIngredient> firstIngredients,
            bool isSlime = false,
            IngredientManager.FirstIngredient slimeIngredient = IngredientManager.FirstIngredient.BlendedCandy,
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
            this.firstIngredients = firstIngredients;
            this.isSlime = isSlime;
            if (isSlime && slimeIngredient is not IngredientManager.FirstIngredient.BlendedCandy && slimeIngredientCount > 0)
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