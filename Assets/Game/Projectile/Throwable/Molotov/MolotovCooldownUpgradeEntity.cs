using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Commander/Molotov/Molotov Cooldown Upgrade")]
public class MolotovCooldownUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * increasePerLevel + baseCost;

    public override int Level { get => saveData.Value.MolotovCooldownLevel; protected set => saveData.Value.MolotovCooldownLevel = value; }
}

public partial class SaveData
{
    public int MolotovCooldownLevel = 0;
}