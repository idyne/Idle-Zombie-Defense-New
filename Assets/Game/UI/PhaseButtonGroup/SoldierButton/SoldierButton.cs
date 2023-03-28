using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class SoldierButton : PhaseButton
{
    [SerializeField] private BoolReference isTowerFull;
    public override void UpdateElement()
    {
        Debug.Log("UpdateElement", this);
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
