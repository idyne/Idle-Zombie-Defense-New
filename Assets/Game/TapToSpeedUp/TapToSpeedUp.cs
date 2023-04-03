using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TapToSpeedUp : MonoBehaviour
{
    [SerializeField] private FloatReference targetVariable;
    [SerializeField] GameEvent onTappedToSpeedUp;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float impactDuration = 1.5f;
    [SerializeField] private float multiplier = 2;

    private float bonusEndTime = -1;
    private bool IsInGame = false;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        targetVariable.Value = 1;
        void TapCheck() { if (!EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject == null) Tap(); }
        InputManager.GetKeyDownEvent(KeyCode.Mouse0).AddListener(TapCheck);
    }


    void Update()
    {
        if (IsInGame)
            UpdateMultiplier();
    }

    private void UpdateMultiplier()
    {
        if (Time.time < bonusEndTime)
        {
            if (targetVariable.Value == 1)
            {
                targetVariable.Value = multiplier;
                canvas.enabled = false;
            }
        }
        else
        {
            if (targetVariable.Value == multiplier)
            {
                targetVariable.Value = 1;
                canvas.enabled = true;
            }
        }
    }

    public void Tap()
    {
        onTappedToSpeedUp.Raise();
        bonusEndTime = Time.time + impactDuration;
    }

    public void OnGameStart()
    {
        IsInGame = true;
        canvas.enabled = true;
    }

    public void OnGameEnd()
    {
        IsInGame = false;
        canvas.enabled = false;
    }
}
