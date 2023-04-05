using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Phase Upgrades/Merge Upgrade")]
public class MergeUpgradeEntity : PhaseUpgradeEntity
{
    public override int Cost => Level * 20;
    protected override int Level { get => saveData.Value.MergeCount; set => saveData.Value.MergeCount = value; }
}
public partial class SaveData
{
    public int MergeCount = 0;
}