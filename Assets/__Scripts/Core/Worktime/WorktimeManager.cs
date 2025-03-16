using Player.Controller;
using UnityEngine;

namespace Core.Worktime
{
    public class WorktimeManager : MonoBehaviour
    {
        public static WorktimeManager Instance { get; private set; }
        
        [Header("Settings")]
        [SerializeField] private float _timeToWork;
        [SerializeField] private int _dayStartHour = 8;
        [SerializeField] private int _dayEndHour = 21;

        private int _currentDay = 1;
        private float _currentTime = 0f;
        private bool _isWorking = false;
        private int _lastGameMinute = -1;
        
        public bool IsWorking => _isWorking;
        
        public event System.Action OnTimeUpdated;
        
        public void SetCurrentDay(int day) => _currentDay = day;
        
        public int CurrentDay => _currentDay;
        
        public string GetCurrentTime() => ConvertToGameTime(_currentTime);
        
        public void StartDay()
        {
            _currentTime = 0f;
            _isWorking = true;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
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
                EndDay();
                
            _currentTime += Time.deltaTime;
        }
        
        /// <summary>
        /// When work day ends we add +1 to current day and disable player controls
        /// </summary>
        private void EndDay()
        {
            _isWorking = false;
            _currentDay++;
            
            ControlsManager.Instance.DisableControls();
            
            Debug.Log("Work End");
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
        
    }
}


