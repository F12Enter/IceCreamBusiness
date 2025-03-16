using Core.Input;
using UnityEngine;

namespace Player.Controller.Rotating
{
    public class PlayerRotating : MonoBehaviour, IRotatingController
    {
        [SerializeField] private Transform _cameraTransform;
        
        [Space]
        
        [SerializeField] private float _mouseSensitivity = 1f;

        [Space] 
        
        [SerializeField] private Range _xRotationLimits = new Range(-90, 90);

        private float _bodyRotationY;
        private float _cameraRotationX;
        
        private Controls _controls;
        private Vector2 _lookInput;

        private bool _isRotationLocked;
        
        public void SetSensitivity(float sensitivity) => _mouseSensitivity = sensitivity;
        
        public void LockRotation() => _isRotationLocked = true;
        
        public void UnlockRotation() => _isRotationLocked = false;
        
        private void Awake()
        {
            _controls = new Controls();
            
            _controls.Gameplay.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
            _controls.Gameplay.Look.canceled += _ => _lookInput = Vector2.zero;
        }
        
        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Disable();
        
        private void Update()
        {
            float mouseX = _isRotationLocked ? 0 : _lookInput.x * _mouseSensitivity;
            float mouseY = _isRotationLocked ? 0 : _lookInput.y * _mouseSensitivity;
            
            _bodyRotationY += mouseX;
            _cameraRotationX = Mathf.Clamp(_cameraRotationX - mouseY, _xRotationLimits.min, _xRotationLimits.max);
            
            transform.rotation = Quaternion.Euler(0, _bodyRotationY, 0);
            _cameraTransform.localRotation = Quaternion.Euler(_cameraRotationX, 0, 0);
        }
    }
}

