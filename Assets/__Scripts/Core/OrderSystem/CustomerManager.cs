using System.Collections;
using Core.Worktime;
using UnityEngine;

namespace Core.OrderSystem
{
    public class CustomerManager : MonoBehaviour
    {
        [Header("Customers Settings")]
        
        [SerializeField] private float _minDelay;
        [SerializeField] private float _maxDelay;

        [Space]
        
        [SerializeField] private OrderData[] _orders;
        [SerializeField] private Customer _customer;
        
        private bool _isAlreadyOrdered;

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
            
        }
    }
}