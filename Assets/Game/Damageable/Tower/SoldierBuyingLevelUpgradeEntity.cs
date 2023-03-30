using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Base/Tower/Soldier Buying Level Upgrade")]
public class SoldierBuyingLevelUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * 10;
    protected override int Level { get => saveData.Value.SoldierBuyingLevel; set => saveData.Value.SoldierBuyingLevel = value; }
}
public partial class SaveData
{
    public int SoldierBuyingLevel = 1;
}