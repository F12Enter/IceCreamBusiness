using System.Collections;
using UnityEngine;
using Core.OrderSystem;

namespace Core.IceCreamMaker
{
    /// <summary>
    /// Ice cream machine technical system
    /// </summary>
    public class IceCreamMachine : MonoBehaviour
    {
        [Header("Ice Cream Machine Settings")]
        [SerializeField] private IceCreamBall _ballPrefab;
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private float _spawnDelay;
        
        private Flavour _currentFlavour;
        
        private bool _isHolding;
        private bool _isAbleToUse;

        public bool IsHolding => _isHolding;
        public bool IsAbleToUse => _isAbleToUse;
        
        public void SetHolding(bool value) => _isHolding = value;
        public void SetAbleToUse(bool value) => _isAbleToUse = value;
        
        public IEnumerator SpawnBall()
        {
            while (_isHolding && _isAbleToUse)
            {
                var iceCream = Instantiate(_ballPrefab, _spawnPosition.position, Quaternion.identity, ConeSpawner.Instance.Cone.transform);
                iceCream.GetComponent<IceCreamIdentifier>().Flavour = _currentFlavour;
                yield return new WaitForSeconds(_spawnDelay);
            }

        }
    }
}