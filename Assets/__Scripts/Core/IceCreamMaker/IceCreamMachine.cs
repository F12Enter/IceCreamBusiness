using System.Collections;
using UnityEngine;
using Core.OrderSystem;
using Core.Statistics;

namespace Core.IceCreamMaker
{
    /// <summary>
    /// Ice cream machine technical system
    /// </summary>
    public class IceCreamMachine : MonoBehaviour
    {
        public static IceCreamMachine Instance { get; private set; }

        [Header("Ice Cream Machine Settings")]
        [SerializeField] private IceCreamBall _ballPrefab;
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private float _spawnDelay;
        
        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _pointSound;
        
        private Flavour _currentFlavour;
        
        private bool _isHolding;
        private bool _isAbleToUse;
        private int _wastedBalls = 0;

        public int WastedBalls => _wastedBalls;
        public bool IsHolding => _isHolding;
        public bool IsAbleToUse => _isAbleToUse;

        public void ResetWastedBalls() => _wastedBalls = 0;

        public void SetHolding(bool value) => _isHolding = value;
        public void SetAbleToUse(bool value) => _isAbleToUse = value;

        public void SetFlavour(Flavour newFlavour) => _currentFlavour = newFlavour;

        public IEnumerator SpawnBall()
        {
            while (_isHolding && _isAbleToUse && IceCreamStorage.GetCountByFlavour(_currentFlavour) > 0)
            {
                var iceCream = Instantiate(_ballPrefab, _spawnPosition.position, Quaternion.identity, ConeSpawner.Instance.Cone.transform);
                iceCream.GetComponent<IceCreamIdentifier>().Setup(_currentFlavour);

                IceCreamStorage.Remove(_currentFlavour, 1);
                _wastedBalls++;
                
                _audioSource.PlayOneShot(_pointSound);

                yield return new WaitForSeconds(_spawnDelay);
            }

        }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
        }
    }
}