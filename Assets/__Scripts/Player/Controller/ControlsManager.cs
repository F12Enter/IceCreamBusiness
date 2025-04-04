using UnityEngine;
using Player.Controller.Rotating;

namespace Player.Controller
{
    public class ControlsManager : MonoBehaviour
    {
        public static ControlsManager Instance { get; private set; }

        private PlayerMovement _playerMovement;
        private PlayerRotating _playerRotating;

        public void DisableControls()
        {
            _playerMovement.LockMovement();
            _playerRotating.LockRotation();
        }

        public void EnableControls()
        {
            _playerMovement.UnlockMovement();
            _playerRotating.UnlockRotation();
        }
        
        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerRotating = GetComponent<PlayerRotating>();
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
        }
    }
}