using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Mine/Explosive/Explosive Mine Count Upgrade")]
public class ExplosiveMineCountUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * 10;
    protected override int Level { get => saveData.Value.ExplosiveMineCount; set => saveData.Value.ExplosiveMineCount = value; }
    public override void Initialize()
    {
        ExplosiveMine[] mines = FindObjectsOfType<ExplosiveMine>(true);
        int count = Mathf.Clamp(Level, 0, mines.Length);
        for (int i = 0; i < count; i++)
        {
            mines[i].Activate();
        }
    }
    public override void Upgrade()
    {
        base.Upgrade();
        Initialize();
    }
}
public partial class SaveData
{
    public int ExplosiveMineCount = 0;
}