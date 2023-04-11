using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Base/Tower/Soldier Buying Level Upgrade")]
public class SoldierBuyingLevelUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => (Level - 1) * increasePerLevel + baseCost;
    public override int Level { get => saveData.Value.SoldierBuyingLevel; protected set => saveData.Value.SoldierBuyingLevel = value; }
}
public partial class SaveData
{
    public int SoldierBuyingLevel = 1;
}