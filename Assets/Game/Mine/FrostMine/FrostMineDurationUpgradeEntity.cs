using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Base/Mine/Frost/Frost Mine Duration Upgrade")]
public class FrostMineDurationUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * increasePerLevel + baseCost;
    public override int Level { get => saveData.Value.FrostMineDurationLevel; protected set => saveData.Value.FrostMineDurationLevel = value; }
}
public partial class SaveData
{
    public int FrostMineDurationLevel = 0;
}