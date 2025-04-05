using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Core.Input
{
    public class KeysManager : MonoBehaviour
    {
        [Header("Input actions keys")]
        [SerializeField] private string _actionMapName = "Gameplay";
        [SerializeField] private string _actionName;

        [Space]
        
        [Header("Composite Settings")] 
        [SerializeField] private string _compositePartName;

        [Space]
        
        [Header("UI")] 
        [SerializeField] private Button _button;
        [SerializeField] private Button _resetButton;
        [SerializeField] private TextMeshProUGUI _text;

        [Space] 
        
        private InputAction _action;
        private int _bindingIndex;

        void Start()
        {
            var controls = InputManager.Instance.Controls;
            _action = controls.asset.FindActionMap(_actionMapName)?.FindAction(_actionName);
            if (_action == null)
            {
                Debug.LogError($"Action '{_actionName}' not found in map '{_actionMapName}'");
                return;
            }

            FindBindingIndex();

            string savedPath = PlayerPrefs.GetString(GetBindingKey(), "");
            if (!string.IsNullOrEmpty(savedPath))
            {
                _action.ApplyBindingOverride(_bindingIndex, savedPath);
            }

            _text.text = GetBindingDisplay();
            
            _button.onClick.AddListener(StartRebind);
            _resetButton.onClick.AddListener(ResetToDefault);
        }
        
        private void FindBindingIndex()
        {
            _bindingIndex = -1;

            for (int i = 0; i < _action.bindings.Count; i++)
            {
                var binding = _action.bindings[i];

                if (!string.IsNullOrEmpty(_compositePartName))
                {
                    if (binding.isPartOfComposite && binding.name.ToLower() == _compositePartName.ToLower())
                    {
                        _bindingIndex = i;
                        return;
                    }
                }
                else if (!binding.isPartOfComposite)
                {
                    _bindingIndex = i;
                    return;
                }
            }

            if (_bindingIndex == -1)
                Debug.LogWarning($"Binding not found for '{_actionName}' part '{_compositePartName}'");
        }

        private void StartRebind()
        {
            _text.text = "_";

            _action.Disable();
            
            _action.PerformInteractiveRebinding(_bindingIndex)
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation =>
                {
                    string newPath = _action.bindings[_bindingIndex].effectivePath;

                    PlayerPrefs.SetString(GetBindingKey(), newPath);
                    PlayerPrefs.Save();

                    _text.text = GetBindingDisplay();
                    operation.Dispose();
                })
                .Start();
            
            _action.Enable();
        }
        
        private void ResetToDefault()
        {
            if (_action == null || _bindingIndex == -1) return;
            
            _action.RemoveBindingOverride(_bindingIndex);
            
            PlayerPrefs.DeleteKey(GetBindingKey());
            PlayerPrefs.Save();
            
            _text.text = GetBindingDisplay();
        }
        
        private string GetBindingKey()
        {
            return $"binding_{_actionMapName}_{_actionName}_{_compositePartName}";
        }

        private string GetBindingDisplay()
        {
            var displayString = _action.GetBindingDisplayString(_bindingIndex);
            return string.IsNullOrEmpty(displayString) ? "Unbound" : displayString;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
            _resetButton.onClick.RemoveAllListeners();
        }

    }
}

