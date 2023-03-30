using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Base/Mine/Explosive/Explosive Mine Damage Upgrade")]
public class ExplosiveMineDamageUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * 10;
    protected override int Level { get => saveData.Value.ExplosiveMineDamageLevel; set => saveData.Value.ExplosiveMineDamageLevel = value; }
}
public partial class SaveData
{
    public int ExplosiveMineDamageLevel = 1;
}