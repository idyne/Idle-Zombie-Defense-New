using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Turret/Turret Count")]
public class TurretCountUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * 10;
    protected override int Level { get => saveData.Value.TurretCount; set => saveData.Value.TurretCount = value; }
    public override void Initialize()
    {
        Turret[] turrets = FindObjectsOfType<Turret>(true);
        int count = Mathf.Clamp(Level, 0, turrets.Length);
        for (int i = 0; i < count; i++)
        {
            turrets[i].Activate();
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
    public int TurretCount = 0;
}
