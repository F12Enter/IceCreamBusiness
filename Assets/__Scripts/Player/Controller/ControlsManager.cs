using UnityEngine;

namespace Player.Controller
{
    public class ControlsManager : MonoBehaviour
    {
        public static ControlsManager Instance { get; private set; }

        private PlayerMovement _playerMovement;

        public void DisableControls()
        {
            _playerMovement.LockMovement();
        }

        public void EnableControls()
        {
            _playerMovement.UnlockMovement();
        }
        
        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
        }
    }
}