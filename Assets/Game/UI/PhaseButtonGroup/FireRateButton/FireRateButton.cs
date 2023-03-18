using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.Events;

public class FireRateButton : DynamicUIElement
{
    [SerializeField] private UnityEvent onFireRateIncreased;
    [SerializeField] private SaveDataVariable saveData;
    public override void UpdateElement()
    {
        throw new System.NotImplementedException();
    }

    public void IncreaseFireRate()
    {
        saveData.Value.FireRateLevel++;
        onFireRateIncreased.Invoke();
    }

}
