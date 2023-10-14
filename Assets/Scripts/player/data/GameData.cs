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

        // debug
        public float gameStartTime;
        public float gameEndTime;
        public float gameHourLength;
        public float perfectConcentrationRange;
        public float perfectTemperatureStart;
        public float perfectTemperatureEnd;
        public float perfectDryTime;
        public float sugarLossPerCm;
        public float waterLossPerCm;
        public float temperatureLossPerOnce;
        public float temperatureLossPerTime;
        public float waterLossPerTime;
        public float lossTime;
        public float boilerCapacity;
        public float addLiquidPerOnce;
        public float fuelAddPerOnce;
        public float skewerLength;
        public float skewerMinLength;
        public float basicSatisfaction;
        public float satisfactionLossWhenIngredientMistake;
        public float satisfactionLossWhenConcentrationIsHigh;
        public float satisfactionLossWhenConcentrationIsLow;
        public float satisfactionLossWhenNeedMoreDry;
        public float satisfactionLossWhenWaitTooLong;
        public float initialPopularity;
        public float popularityLossWhenFail;
        public float popularityGainWhenSuccess;
        public float popularityLossWhenRefuse;
        public float popularityLossWhenPoke;
        public float cherry;
        public float dragonFruit;
        public float greenGrape;
        public float mikan;
        public float pineapple;
        public float strawberry;
        public float sugarPrice;
        public float gasPrice;
        public float rentPrice;
        public float tax;
        public float electricityPrice;

        public GameData()
        {
            playerName = "Player";
            playerLevel = 1;
            money = 2000;
            popularity = 60;
            equipment = new List<string>();
            // ingredients = new List<int>
            // {
            //     20, // cherry
            //     5, // dragonFruit
            //     20, // greenGrape
            //     3, // mikan
            //     10, // pineapple
            //     10 // strawberry
            // };

            // debug
            gameStartTime = 0F; // ok
            gameEndTime = 0F; // ok
            gameHourLength = 0F;
            
            perfectConcentrationRange = 0F;
            perfectTemperatureStart = 0F;
            perfectTemperatureEnd = 0F;
            perfectDryTime = 0F;
            sugarLossPerCm = 0F;
            waterLossPerCm = 0F;
            temperatureLossPerOnce = 0F;
            temperatureLossPerTime = 0F;
            waterLossPerTime = 0F;
            lossTime = 0F;
            boilerCapacity = 0F;
            addLiquidPerOnce = 0F;
            fuelAddPerOnce = 0F;
            skewerLength = 0F;
            skewerMinLength = 0F;
            basicSatisfaction = 0F;
            satisfactionLossWhenIngredientMistake = 0F;
            satisfactionLossWhenConcentrationIsHigh = 0F;
            satisfactionLossWhenConcentrationIsLow = 0F;
            satisfactionLossWhenNeedMoreDry = 0F;
            satisfactionLossWhenWaitTooLong = 0F;
            initialPopularity = 0F;
            popularityLossWhenFail = 0F;
            popularityGainWhenSuccess = 0F;
            popularityLossWhenRefuse = 0F;
            popularityLossWhenPoke = 0F;

            money = 0;
            cherry = 0F;
            dragonFruit = 0F;
            greenGrape = 0F;
            mikan = 0F;
            pineapple = 0F;
            strawberry = 0F;
            ingredients = new List<int>
            {
                (int)cherry,
                (int)dragonFruit,
                (int)greenGrape,
                (int)mikan,
                (int)pineapple,
                (int)strawberry
            };

            sugarPrice = 0F;
            gasPrice = 0F;
            rentPrice = 0F;
            tax = 0F;
            electricityPrice = 0F;

            popularity = initialPopularity;
        }

        public GameData Clone()
        {
            return (GameData)MemberwiseClone();
        }
    }
}