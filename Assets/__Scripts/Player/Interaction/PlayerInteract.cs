using Core.Input;
using UnityEngine;

namespace Player.Interaction
{
    public class PlayerInteract : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float _distance = 5f;
        [SerializeField] private LayerMask _interactionLayer;
        
        private Controls _controls;

        private void Awake()
        {
            _controls = new Controls();
            
            _controls.Gameplay.Interact.performed += _ => Interact();
        }
        
        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Disable();

        private void Interact()
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

