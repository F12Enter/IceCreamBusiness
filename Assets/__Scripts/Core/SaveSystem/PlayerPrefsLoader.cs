using UnityEngine;
using Core.Statistics;
using Core.OrderSystem;

namespace Core.SaveSystem
{
    public class PlayerPrefsLoader : MonoBehaviour
    {
        private const string MoneyKey = "Money";
        
        private void SaveIceCreamBalls(Flavour flavour, int amount)
        {
            PlayerPrefs.SetInt(nameof(flavour), amount);
        }

        private void LoadIceCreamBalls(Flavour flavour)
        {
            int amount = PlayerPrefs.GetInt(nameof(flavour));
            IceCreamStorage.Set(flavour, amount);
        }

        private void SaveMoney()
        {
            int amount = Economy.EconomyManager.Money;
            PlayerPrefs.SetInt(MoneyKey, amount);
        }

        private void LoadMoney()
        {
            int amount = PlayerPrefs.GetInt(MoneyKey);
            Economy.EconomyManager.SetMoney(amount);

        }

    }
}

