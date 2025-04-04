using UnityEngine;
using Core.Input;
using Player.Interaction;

namespace Core.IceCreamMaker
{
    /// <summary>
    /// Player controller for IceCreamMachine
    /// </summary>
    public class IceCreamPlayerController : MonoBehaviour, IOnceInteractable
    {
        public static IceCreamPlayerController Instance { get; private set; }
        
        [Header("Ice Cream Player Controller Settings")]
        [SerializeField] private IceCreamMachine _machine;
        
        private Controls _controls;
        private Coroutine _spawnCoroutine;

        private GameObject _cone;
        
        public void SetCone(GameObject cone) => _cone = cone;
        
        public void Interact()
        {
            if (!_cone) return; // If there are no cone we can't use machine
            
            var plyFocus = PlayerFocusing.Instance;

            // Check if player already focused
            // If yes, we unfocus from machine
            // If no, we focus on machine
            if (plyFocus.IsFocused)
            {
                plyFocus.Unfocus();
                _machine.SetAbleToUse(false);
                StopSpawnCoroutine();
            }
            else
            {
                plyFocus.Focus();
                _machine.SetAbleToUse(true);
            }
        }

        private void StopSpawnCoroutine()
        {
            if (_spawnCoroutine == null) return;
            
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
            
            _controls = new Controls();
        }

        private void OnHoldStarted()
        {
            if (_machine.IsAbleToUse && _spawnCoroutine == null)
            {
                _machine.SetHolding(true);
                _spawnCoroutine = StartCoroutine(_machine.SpawnBall());
            }
        }

        private void OnHoldStopped()
        {
            _machine.SetHolding(false);
            StopSpawnCoroutine();
        }
        
        private void OnEnable()
        {
            _controls.Gameplay.Enable();
            _controls.Gameplay.IceCream.started += _ => OnHoldStarted();
            _controls.Gameplay.IceCream.canceled += _ => OnHoldStopped();
        }
        
        private void OnDisable() => _controls.Gameplay.Disable();
        
    }
}


