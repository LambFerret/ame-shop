using System.Collections.Generic;
using System.Linq;
using Script.setting;
using UnityEngine;

namespace Script.skewer
{
    public class Skewer
    {
        private readonly List<IngredientManager.FirstIngredient> _firstIngredients;
        private readonly List<IngredientManager.SecondIngredient> _secondIngredients;
        private int _dryTime;
        private readonly List<IngredientManager.ThirdIngredient> _thirdIngredients;
        private bool _isThirdAddedFirst;

        public bool IsAlreadyBlended;
        public BlendedCandy BlendedCandy;

        public Skewer()
        {
            _firstIngredients = new List<IngredientManager.FirstIngredient>();
            _secondIngredients = new List<IngredientManager.SecondIngredient>();
            _thirdIngredients = new List<IngredientManager.ThirdIngredient>();
        }

        public void AddFirstIngredient(IngredientManager.FirstIngredient firstIngredient)
        {
            _firstIngredients.Add(firstIngredient);
            GetFirstIngredients();
        }

        public void AddSecondIngredient(IngredientManager.SecondIngredient secondIngredient)
        {
            _secondIngredients.Add(secondIngredient);
            GetSecondIngredients();
        }

        public void AddDryTime(int dryTime)
        {
            _dryTime = dryTime;
            Debug.Log("Dry time: " + _dryTime + "s");
        }

        public void AddThirdIngredient(IngredientManager.ThirdIngredient thirdIngredient)
        {
            _thirdIngredients.Add(thirdIngredient);
            GetThirdIngredients();
        }

        private BlendedCandy Blend()
        {
            if (IsAlreadyBlended)
            {
                BlendedCandy blendedCandy = BlendedCandy;
                blendedCandy.FirstIngredients.AddRange(GetFirstIngredients());
                blendedCandy.ThirdIngredients.AddRange(GetThirdIngredients());
                foreach (IngredientManager.SecondIngredient ingredient in GetSecondIngredients())
                {
                    switch (ingredient)
                    {
                        case IngredientManager.SecondIngredient.NormalSugar:
                            blendedCandy.SugarAmount += 10;
                            break;
                        case IngredientManager.SecondIngredient.ExtraSugar:
                            blendedCandy.SugarAmount += 20;
                            break;
                        case IngredientManager.SecondIngredient.None:
                            break;
                    }
                }
                return blendedCandy;
            }
            else
            {
                BlendedCandy blendedCandy = new BlendedCandy();
                blendedCandy.FirstIngredients = GetFirstIngredients();
                blendedCandy.ThirdIngredients = GetThirdIngredients();
                foreach (IngredientManager.SecondIngredient ingredient in GetSecondIngredients())
                {
                    switch (ingredient)
                    {
                        case IngredientManager.SecondIngredient.NormalSugar:
                            blendedCandy.SugarAmount += 10;
                            break;
                        case IngredientManager.SecondIngredient.ExtraSugar:
                            blendedCandy.SugarAmount += 20;
                            break;
                        case IngredientManager.SecondIngredient.None:
                            break;
                    }
                }
                return blendedCandy;
            }
        }

        public void AddBlendIngredient()
        {
            BlendedCandy = Blend();
            _firstIngredients.Clear();
            _secondIngredients.Clear();
            _thirdIngredients.Clear();
            _dryTime = 0;
            _isThirdAddedFirst = false;
            IsAlreadyBlended = true;
            _firstIngredients.Add(IngredientManager.FirstIngredient.BlendedCandy);
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

        public void SetThirdAddedFirst()
        {
            _isThirdAddedFirst = true;
        }

        public bool IsThirdAddedFirst()
        {
            return _isThirdAddedFirst;
        }

        public List<IngredientManager.FirstIngredient> GetFirstIngredients()
        {
            string listString = string.Join(" - ", _firstIngredients.Select(i => i.ToString()));
            Debug.Log($"[{listString}]");
            return _firstIngredients;
        }

        public string ReadFirstIngredients()
        {
            return string.Join(" - ", _firstIngredients.Select(i => i.ToString()));
        }

        public List<IngredientManager.SecondIngredient> GetSecondIngredients()
        {
            string listString = string.Join(" - ", _secondIngredients.Select(i => i.ToString()));
            Debug.Log($"[{listString}]");

            return _secondIngredients;
        }

        public int GetSecondDryTime()
        {
            return _dryTime;
        }

        public List<IngredientManager.ThirdIngredient> GetThirdIngredients()
        {
            string listString = string.Join(" - ", _thirdIngredients.Select(i => i.ToString()));
            Debug.Log($"[{listString}]");

            return _thirdIngredients;
        }
    }
}