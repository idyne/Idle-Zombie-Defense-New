using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Base/Mine/Explosive/Explosive Mine Damage Upgrade")]
public class ExplosiveMineDamageUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * increasePerLevel + baseCost;
    public override int Level { get => saveData.Value.ExplosiveMineDamageLevel; protected set => saveData.Value.ExplosiveMineDamageLevel = value; }
}
public partial class SaveData
{
    public int ExplosiveMineDamageLevel = 0;
}