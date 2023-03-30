using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class SoldierButton : PhaseButton
{
    [SerializeField] private BoolVariable isTowerFull;

    private void OnEnable()
    {
        isTowerFull.OnValueChanged.AddListener(UpdateElement);
    }
    private void OnDisable()
    {
        isTowerFull.OnValueChanged.RemoveListener(UpdateElement);
    }
    public void UpdateElement(bool a, bool b)
    {
        UpdateElement();
    }
    public override void UpdateElement()
    {
        nameText.text = phaseUpgrade.UpgradeName;
        if (phaseUpgrade.MaxedOut || isTowerFull.Value)
        {
            SwitchToInfo("MAX");
        }
        else if (phaseUpgrade.Locked)
        {
            SwitchToInfo("Locked");
        }
        else
        {
            SwitchToCost(phaseUpgrade.Cost, phaseUpgrade.Affordable);
        }
    }
}
