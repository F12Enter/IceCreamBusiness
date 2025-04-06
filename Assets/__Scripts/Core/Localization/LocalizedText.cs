using System;
using TMPro;
using UnityEngine;

namespace Core.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key;
        
        private TextMeshProUGUI _text;

        private void UpdateText()
        {
            if (_text == null || TranslationManager.Instance == null) return;
            
            _text.text = TranslationManager.Instance.GetTranslation(_key);
        }
        
        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            
            TranslationManager.Instance.OnLanguageChanged += UpdateText;
            
            UpdateText();
        }
        
        
        private void OnDestroy()
        {
            TranslationManager.Instance.OnLanguageChanged -= UpdateText;
        }
        
    }
}