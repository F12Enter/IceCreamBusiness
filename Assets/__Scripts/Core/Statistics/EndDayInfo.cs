using UnityEngine;
using Core.IceCreamMaker;
using Core.OrderSystem;
using TMPro;

namespace Core.Statistics
{
    public class EndDayInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _wastedToday;
        [SerializeField] private TextMeshProUGUI _moneyBalance;
        [SerializeField] private TextMeshProUGUI _moneyGeneral;

        [SerializeField] private TextMeshProUGUI _vanillaLasts;
        [SerializeField] private TextMeshProUGUI _chocolateLasts;
        [SerializeField] private TextMeshProUGUI _strawberryLasts;

        private void OnEnable()
        {
            FillInfo();
        }

        public void FillInfo()
        {
            _wastedToday.text = IceCreamMachine.Instance.WastedBalls.ToString();
            _moneyBalance.text = CustomerManager.Instance.MoneyPerDay.ToString();
            _moneyGeneral.text = Economy.EconomyManager.Money.ToString();

            _vanillaLasts.text = IceCreamStorage.VanillaCount.ToString();
            _chocolateLasts.text = IceCreamStorage.ChocolateCount.ToString();
            _strawberryLasts.text = IceCreamStorage.StrawberryCount.ToString();
        }

    }
}

