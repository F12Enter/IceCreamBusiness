using System;
using System.Collections.Generic;
using System.Linq;
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
        [Header("Global Information")]
        [SerializeField] private TextMeshProUGUI _spentScoopsPerDayInfo;
        [SerializeField] private TextMeshProUGUI _currentBalanceInfo;
        [SerializeField] private TextMeshProUGUI _day;
        
        [Header("Daily Information")]
        [SerializeField] private TextMeshProUGUI _totalIcecreamBallsInfo;
        [SerializeField] private TextMeshProUGUI _dailyProfitInfo;


        [Header("Shop Panel")]
        [SerializeField] private TMP_InputField _vanillaBallsInputField;
        [SerializeField] private TMP_InputField _chocolateBallsInputField;
        [SerializeField] private TMP_InputField _strawberryBallsInputField;
        [SerializeField] private TextMeshProUGUI _totalCostInfo;
        
        [SerializeField] private TextMeshProUGUI _vanillaAmount;
        [SerializeField] private TextMeshProUGUI _chocolateAmount;
        [SerializeField] private TextMeshProUGUI _strawberryAmount;

        [Space]
        
        [SerializeField] private Button _toNextDayButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private GameObject _looseScreen;
        [SerializeField] private Button _exitAndResetGameButton;
        
        private List<TMP_InputField> _shopsInputFieldsList;

        public void FillInfo()
        {
            _day.text = "Day " + (WorktimeManager.Instance.CurrentDay - 1) + " Completed!";

            int spentScoopsPerDayCount =
                IceCreamStorage.VanillaCount +
                IceCreamStorage.ChocolateCount +
                IceCreamStorage.StrawberryCount;

            //Global Information
            _totalIcecreamBallsInfo.text = IceCreamMachine.Instance.WastedBalls.ToString();
            _currentBalanceInfo.text = Economy.EconomyManager.Money.ToString();

            //Daily Information
            _spentScoopsPerDayInfo.text = spentScoopsPerDayCount.ToString();
            _dailyProfitInfo.text = CustomerManager.Instance.MoneyPerDay.ToString();

            //Shop
            _vanillaAmount.text = IceCreamStorage.VanillaCount.ToString();
            _chocolateAmount.text = IceCreamStorage.ChocolateCount.ToString();
            _strawberryAmount.text = IceCreamStorage.StrawberryCount.ToString();
            
        }
        
        private void OnSomeShopsInputFieldChang(string val)
        {
            int cost = GetTotalCost();
            _totalCostInfo.text = cost.ToString();
            
            if (cost > EconomyManager.Money)
                _buyButton.interactable = false;
            else
                _buyButton.interactable = true;
            
                
        }
        
        private int GetTotalCost()
        {
            int totalCost = _shopsInputFieldsList
                .Select(field => int.TryParse(field.text, out int val) ? val : 0)
                .Sum();

            return totalCost;
        }

        public void OnBuyButtonClick()
        {
            if (GetTotalCost() > EconomyManager.Money)
            {
                _buyButton.interactable = false;
                return;
            }
            else
                _buyButton.interactable = true;

            EconomyManager.RemoveMoney(GetTotalCost());
            
            IceCreamStorage.Add(Flavour.Vanilla, int.TryParse(_vanillaBallsInputField.text, out int vanAmount) ? vanAmount : 0);
            IceCreamStorage.Add(Flavour.Chocolate, int.TryParse(_chocolateBallsInputField.text, out int chocolateAmount) ? chocolateAmount : 0);
            IceCreamStorage.Add(Flavour.Strawberry, int.TryParse(_strawberryBallsInputField.text, out int strawberryAmount) ? strawberryAmount : 0);
            
            FillInfo();
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
            PlayerPrefsLoader.SetGameSaveableState(true);
            
            SaveGame();
            SceneManager.LoadScene(1);
        }
        
        private void OnEnable()
        {
            FillInfo();
            
            foreach (var inputField in _shopsInputFieldsList)
            {
                inputField.onValueChanged.AddListener(OnSomeShopsInputFieldChang);
            }
            
            _toNextDayButton.onClick.AddListener(StartNextDay);
            _buyButton.onClick.AddListener(OnBuyButtonClick);
            _exitAndResetGameButton.onClick.AddListener(ExitAndResetGame);
            
            if (EconomyManager.Money == 0 && IceCreamStorage.VanillaCount == 0 && IceCreamStorage.ChocolateCount == 0 &&
                IceCreamStorage.StrawberryCount == 0)
            {
                Loose();
            }
        }

        private void Loose()
        {
            _looseScreen.gameObject.SetActive(true);
        }

        private void ExitAndResetGame()
        {
            PlayerPrefsLoader.SetGameSaveableState(false);
            SceneManager.LoadScene(0);
        }

        private void OnDisable()
        {
            foreach (var inputField in _shopsInputFieldsList)
            {
                inputField.onValueChanged.RemoveListener(OnSomeShopsInputFieldChang);
            }
            
            _toNextDayButton.onClick.RemoveAllListeners();
            _buyButton.onClick.RemoveAllListeners();
            _exitAndResetGameButton.onClick.RemoveAllListeners();
        }
        
        private void Awake()
        {
            _shopsInputFieldsList = new List<TMP_InputField>
            {
                _vanillaBallsInputField,
                _chocolateBallsInputField,
                _strawberryBallsInputField,
            };
        }
        
    }
}

