using System.Collections.Generic;
using Script.machine;
using UnityEngine;

namespace Script.skewer
{
    public class Skewer
    {
        private readonly List<FirstIngredient> _firstIngredients;
        private readonly List<SecondIngredient> _secondIngredients;
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
            _secondIngredients.Clear();
        }

        public void ClearThirdIngredient()
        {
            _thirdIngredients.Clear();
        }

        public List<FirstIngredient> GetFirstIngredients()
        {
            foreach (FirstIngredient ingredient in _firstIngredients)
            {
                Debug.Log(ingredient.ToString());
            }

            return _firstIngredients;
        }

        public List<SecondIngredient> GetSecondIngredients()
        {
            foreach (SecondIngredient ingredient in _secondIngredients)
            {
                Debug.Log(ingredient.ToString());
            }

            return _secondIngredients;
        }

        public List<ThirdIngredient> GetThirdIngredients()
        {
            foreach (ThirdIngredient ingredient in _thirdIngredients)
            {
                Debug.Log(ingredient.ToString());
            }

            return _thirdIngredients;
        }
    }
}