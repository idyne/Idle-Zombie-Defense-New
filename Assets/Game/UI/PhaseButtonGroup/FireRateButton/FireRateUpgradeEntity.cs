using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Upgrades/Phase Upgrades/Fire Rate Upgrade")]
public class FireRateUpgradeEntity : PhaseUpgradeEntity
{
    public override int Cost => Level * 10;

    protected override int Level { get => saveData.Value.FireRateLevel; set => saveData.Value.FireRateLevel = value; }
}

public partial class SaveData
{
    public int FireRateLevel = 1;
}