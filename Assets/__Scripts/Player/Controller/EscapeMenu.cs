using Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Player.Controller
{
    public class EscapeMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _menu;
        [SerializeField] private GameObject _warningPopup;
        
        [SerializeField] private Button _toMenuButton;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        private Controls _controls;

        private void Awake()
        {
            _controls = new Controls();
            _controls.Enable();
        }

        private void OnEnable()
        {
            _controls.UI.EscapeMenu.performed += ToggleMenu;
            _toMenuButton.onClick.AddListener(() => ShowPopup(true));
            _cancelButton.onClick.AddListener(() => ShowPopup(false));
            _confirmButton.onClick.AddListener(Leave);
        }

        private void OnDisable()
        {
            _controls.UI.EscapeMenu.performed -= ToggleMenu;
            _toMenuButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            _confirmButton.onClick.RemoveListener(Leave);
            _controls.Disable();
        }

        private void ToggleMenu(InputAction.CallbackContext ctx)
        {
            _menu.SetActive(!_menu.activeSelf);

            if (_menu.activeSelf)
            {
                CursorManager.UnlockCursor();
                ControlsManager.Instance.DisableControls();
            }
            else
            {
                CursorManager.LockCursor();
                ControlsManager.Instance.EnableControls();
                _warningPopup.SetActive(false);
            }

        }

        private void ShowPopup(bool show)
        {
            _warningPopup.SetActive(show);
        }

        private void Leave() => SceneManager.LoadScene(0);
    }
}

