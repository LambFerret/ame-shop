using System;
using System.Collections.Generic;
using Script.ingredient;
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
        public float popularity;
        public List<string> equipment;

        public int dayMoneyEarned;
        public int dayElectricityUsage;
        public int dayGasUsage;
        public int daySugarUsage;

        public List<int> ingredients;


        public int language;
        public float volume;

        public bool isTutorialDone;

        public GameData()
        {
            playerName = "Player";
            playerLevel = 1;
            money = 2000;
            popularity = 60;
            equipment = new List<string>();
            ingredients = new List<int>
            {
                0, //BlendedCandy
                20, //Strawberry
                5, //Banana
                20, //GreenGrape
                3, //Apple
                10, //BigGrape
                10, //Coconut
            };
        }
    }
}