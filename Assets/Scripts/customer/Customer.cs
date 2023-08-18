using System;
using System.Collections.Generic;
using ingredient;
using UnityEngine;

namespace customer
{
    [Serializable]
    public abstract class Customer : ScriptableObject
    {
        public enum Emotion
        {
            Idle,
            Happy,
            Sad,
            Angry,
            Poked
        }

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
        public bool hasSpecialEvent;
        public int patience;
        public List<Emotion> emotions;
        public List<Sprite> sprites;
        public bool isSlime;

        [Header("Info")] public Ingredient ingredient;

        protected Customer(
            string id,
            bool hasSpecialEvent,
            int patience,
            bool isSlime = false
        )
        {
            this.id = id;
            this.hasSpecialEvent = hasSpecialEvent;
            this.patience = patience;
            this.isSlime = isSlime;
        }

        public Sprite GetSprite(Emotion emotion)
        {
            int index = emotions.IndexOf(emotion);
            return index >= 0 ? sprites[index] : sprites[0];
        }
    }
}