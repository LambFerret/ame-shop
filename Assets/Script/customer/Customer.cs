using System;
using System.Collections.Generic;
using Script.machine;
using UnityEngine;

namespace Script.customer
{
    [Serializable]
    public abstract class Customer : ScriptableObject
    {
        public string id;
        public string customerName;
        public bool hasSpecialEvent;
        public List<string> lines;
        public int patience;
        public int satisfactionSpeed;
        public int maxMoney;
        public int minMoney;
        public List<FirstIngredient> firstIngredients;
        public List<SecondIngredient> secondIngredients;
        public List<ThirdIngredient> thirdIngredients;


        protected Customer(
            string id,
            bool hasSpecialEvent,
            int patience,
            int satisfactionSpeed,
            int maxMoney,
            int minMoney,
            List<FirstIngredient> firstIngredients,
            List<SecondIngredient> secondIngredients,
            List<ThirdIngredient> thirdIngredients
        )
        {
            this.id = id;
            // var info = Text.CustomerInfoDictionary[id];
            // customerName = info.name;
            // lines = info.lines;
            this.hasSpecialEvent = hasSpecialEvent;
            this.patience = patience;
            this.satisfactionSpeed = satisfactionSpeed;
            this.maxMoney = maxMoney;
            this.minMoney = minMoney;
            this.firstIngredients = firstIngredients;
            this.secondIngredients = secondIngredients;
            this.thirdIngredients = thirdIngredients;
        }
    }
}