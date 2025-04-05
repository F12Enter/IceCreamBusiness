using Core.SaveSystem;
using Player.Controller;
using UnityEngine;

namespace Core.Worktime
{
    public class WorktimeManager : MonoBehaviour
    {
        public static WorktimeManager Instance { get; private set; }

        [SerializeField] private GameObject _endDayMenu;

        [Header("Settings")]
        [SerializeField] private float _timeToWork;
        [SerializeField] private int _dayStartHour = 8;
        [SerializeField] private int _dayEndHour = 21;

        private int _currentDay = 1;
        private float _currentTime = 0f;
        private bool _isWorking = false;
        private int _lastGameMinute = -1;

        public int CurrentDay => _currentDay;
        
        public bool IsWorkDayNow => 0 < _currentTime && _currentTime < _timeToWork;

        public bool IsWorking => _isWorking;
        
        public event System.Action OnTimeUpdated;

        public string GetCurrentTime() => ConvertToGameTime(_currentTime);
        
        public void SetCurrentDay(int day) => _currentDay = day;
        
        /// <summary>
        /// When work day ends we add +1 to current day and disable player controls
        /// </summary>
        public void EndDay()
        {
            _currentDay++;

            Invoke(nameof(OpenMenu), 3f);

            ControlsManager.Instance.DisableControls();
            CursorManager.UnlockCursor();

            Debug.Log("Work End");
        }
        
        public void StartDay()
        {
            _currentTime = 0f;
            _isWorking = true;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;

            _currentDay = PlayerPrefsLoader.GetDays();

            CursorManager.LockCursor();
        }
        
        private void Update()
        {
            if (!_isWorking) return;
            
            //Check time changes to invoke OnTimeUpdated every minute change
            string currentTime = GetCurrentTime();
            int currentMinute = int.Parse(currentTime.Split(':')[1]);
            if (currentMinute != _lastGameMinute)
            {
                _lastGameMinute = currentMinute;
                OnTimeUpdated?.Invoke();
            }

            if (_currentTime >= _timeToWork)
                _isWorking = false;
            
                
            _currentTime += Time.deltaTime;
        }


        /// <summary>
        /// Converts seconds to game time in format HH:mm
        /// </summary>
        private string ConvertToGameTime(float time)
        {
            float totalHours = (time / _timeToWork) * (_dayEndHour - _dayStartHour);
            
            int hours = _dayStartHour + (int)totalHours;
            int minutes = (int)((totalHours - (int)totalHours) * 60);

            return $"{hours:00}:{minutes:00}";
        }

        private void OpenMenu()
        {
            _endDayMenu.SetActive(true);
        }
    }
}


