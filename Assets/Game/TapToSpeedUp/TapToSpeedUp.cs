using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TapToSpeedUp : MonoBehaviour
{
    [SerializeField] private FloatReference targetVariable;
    [SerializeField] GameEvent soldierGlow;
    [SerializeField] private float impactDuration = 1.5f;
    [SerializeField] private float multiplier = 2;

    private float bonusEndTime = -1;

    private void Start()
    {
        targetVariable.Value = 1;
        InputManager.GetKeyDownEvent(KeyCode.Mouse0).AddListener(TapCheck);
    }

    void Update()
    {
        UpdateMultiplier();
    }

    private void UpdateMultiplier()
    {
        if (Time.time < bonusEndTime)
        {
            if (targetVariable.Value == 1)
                targetVariable.Value = multiplier;
        }
        else
        {
            if (targetVariable.Value == 2)
                targetVariable.Value = 1;
        }
            
    }

    private void TapCheck()
    {
        if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null) return;
        Tap();
    }

    public void Tap()
    {
        soldierGlow.Raise();
        bonusEndTime = Time.time + impactDuration;
    }
}
