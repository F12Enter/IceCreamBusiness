using Core.Input;
using Player.Controller;
using Player.Controller.Rotating;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Interaction
{
    /// <summary>
    /// Player focusing on items system
    /// </summary>
    public class PlayerFocusing : MonoBehaviour
    {
        public static PlayerFocusing Instance { get; private set; }
        
        [SerializeField] private GameObject _warningText;
        
        public bool IsFocused => _isFocused;

        private bool _isFocused;
        private Controls _controls;
        
        private IMovementController _playerMovement;
        private IRotatingController _playerRotating;
        
        public void Focus()
        {
            _isFocused = true;
            _warningText.SetActive(true);
            
            _playerMovement.LockMovement();
            _playerRotating.LockRotation();
        }

        public void Unfocus()
        {
            _isFocused = false;
            _warningText.SetActive(false);
            
            _playerMovement.UnlockMovement();
            _playerRotating.UnlockRotation();
        }
        
        private void Unfocus(InputAction.CallbackContext _) => Unfocus();
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
            
            _playerMovement = GetComponent<PlayerMovement>();
            _playerRotating = GetComponent<PlayerRotating>();
            
            _controls = InputManager.Instance.Controls;
            _controls.UI.Enable();
            _controls.UI.EscapeMenu.performed += Unfocus;
        }

        private void OnDestroy()
        {
            _controls.UI.Disable();
            _controls.UI.EscapeMenu.performed -= Unfocus;
        }
    }
}