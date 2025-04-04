using UnityEngine;
using Core.IceCreamMaker;
using Core.OrderSystem;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using Player.Controller;

namespace Core.Statistics
{
    public class EndDayInfo : MonoBehaviour
    {
        [Header("Global Information")]
        [SerializeField] private TextMeshProUGUI _totalIcecreamBallsInfo;
        [SerializeField] private TextMeshProUGUI _currentBalanceInfo;

        [Header("Daily Information")]
        [SerializeField] private TextMeshProUGUI _spentScoopsPerDayInfo;
        [SerializeField] private TextMeshProUGUI _dailyProfitInfo;


        [Header("Shop Panel")]
        [SerializeField] private TMP_InputField _vanillaBallsInputField;
        [SerializeField] private TMP_InputField _chocolateBallsInputField;
        [SerializeField] private TMP_InputField _strawberryBallsInputField;
        [SerializeField] private TextMeshProUGUI _totalCostInfo;

        List<TMP_InputField> _shopsInputFieldsList;

        private void Awake()
        {
            _shopsInputFieldsList = new List<TMP_InputField>
            {
                _vanillaBallsInputField,
                _chocolateBallsInputField,
                _strawberryBallsInputField,
            };
        }
       

        private void OnEnable()
        {
            //CursorManager.UnlockCursor();я так розумію він вже десь лочиться та анлочітся

            foreach (var inputField in _shopsInputFieldsList)
            {
                inputField.onValueChanged.AddListener(OnSomeShopsInputFieldChang);
            }

            FillInfo();
        }
        private void OnDisable()
        {
            //CursorManager.LockCursor();

            foreach (var inputField in _shopsInputFieldsList)
            {
                inputField.onValueChanged.RemoveListener(OnSomeShopsInputFieldChang);
            }
        }
        private void Update()
        {
            CursorManager.UnlockCursor();//для тесту
        }

        public void FillInfo()
        {
            int spentScoopsPerDayCount =
               IceCreamStorage.VanillaCount +
               IceCreamStorage.ChocolateCount +
               IceCreamStorage.StrawberryCount;

            //Global Information
            _totalIcecreamBallsInfo.text = IceCreamMachine.Instance.WastedBalls.ToString();
            _currentBalanceInfo.text = Economy.EconomyManager.Money.ToString();

            //Daily Information
            _spentScoopsPerDayInfo.text = spentScoopsPerDayCount.ToString();
            _dailyProfitInfo.text = Economy.EconomyManager.Money.ToString();//тут повинен бути прибуток за день

            //Shop
            SetTotalCost();
            //_vanillaBallsInputField.text = IceCreamStorage.VanillaCount.ToString();
            //_chocolateBallsInputField.text = IceCreamStorage.ChocolateCount.ToString();
            //_strawberryBallsInputField.text = IceCreamStorage.StrawberryCount.ToString();
        }



        void OnSomeShopsInputFieldChang(string val)
        {
            SetTotalCost();
        }
        void SetTotalCost()
        {
            int totalCost = _shopsInputFieldsList
                .Select(field => int.TryParse(field.text, out int val) ? val : 0)
                .Sum();

            _totalCostInfo.text = totalCost.ToString();
        }

        public void OnBuyButtonClick()
        {
            Debug.Log("Buy!");
        }

    }
}

