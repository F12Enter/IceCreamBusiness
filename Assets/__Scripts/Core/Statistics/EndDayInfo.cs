using System;
using Core.Economy;
using UnityEngine;
using Core.IceCreamMaker;
using Core.OrderSystem;
using Core.SaveSystem;
using Core.Worktime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Core.Statistics
{
    public class EndDayInfo : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _wastedToday;
        [SerializeField] private TextMeshProUGUI _moneyBalance;
        [SerializeField] private TextMeshProUGUI _moneyGeneral;
        [SerializeField] private TextMeshProUGUI _day;
        
        [SerializeField] private TextMeshProUGUI _vanillaLasts;
        [SerializeField] private TextMeshProUGUI _chocolateLasts;
        [SerializeField] private TextMeshProUGUI _strawberryLasts;

        [Space]
        
        [SerializeField] private Button _toNextDayButton;

        public void FillInfo()
        {
            _wastedToday.text = IceCreamMachine.Instance.WastedBalls.ToString();
            _moneyBalance.text = CustomerManager.Instance.MoneyPerDay.ToString();
            _moneyGeneral.text = EconomyManager.Money.ToString();
            _day.text = "Day " + WorktimeManager.Instance.CurrentDay + " Completed!";

            _vanillaLasts.text = IceCreamStorage.VanillaCount.ToString();
            _chocolateLasts.text = IceCreamStorage.ChocolateCount.ToString();
            _strawberryLasts.text = IceCreamStorage.StrawberryCount.ToString();
        }

        /// <summary>
        /// This method saves player money and all ice cream flavours. 
        /// </summary>
        private void SaveGame()
        {
            PlayerPrefsLoader.SaveMoney();

            int days = WorktimeManager.Instance.CurrentDay;
            PlayerPrefsLoader.SaveDays(days);
            
            Array flavours = Enum.GetValues(typeof(Flavour));
            foreach (var flav in flavours)
            {
                var flavour = (Flavour)flav;
                var amount = IceCreamStorage.GetCountByFlavour(flavour);
                PlayerPrefsLoader.SaveIceCreamBalls(flavour, amount);
            }
        }

        private void StartNextDay()
        {
            SceneManager.LoadScene(1);
        }
        
        private void OnEnable()
        {
            FillInfo();
            SaveGame();
            
            _toNextDayButton.onClick.AddListener(StartNextDay);
        }

        private void OnDisable()
        {
            _toNextDayButton.onClick.RemoveAllListeners();
        }
        
    }
}

