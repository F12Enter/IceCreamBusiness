using UnityEngine;

namespace Core.Localization
{
    [System.Serializable]
    public class TranslationPair
    {
        public string Key;
        [TextArea] public string Value;
    }
}