using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Upgrades/Phase Upgrades/Soldier Buying Upgrade")]
public class SoldierBuyUpgradeEntity : PhaseUpgradeEntity
{
    public override int Cost => Level * 10;

    protected override int Level { get => saveData.Value.SoldierBuyingCount; set => saveData.Value.SoldierBuyingCount = value; }
}

public partial class SaveData
{
    public int SoldierBuyingCount = 0;
}