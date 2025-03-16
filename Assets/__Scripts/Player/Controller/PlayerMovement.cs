using Core.Input;
using UnityEngine;

namespace Player.Controller
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour, IMovementController
    {
        private static float _movementMultiplier = 1f;
        
        [Header("Movement Settings")]
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _movementAcceleration = 10f;
        [SerializeField] private float _notGroundedMultiplier = 0.2f;
        [SerializeField] private float _gravityMultiplier = 3f;
        [SerializeField] private float _gravityMagnite = -1f;
        
        public static Vector3 Position { get; private set; } = Vector3.zero;
        public static Vector3 Velocity { get; private set; } = Vector3.zero;
        
        private Vector3 _inputDirection;
        private Vector3 _currentMovementVelocity;
        private float _currentVerticalVelocity;
        
        private CharacterController _controller;
        private bool _isMovementLocked = false;
        private Controls _controls;

        public static void SetMovementMultiplier(float multiplier)
        {
            _movementMultiplier = multiplier;
        }
        
        public void LockMovement() => _isMovementLocked = true;
        public void UnlockMovement() => _isMovementLocked = false;
        
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _controls = new Controls();
            
            _controls.Gameplay.Move.performed += ctx => _inputDirection = ctx.ReadValue<Vector2>();
            _controls.Gameplay.Move.canceled += _ => _inputDirection = Vector2.zero;
        }
        
        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Disable();

        private void FixedUpdate()
        {
            if (_isMovementLocked)
                _inputDirection = Vector3.zero;
            
            float speed = _movementSpeed;
            Vector3 moveDir = new Vector3(_inputDirection.x, 0, _inputDirection.y);
            Vector3 targetVelocity = (transform.forward * moveDir.z + transform.right * moveDir.x).normalized * speed;
    
            float acceleration = _movementAcceleration * Time.fixedDeltaTime * ((_controller.isGrounded) ? 1 : _notGroundedMultiplier);
    
            _currentMovementVelocity = moveDir.magnitude == 0 
                ? Vector3.Lerp(_currentMovementVelocity, Vector3.zero, acceleration * 0.5f)
                : Vector3.Lerp(_currentMovementVelocity, targetVelocity, acceleration);

            _currentVerticalVelocity = _controller.isGrounded ? _gravityMagnite : _currentVerticalVelocity += Physics.gravity.y * _gravityMultiplier * Time.fixedDeltaTime;
            
            var totalVelocity = _currentMovementVelocity + Vector3.up * _currentVerticalVelocity;
            
            Velocity = totalVelocity;
            _controller.Move(totalVelocity * (_movementMultiplier * Time.fixedDeltaTime));
            Position = transform.position;
        }

    }
}