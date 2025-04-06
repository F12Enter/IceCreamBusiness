using System;
using System.Collections;
using System.Collections.Generic;
using Core.Worktime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.OrderSystem
{
    public class CustomerManager : MonoBehaviour
    {
        public static CustomerManager Instance { get; private set; }
        
        [Header("Customers Settings")]
        
        [SerializeField] private float _minDelay;
        [SerializeField] private float _maxDelay;

        [Space]

        [Header("Payment Settings")]

        [SerializeField] private int _vanillaPay;
        [SerializeField] private int _chocolatePay;
        [SerializeField] private int _strawberryPay;

        [Space]
        
        [SerializeField] private Customer _customer;
        [SerializeField] private Transform _customerObject;
        
        private CustomerModelRandomizer _customerModelRandomizer;
        private Vector3 _customerStartPosition;
        private bool _isAlreadyOrdered;
        private int _moneyPerDay = 0;

        public int MoneyPerDay => _moneyPerDay;

        public void ResetMoneyPd() => _moneyPerDay = 0;

        public void EndOrder(OrderData orderData, bool successed)
        {
            if (successed)
            {
                int payment = GeneratePrice(orderData);
                Economy.EconomyManager.AddMoney(payment);
                _moneyPerDay += payment;
            }

            Debug.Log("EndOrder");
            _isAlreadyOrdered = false;
            StartCoroutine(PlayAnimation(false));
        }
        
        private IEnumerator SpawnCustomers()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(_minDelay, _maxDelay));

                if (!WorktimeManager.Instance.IsWorking || _isAlreadyOrdered)
                    continue;
                
                
                _isAlreadyOrdered = true;
                
                SpawnCustomer();
            }
        }

        private void SpawnCustomer()
        {
            StartCoroutine(PlayAnimation());
            
            _customerModelRandomizer.ChangeRandomModel();
            _customer.SetOrder(GenerateRandomOrder());

        }

        /// <summary>
        /// Generates random OrderData.
        /// For now, it generates random flavour count from Flavour enum length
        /// And random flavours for each generated flavour count,
        /// Also it generates random scoops count
        /// </summary>
        /// <returns></returns>
        private OrderData GenerateRandomOrder()
        {
            int flavourCount = Random.Range(2, Enum.GetNames(typeof(Flavour)).Length);
            List<Flavour> flavours = new();
            
            for (int i = 0; i < flavourCount; i++)
            {
                Array flavourValues = Enum.GetValues(typeof(Flavour));
                Flavour randomFlav = (Flavour)flavourValues.GetValue(Random.Range(0, flavourValues.Length));
                flavours.Add(randomFlav);
            }
            
            int scoops = flavourCount;
            int id = Random.Range(1, int.MaxValue); 
            
            var order = new OrderData(flavours, scoops, id.ToString());
            return order;
        }

        private int GeneratePrice(OrderData orderData)
        {
            int price = 0;

            foreach (Flavour flavour in orderData.RequiredFlavours)
            {
                switch (flavour)
                {
                    case Flavour.Vanilla:
                        price += _vanillaPay;
                        break;
                    case Flavour.Chocolate:
                        price += _chocolatePay;
                        break;
                    case Flavour.Strawberry:
                        price += _strawberryPay;
                        break;
                }
            }
            Debug.Log("Price " + price);
            return price;
        }

        private IEnumerator PlayAnimation(bool isComing = true)
        {
            float duration = 5f;
            float elapsedTime = 0f;
            Vector3 startPosition = _customerObject.position;
            Vector3 endPosition = isComing ? _customerStartPosition : new(_customerStartPosition.x, _customerStartPosition.y - 5, _customerStartPosition.z);
            
            while (elapsedTime < duration)
            {
                _customerObject.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _customerObject.position = endPosition;
        }
        
        private void Start()
        {
            _customerStartPosition = _customerObject.position;
            _customerObject.position += new Vector3(0, -5, 0); // Hide customer
            
            _customerModelRandomizer = GetComponent<CustomerModelRandomizer>();
            
            StartCoroutine(SpawnCustomers());
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
        }
    }
}