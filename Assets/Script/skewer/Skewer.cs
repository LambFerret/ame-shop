using System.Collections.Generic;
using System.Linq;
using Script.machine;
using UnityEngine;

namespace Script.skewer
{
    public class Skewer
    {
        private readonly List<FirstIngredient> _firstIngredients;
        private readonly List<SecondIngredient> _secondIngredients;
        private int _dryTime;
        private readonly List<ThirdIngredient> _thirdIngredients;

        public Skewer()
        {
            _firstIngredients = new List<FirstIngredient>();
            _secondIngredients = new List<SecondIngredient>();
            _thirdIngredients = new List<ThirdIngredient>();
        }

        public void AddFirstIngredient(FirstIngredient firstIngredient)
        {
            _firstIngredients.Add(firstIngredient);
            GetFirstIngredients();
        }

        public void AddSecondIngredient(SecondIngredient secondIngredient)
        {
            _secondIngredients.Add(secondIngredient);
            GetSecondIngredients();
        }

        public void AddDryTime(int dryTime)
        {
            _dryTime = dryTime;
            Debug.Log("Dry time: " + _dryTime + "s");
        }

        public void AddThirdIngredient(ThirdIngredient thirdIngredient)
        {
            _thirdIngredients.Add(thirdIngredient);
            GetThirdIngredients();
        }

        public void ClearFirstIngredient()
        {
            _firstIngredients.Clear();
        }

        public void ClearSecondIngredient()
        {
            _dryTime = 0;
            _secondIngredients.Clear();
        }

        public void ClearThirdIngredient()
        {
            _thirdIngredients.Clear();
        }

        public List<FirstIngredient> GetFirstIngredients()
        {
            string listString = string.Join(" - ", _firstIngredients.Select(i => i.ToString()));
            Debug.Log($"[{listString}]");
            return _firstIngredients;
        }

        public string ReadFirstIngredients()
        {
            return string.Join(" - ", _firstIngredients.Select(i => i.ToString()));
        }

        public List<SecondIngredient> GetSecondIngredients()
        {
            string listString = string.Join(" - ", _secondIngredients.Select(i => i.ToString()));
            Debug.Log($"[{listString}]");

            return _secondIngredients;
        }

        public int GetSecondDryTime()
        {
            return _dryTime;
        }

        public List<ThirdIngredient> GetThirdIngredients()
        {
            string listString = string.Join(" - ", _thirdIngredients.Select(i => i.ToString()));
            Debug.Log($"[{listString}]");

            return _thirdIngredients;
        }
    }
}