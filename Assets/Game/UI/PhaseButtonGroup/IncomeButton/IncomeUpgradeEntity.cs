using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Phase Upgrades/Income Upgrade")]
public class IncomeUpgradeEntity : PhaseUpgradeEntity
{
    public override int Cost => Level * 15;

    protected override int Level { get => saveData.Value.IncomeLevel; set => saveData.Value.IncomeLevel = value; }
}

public partial class SaveData
{
    public int IncomeLevel = 1;
}