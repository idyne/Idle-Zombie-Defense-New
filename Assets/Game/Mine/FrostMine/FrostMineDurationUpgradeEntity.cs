using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Base/Mine/Frost/Frost Mine Duration Upgrade")]
public class FrostMineDurationUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * 10;
    protected override int Level { get => saveData.Value.FrostMineDurationLevel; set => saveData.Value.FrostMineDurationLevel = value; }
}
public partial class SaveData
{
    public int FrostMineDurationLevel = 1;
}