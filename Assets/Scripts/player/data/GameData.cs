using System;
using System.Collections.Generic;

namespace player.data
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
        public float dayPopularityLoss;
        public float dayPopularityGain;

        public List<int> ingredients;


        public int language;
        public float musicVolume;
        public float sfxVolume;

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
                10 //Coconut
            };
        }
    }
}