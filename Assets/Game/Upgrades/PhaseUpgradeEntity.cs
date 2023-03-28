using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhaseUpgradeEntity : UpgradeEntity
{
    public override bool Affordable => saveData.CanAffordMoney(Cost);
    public override void BuyUpgrade()
    {
        if (!Affordable) return;
        saveData.SpendMoney(Cost);
        Upgrade();
    }
}
