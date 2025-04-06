using Core.Input;
using Core.Worktime;
using Player.Interaction;
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
            if (PlayerFocusing.Instance.IsFocused || WorktimeManager.Instance.GetCurrentTime() == "21:00") return;

            _menu.SetActive(!_menu.activeSelf);

            if (_menu.activeSelf)
            {
                CursorManager.UnlockCursor();
                ControlsManager.Instance.DisableControls();
                Time.timeScale = 0f;
            }
            else
            {
                CursorManager.LockCursor();
                ControlsManager.Instance.EnableControls();
                Time.timeScale = 1f;
                _warningPopup.SetActive(false);
            }

        }

        private void ShowPopup(bool show)
        {
            _warningPopup.SetActive(show);
        }

        private void Leave()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}

