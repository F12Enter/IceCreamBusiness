using UnityEngine;
using UnityEngine.UI;

namespace Player.Controllers
{
    public class Settings : MonoBehaviour
    {
        private const string VolumeKey = "Volume";
        
        [SerializeField] private Slider _volumeSlider;

        private void Start()
        {
            _volumeSlider.maxValue = 1f;
            AudioListener.volume = _volumeSlider.value;
            
            _volumeSlider.value = PlayerPrefs.GetFloat(VolumeKey, 1f);
        }
        
        private void OnEnable()
        {
            _volumeSlider.onValueChanged.AddListener(OnSliderChanged);
        }

        private void OnDisable()
        {
            _volumeSlider.onValueChanged.RemoveListener(OnSliderChanged);
        }

        private void OnSliderChanged(float val)
        {
            AudioListener.volume = val;
            PlayerPrefs.SetFloat(VolumeKey, val);
            PlayerPrefs.Save();
        }
    }

}
