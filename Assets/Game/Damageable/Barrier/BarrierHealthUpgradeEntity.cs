using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Base/Barrier/Barrier Health Upgrade")]
public class BarrierHealthUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * increasePerLevel + baseCost;
    public override int Level { get => saveData.Value.BarrierHealthLevel; protected set => saveData.Value.BarrierHealthLevel = value; }
}
public partial class SaveData
{
    public int BarrierHealthLevel = 0;
}