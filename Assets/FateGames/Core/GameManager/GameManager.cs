using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static WaveController;

namespace FateGames.Core
{

    [CreateAssetMenu(menuName = "Fate/Manager/GameManager")]
    public class GameManager : ScriptableObject
    {
        [SerializeField] private int targetFrameRate = -1;
        [SerializeField] private GameStateVariable gameState;
        [SerializeField] private UnityEvent OnPause, OnResume;
        private GameState stateBeforePaused = GameState.NONE;

        public void Initialize()
        {
            Application.targetFrameRate = targetFrameRate;
        }

        public void PauseGame()
        {
            if (gameState.Value == GameState.PAUSED) return;
            stateBeforePaused = gameState.Value;
            gameState.Value = GameState.PAUSED;
            Time.timeScale = 0;
            OnPause.Invoke();
        }

        public void ResumeGame()
        {
            if (gameState.Value != GameState.PAUSED) return;
            Time.timeScale = 1;
            gameState.Value = stateBeforePaused;
            OnResume.Invoke();
        }

        public void ShowInterstitial()
        {
            return;
            SDKManager.Instance.ShowInterstitial();
        }

        public void ShowBannerAd()
        {
            //SDKManager.Instance.ShowBannerAd();
        }
    }
}
