using System;
using UnityEngine;
using Core.Statistics;
using Core.OrderSystem;

namespace Core.SaveSystem
{
    public class PlayerPrefsLoader : MonoBehaviour
    {
        private const string MoneyKey = "Money";
        private const string DaysKey = "Days";
        private const string SaveableKey = "Saveable";

        public static void SaveDays(int days)
        {
            PlayerPrefs.SetInt(DaysKey, days);
            PlayerPrefs.Save();
        }
        
        public static int GetDays() => PlayerPrefs.GetInt(DaysKey, 1);
        
        public static void SaveIceCreamBalls(Flavour flavour, int amount)
        {
            PlayerPrefs.SetInt(nameof(flavour), amount);
            PlayerPrefs.Save();
        }

        public static void LoadIceCreamBalls()
        {
            Array flavours = Enum.GetValues(typeof(Flavour));
            foreach (var flav in flavours)
            {
                var flavour = (Flavour)flav;
                int amount = PlayerPrefs.GetInt(nameof(flavour));
                IceCreamStorage.Set(flavour, amount);
            }
        }

        public static void SaveMoney()
        {
            int amount = Economy.EconomyManager.Money;
            PlayerPrefs.SetInt(MoneyKey, amount);
            PlayerPrefs.Save();
        }

        public static void LoadMoney()
        {
            int amount = PlayerPrefs.GetInt(MoneyKey);
            Economy.EconomyManager.SetMoney(amount);

        }

        public static void SetGameSaveableState(bool state)
        {
            PlayerPrefs.SetInt(SaveableKey, state ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static bool GetGameSaveableState()
        {
            return PlayerPrefs.GetInt(SaveableKey, 0) == 1;
        }
        
    }
}

