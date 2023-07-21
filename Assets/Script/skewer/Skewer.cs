using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script.skewer
{
    public class Skewer
    {
        private readonly List<FirstIngredient> _firstIngredients;
        private readonly List<SecondIngredient> _secondIngredients;
        private int _dryTime;
        private readonly List<ThirdIngredient> _thirdIngredients;
        private bool _isThirdAddedFirst;

        public bool IsAlreadyBlended;
        public BlendedCandy BlendedCandy;

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

        private BlendedCandy Blend()
        {
            if (IsAlreadyBlended)
            {
                BlendedCandy blendedCandy = BlendedCandy;
                blendedCandy.FirstIngredients.AddRange(GetFirstIngredients());
                blendedCandy.ThirdIngredients.AddRange(GetThirdIngredients());
                foreach (SecondIngredient ingredient in GetSecondIngredients())
                {
                    switch (ingredient)
                    {
                        case SecondIngredient.NormalSugar:
                            blendedCandy.SugarAmount += 10;
                            break;
                        case SecondIngredient.ExtraSugar:
                            blendedCandy.SugarAmount += 20;
                            break;
                        case SecondIngredient.None:
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
                foreach (SecondIngredient ingredient in GetSecondIngredients())
                {
                    switch (ingredient)
                    {
                        case SecondIngredient.NormalSugar:
                            blendedCandy.SugarAmount += 10;
                            break;
                        case SecondIngredient.ExtraSugar:
                            blendedCandy.SugarAmount += 20;
                            break;
                        case SecondIngredient.None:
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
            _firstIngredients.Add(FirstIngredient.BlendedCandy);
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