using System.Collections;
using Core.Economy;
using Core.SaveSystem;
using Core.Statistics;
using Player.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Worktime
{
    public class WorktimeManager : MonoBehaviour
    {
        public static WorktimeManager Instance { get; private set; }

        [SerializeField] private Image _endMenuBg;
        [SerializeField] private GameObject _endDayMenu;

        [Header("Settings")]
        [SerializeField] private float _timeToWork;
        [SerializeField] private int _dayStartHour = 8;
        [SerializeField] private int _dayEndHour = 21;

        [Header("Visual effects")] 
        [SerializeField] private Material _skyboxMat;
        [SerializeField] private Light _sunLight;
        [SerializeField] private Color _morningColor;
        [SerializeField] private Color _eveningColor;

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

            StartCoroutine(ShowEndDayMenu(3f));

            ControlsManager.Instance.DisableControls();
            CursorManager.UnlockCursor();
            
            Debug.Log("Work End");
        }
        
        
        public void StartDay()
        {
            _currentTime = 0f;
            _isWorking = true;
            
            RenderSettings.skybox.SetFloat("_BlendFactor", 0);
            _sunLight.color = _morningColor;
        }

        private IEnumerator ShowEndDayMenu(float timeToShow)
        {
            float elapsedTime = 0f;
            
            Color startColor = _endMenuBg.color;
            startColor.a = 0f;
            _endMenuBg.color = startColor;
            
            Color endColor = _endMenuBg.color;
            endColor.a = 1f;

            while (elapsedTime < timeToShow)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / timeToShow);
                
                _endMenuBg.color = Color.Lerp(startColor, endColor, t);

                yield return null;
            }
            
            _endMenuBg.color = endColor;
            _endDayMenu.SetActive(true);
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
            
            // visual effects
            float blendFactor = Mathf.Clamp01(_currentTime / _timeToWork);
            _skyboxMat.SetFloat("_BlendFactor", blendFactor);
            _sunLight.color = Color.Lerp(_morningColor, _eveningColor, blendFactor);
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


