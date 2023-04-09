using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Base/Turret/Turret Count")]
public class TurretCountUpgradeEntity : PreparationUpgradeEntity
{
    [SerializeField] private SoundEntity sound;
    [SerializeField] private SoundManager soundManager;
    public override int Cost => Level * 10;
    public override int Level { get => saveData.Value.TurretCount; protected set => saveData.Value.TurretCount = value; }
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
        soundManager.PlaySound(sound);
    }
}


public partial class SaveData
{
    public int TurretCount = 0;
}
