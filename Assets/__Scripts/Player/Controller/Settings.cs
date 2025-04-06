using Core.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Controllers
{
    public class Settings : MonoBehaviour
    {
        private const string VolumeKey = "Volume";
        public const string LanguageKey = "Language";
        
        
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private TMP_Dropdown _languagesDropdown;

        private void Start()
        {
            _volumeSlider.maxValue = 1f;
            AudioListener.volume = _volumeSlider.value;
            _volumeSlider.value = PlayerPrefs.GetFloat(VolumeKey, 1f);

            foreach (var lang in TranslationManager.Instance.Languages)
            {
                _languagesDropdown.options.Add(new TMP_Dropdown.OptionData() { text = lang.LanguageName });
            }
            
            int languageIndex = PlayerPrefs.GetInt(LanguageKey, 0); // index 0 is probably english; english is default language
            _languagesDropdown.value = languageIndex;
        }
        
        private void OnEnable()
        {
            _volumeSlider.onValueChanged.AddListener(OnSliderChanged);
            _languagesDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void OnDisable()
        {
            _volumeSlider.onValueChanged.RemoveListener(OnSliderChanged);
            _languagesDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }

        private void OnSliderChanged(float val)
        {
            AudioListener.volume = val;
            PlayerPrefs.SetFloat(VolumeKey, val);
            PlayerPrefs.Save();
        }

        private void OnDropdownValueChanged(int val)
        {
            string code = TranslationManager.Instance.Languages[val].LanguageCode;
            TranslationManager.Instance.SetLanguage(code);
            
            PlayerPrefs.SetInt(LanguageKey, val);
            PlayerPrefs.Save();
        }
    }

}
