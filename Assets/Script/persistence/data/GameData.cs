using System;
using System.Collections.Generic;

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

        public int strawberry;
        public int banana;
        public int greenGrape;
        public int apple;
        public int bigGrape;
        public int coconut;

        public GameData()
        {
            playerName = "Player";
            playerLevel = 1;
            money = 0;
            popularity = 0;
            equipment = new List<string>();
            strawberry = 20;
            banana = 5;
            greenGrape = 20;
            apple = 3;
            bigGrape = 10;
            coconut = 10;
        }
    }
}