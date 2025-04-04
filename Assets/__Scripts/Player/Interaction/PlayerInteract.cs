using Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Interaction
{
    public class PlayerInteract : MonoBehaviour
    {
        public static PlayerInteract Instance { get; private set; }
        
        [Header("Interaction Settings")]
        [SerializeField] private float _distance = 5f;
        [SerializeField] private LayerMask _interactionLayer;
        
        private Controls _controls;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
            
            _controls = new Controls();
            
        }
        private void OnEnable()
        {
            _controls.Gameplay.Enable();
            _controls.Gameplay.Interact.performed += Interact;
        }

        private void OnDisable()
        {
            _controls.Gameplay.Interact.performed -= Interact;
            _controls.Disable();
        }

        private void Interact(InputAction.CallbackContext context)
        {
            var cameraTransform = Camera.main.transform;
            if (Physics.Raycast(cameraTransform.position, 
                    cameraTransform.forward, out var hit, _distance, _interactionLayer))
            {
                if (hit.collider.TryGetComponent(out IOnceInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }
    }
}

