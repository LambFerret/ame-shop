using System.Collections.Generic;
using Script.machine;

namespace Script.stage
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
        }

        public void AddSecondIngredient(SecondIngredient secondIngredient)
        {
            _secondIngredients.Add(secondIngredient);
        }

        public void AddThirdIngredient(ThirdIngredient thirdIngredient)
        {
            _thirdIngredients.Add(thirdIngredient);
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
            return _firstIngredients;
        }

        public List<SecondIngredient> GetSecondIngredients()
        {
            return _secondIngredients;
        }

        public List<ThirdIngredient> GetThirdIngredients()
        {
            return _thirdIngredients;
        }
    }
}