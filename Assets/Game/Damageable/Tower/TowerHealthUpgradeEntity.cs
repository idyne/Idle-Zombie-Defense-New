using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Base/Tower/Tower Health Upgrade")]
public class TowerHealthUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * increasePerLevel + baseCost;
    public override int Level { get => saveData.Value.TowerHealthLevel; protected set => saveData.Value.TowerHealthLevel = value; }
}
public partial class SaveData
{
    public int TowerHealthLevel = 0;
}