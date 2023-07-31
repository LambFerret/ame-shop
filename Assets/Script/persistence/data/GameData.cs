using System;
using System.Collections.Generic;
using Script.setting;

namespace Script.persistence.data
{
    [Serializable]
    public class GameData
    {
        public long lastUpdated;
        public string playerName;
        public int playerLevel;
        public int money;
        public int popularity;
        public List<string> equipment;

        public int dayMoneyEarned;
        public int dayElectricityUsage;
        public int dayGasUsage;
        public int daySugarUsage;

        public SerializableDictionary<IngredientManager.FirstIngredient, int> ingredients;


        public int language;
        public float volume;

        public bool isTutorialDone;

        public GameData()
        {
            playerName = "Player";
            playerLevel = 1;
            money = 0;
            popularity = 0;
            equipment = new List<string>();
            ingredients = new SerializableDictionary<IngredientManager.FirstIngredient, int>
            {
                { IngredientManager.FirstIngredient.Strawberry, 20 },
                { IngredientManager.FirstIngredient.BlendedCandy, 0 },
                { IngredientManager.FirstIngredient.Banana, 5 },
                { IngredientManager.FirstIngredient.GreenGrape, 20 },
                { IngredientManager.FirstIngredient.Apple, 3 },
                { IngredientManager.FirstIngredient.BigGrape, 10 },
                { IngredientManager.FirstIngredient.Coconut, 10 },
            };
        }
    }
}