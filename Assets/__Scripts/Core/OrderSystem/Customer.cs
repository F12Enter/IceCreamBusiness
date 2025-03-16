using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.OrderSystem
{
    public class Customer : MonoBehaviour
    {
        [SerializeField] private float _checkRadius = 1f;
        
        
        private OrderData _order;
        private Transform _iceCreamTransform;

        public void SetOrder(OrderData order) => _order = order;
        
        public void SetIceCreamTransform(Transform t) => _iceCreamTransform = t;
        
        public void CheckOrder()
        {
            var scoops = GetScoops();

            if (scoops.Count < _order.MinimumScoops)
            {
                OnOrderFailed();
                return;
            }
            
            bool allMatch = scoops.All(s => _order.RequiredFlavours.Contains(s.Flavour));
            
            if (allMatch)
                OnOrderSucceeded();
            else
                OnOrderFailed();
        }

        private void OnOrderFailed()
        {
            Debug.Log("Order failed");
        }

        private void OnOrderSucceeded()
        {
            Debug.Log("Order succeeded");
        }
        
        private List<IceCreamIdentifier> GetScoops()
        {
            Collider[] colliders = Physics.OverlapSphere(_iceCreamTransform.position, _checkRadius);
            List<IceCreamIdentifier> scoops = new();

            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out IceCreamIdentifier scoop))
                    scoops.Add(scoop);
            }
            
            return scoops;
        }
        
        
    }
}