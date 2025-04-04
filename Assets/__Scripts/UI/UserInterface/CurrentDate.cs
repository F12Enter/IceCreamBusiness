using UnityEngine;
using Core.Worktime;
using UnityEngine.PlayerLoop;

namespace UI.UserInterface
{
    public class CurrentDate : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _text;

        private WorktimeManager _timeManager;
        private int _cachedDay;
        private string _cachedTime;

        private void Start()
        {
            _timeManager = WorktimeManager.Instance;
            _timeManager.OnTimeUpdated += UpdateUI;
            UpdateUI();
        }

        private void UpdateUI()
        {
            int currentDay = _timeManager.CurrentDay;
            string currentTime = _timeManager.GetCurrentTime();

            if (currentDay != _cachedDay || currentTime != _cachedTime)
            {
                _text.text = $"Day: {_timeManager.CurrentDay}. Time: {_timeManager.GetCurrentTime()}";
                _cachedDay = currentDay;
                _cachedTime = currentTime;
            }

        }
        
        private void OnDestroy() => _timeManager.OnTimeUpdated -= UpdateUI;
    }

}
