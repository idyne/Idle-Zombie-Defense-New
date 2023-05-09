using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DevelopmentEditScreen : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameStateVariable gameState;
    [SerializeField] private float xRange = 300;
    [SerializeField] private float yRange = 300;
    [SerializeField] private float tapImpactDuration = 1.5f;
    [SerializeField] private GameObject controlPanel = null;


    private float[] tapValidityTimes = { -1, -1, -1 };

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !controlPanel.activeSelf) CheckTapInRange();
    }

    private void CheckTapInRange()
    {
        if (Input.mousePosition.x > Screen.width - xRange && Input.mousePosition.y > Screen.height - yRange)
            Tap();
    }

    private void Tap()
    {
        UpdateOldestTapValidityTime();
        if (IsTargetTapInDurationReached()) OpenPanel();
    }

    private void UpdateOldestTapValidityTime()
    {
        float oldestValidityTime = float.MaxValue;
        int oldestValidityTimeIndex = -1;

        for (int i = 0; i < tapValidityTimes.Length; i++)
        {
            if (tapValidityTimes[i] < oldestValidityTime)
            {
                oldestValidityTime = tapValidityTimes[i];
                oldestValidityTimeIndex = i;
            }
        }
        tapValidityTimes[oldestValidityTimeIndex] = Time.unscaledTime + tapImpactDuration;
    }

    private bool IsTargetTapInDurationReached()
    {
        for (int i = 0; i < tapValidityTimes.Length; i++)
            if (tapValidityTimes[i] < Time.unscaledTime)
                return false;

        return true;
    }
    private bool paused = false;
    private void OpenPanel()
    {
        if (gameState.Value != GameState.PAUSED)
        {
            gameManager.PauseGame();
            paused = true;
        }
        controlPanel.SetActive(true);
        CleanData();
    }

    public void ClosePanel()
    {
        if (paused)
        {
            gameManager.ResumeGame();
            paused = false;
        }
        controlPanel.SetActive(false);
    }

    private void CleanData()
    {
        for (int i = 0; i < tapValidityTimes.Length; i++)
        {
            tapValidityTimes[i] = -1;
        }
    }
}
