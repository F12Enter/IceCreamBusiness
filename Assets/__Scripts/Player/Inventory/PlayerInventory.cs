using System;
using Core.Input;
using Player.Interaction;
using UnityEngine;

namespace Player.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        public static PlayerInventory Instance { get; private set; }

        [SerializeField] private Transform _handPos;
        
        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _dropSfx;
        
        public bool IsHolding => _objectInHand != null;
        public GameObject ObjectInHand => _objectInHand;
        
        private GameObject _objectInHand;
        private Controls _controls;

        public void Pickup(GameObject newObject)
        {
            Debug.Log("Pickup");
            if (_objectInHand != null)
                ReleaseItem();
            
            _objectInHand = newObject;
            _objectInHand.transform.SetParent(_handPos);
            _objectInHand.transform.localPosition = Vector3.zero;
            
            if (_objectInHand.TryGetComponent<Rigidbody>(out Rigidbody rb))
                rb.isKinematic = true;
        }

        public void ReleaseItem(bool destroy = true, bool playsound = true)
        {
            if (!IsHolding) return;     // Prevents releasing if we aren't holding anything
            
            Debug.Log("Released");
            
            if (destroy)
                Destroy(_objectInHand);
            else
                _objectInHand.transform.SetParent(null);
            
            if (playsound) _audioSource.PlayOneShot(_dropSfx);
            _objectInHand = null;
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
            
            _controls = new Controls();
            _controls.Gameplay.Drop.performed += _ => ReleaseItem();
        }

        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Disable();

    }
}