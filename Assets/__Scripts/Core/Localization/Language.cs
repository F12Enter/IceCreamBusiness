using System.Collections.Generic;
using UnityEngine;

namespace Core.Localization
{
    [CreateAssetMenu(menuName = "Localization/Language", fileName = "New Language")]
    public class Language : ScriptableObject
    {
        public string LanguageName;
        public string LanguageCode;
        [SerializeField] private List<TranslationPair> _translations = new();
        
        [System.NonSerialized] private Dictionary<string, string> _translationDict = null!;

        public void Initialize()
        {
            _translationDict = new Dictionary<string, string>();
            foreach (var pair in _translations)
            {
                _translationDict[pair.Key] = pair.Value;
            }
        }

        public string GetTranslation(string key)
        {
            if (_translationDict == null) Initialize();
            return _translationDict!.TryGetValue(key, out var value) ? value : $"<color=red><b>MISSING TRANSLATION FOR {key}</b></color>";
        }

        public string GetTranslation(string key, params object[] args)
        {
            if (_translationDict == null) Initialize();

            if (_translationDict.TryGetValue(key, out string template))
            {
                return string.Format(template, args);
            }
            return $"<color=red><b>MISSING TRANSLATION FOR {key}</b></color>";
        }
    }
}

