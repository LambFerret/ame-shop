using System;
using System.Collections.Generic;

namespace Script.persistence.data
{
    [Serializable]
    public class GameData
    {public long lastUpdated;
        public string playerName;
        public int playerLevel;
        public int money;
        public int popularity;
        public List<string> equipment;

        public GameData()
        {
            playerName = "Player";
            playerLevel = 1;
            money = 0;
            popularity = 0;
            equipment = new List<string>();
        }
    }
}