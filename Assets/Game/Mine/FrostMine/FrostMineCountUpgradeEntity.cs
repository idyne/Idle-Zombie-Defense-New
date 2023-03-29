using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Mine/Frost/Frost Mine Count Upgrade")]
public class FrostMineCountUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * 10;
    protected override int Level { get => saveData.Value.FrostMineCount; set => saveData.Value.FrostMineCount = value; }
    public override void Initialize()
    {
        FrostMine[] mines = FindObjectsOfType<FrostMine>(true);
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
    public int FrostMineCount = 0;
}