using System;
using Core.Economy;
using Core.OrderSystem;
using Core.SaveSystem;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Standard Settings")]
        [SerializeField] private int _moneyAmount;
        [SerializeField] private int _eachIceCreamCount;

        [Space] 
        
        [Header("Other Settings")]
        [SerializeField] private int _gameSceneIndex;
        
        [Space]
        
        [Header("UI Settings")]
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _resumeGameButton;

        /// <summary>
        /// Clears PlayerPrefs, fills in default values and starts the game scene
        /// </summary>
        private void StartNewGame()
        {
            PlayerPrefs.DeleteAll();
            
            Economy.EconomyManager.SetMoney(_moneyAmount);

            Array flavours = Enum.GetValues(typeof(Flavour));
            foreach (var flav in flavours)
            {
                Statistics.IceCreamStorage.Set((Flavour)flav, _eachIceCreamCount);
            }
            
            SceneManager.LoadScene(_gameSceneIndex);
        }

        /// <summary>
        /// Loads PlayerPrefs and starts the game scene
        /// </summary>
        private void ResumeGame()
        {
            PlayerPrefsLoader.LoadMoney();
            PlayerPrefsLoader.LoadIceCreamBalls();
            
            SceneManager.LoadScene(_gameSceneIndex);
        }
        
        private void Start()
        {
            _startGameButton.onClick.AddListener(StartNewGame);
            _resumeGameButton.onClick.AddListener(ResumeGame);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveAllListeners();
            _resumeGameButton.onClick.RemoveAllListeners();
        }
        

    
    }
}

