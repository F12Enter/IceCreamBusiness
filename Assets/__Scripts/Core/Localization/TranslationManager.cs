using System.Collections.Generic;
using Player.Controllers;
using UnityEngine;

namespace Core.Localization
{
    /// <summary>
    /// A class for managing game localization
    /// </summary>
    public class TranslationManager : MonoBehaviour
    {
        public static TranslationManager Instance { get; private set; }
        
        [SerializeField] private List<Language> _languages;
        
        private Language _currentLanguage;
        
        public event System.Action OnLanguageChanged;
        
        public string CurrentLanguage => _currentLanguage.LanguageName;
        
        public List<Language> Languages => _languages;

        public void SetLanguage(string languageCode)
        {
            _currentLanguage = _languages.Find(lang => lang.LanguageCode == languageCode);
            if (_currentLanguage == null)
            {
                Debug.LogError($"Language {languageCode} not found");
                return;
            }
            
            _currentLanguage.Initialize();
            OnLanguageChanged?.Invoke();
        }

        /// <summary>
        /// Tries to get translation of the current language with key
        /// </summary>
        /// <param name="key">Localization key</param>
        /// <returns></returns>
        public string GetTranslation(string key)
        {
            return _currentLanguage != null ? _currentLanguage.GetTranslation(key) : key;
        }
        
        /// <summary>
        /// Tries to get translation of the current language with key and arguments that will be replaced like {0}, {1} etc.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string GetTranslation(string key, params object[] args)
        {
            return _currentLanguage != null ? _currentLanguage.GetTranslation(key, args) : key;
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
            
            DontDestroyOnLoad(gameObject);

            int index = PlayerPrefs.GetInt(Settings.LanguageKey, 0);
            SetLanguage(_languages[index].LanguageCode);
        }
        
    }
}