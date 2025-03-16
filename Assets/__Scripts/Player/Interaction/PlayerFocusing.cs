using Player.Controller;
using Player.Controller.Rotating;
using UnityEngine;

namespace Player.Interaction
{
    /// <summary>
    /// Player focusing on items system
    /// </summary>
    public class PlayerFocusing : MonoBehaviour
    {
        public static PlayerFocusing Instance { get; private set; }
        
        public bool IsFocused => _isFocused;

        private bool _isFocused;
        
        private IMovementController _playerMovement;
        private IRotatingController _playerRotating;
        
        public void Focus()
        {
            _isFocused = true;
            
            _playerMovement.LockMovement();
            _playerRotating.LockRotation();
        }

        public void Unfocus()
        {
            _isFocused = false;
            
            _playerMovement.UnlockMovement();
            _playerRotating.UnlockRotation();
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
            
            _playerMovement = GetComponent<PlayerMovement>();
            _playerRotating = GetComponent<PlayerRotating>();
        }
    }
}