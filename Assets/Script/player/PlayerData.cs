using System.Collections.Generic;
using UnityEngine;

namespace Script.player
{
    public class PlayerData :ScriptableObject
    {
        public string playerName;
        public int playerLevel;
        public int money;
        public int popularity;
        public List<string> equipment;
    }
}